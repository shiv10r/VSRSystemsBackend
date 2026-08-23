using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;

namespace VSRSystemsBackend.Api.Infrastructure.Caching;

/// <summary>
/// Distributed cache that prefers Redis but transparently falls back to an in-process
/// memory cache when Redis is unavailable, so the application keeps working on any host.
/// It uses a short cooldown before retrying Redis after a failure (circuit-breaker).
/// </summary>
public sealed class ResilientDistributedCache : IDistributedCache
{
    private readonly RedisCache? _redis;
    private readonly IMemoryCache _memory;
    private readonly object _gate = new();
    private bool _redisHealthy = true;
    private DateTimeOffset _nextRetry = DateTimeOffset.MinValue;
    private static readonly TimeSpan RetryCooldown = TimeSpan.FromSeconds(30);

    public ResilientDistributedCache(IConfiguration configuration, IMemoryCache memoryCache)
    {
        _memory = memoryCache;
        var connection = configuration["Redis:Configuration"] ?? "localhost:6379";
        var instanceName = configuration["Redis:InstanceName"] ?? "vsr:";
        try
        {
            _redis = new RedisCache(new RedisCacheOptions
            {
                Configuration = connection,
                InstanceName = instanceName
            });
        }
        catch
        {
            _redis = null;
        }
    }

    private bool RedisAvailable()
    {
        if (_redis is null) return false;
        lock (_gate)
        {
            if (_redisHealthy) return true;
            if (DateTimeOffset.UtcNow >= _nextRetry)
            {
                _redisHealthy = true;
                return true;
            }
            return false;
        }
    }

    private void OnRedisFailure()
    {
        lock (_gate)
        {
            _redisHealthy = false;
            _nextRetry = DateTimeOffset.UtcNow.Add(RetryCooldown);
        }
    }

    private static MemoryCacheEntryOptions ToMemoryOptions(DistributedCacheEntryOptions options) => new()
    {
        AbsoluteExpiration = options.AbsoluteExpiration,
        AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow,
        SlidingExpiration = options.SlidingExpiration
    };

    public byte[]? Get(string key)
    {
        if (RedisAvailable())
        {
            try
            {
                var value = _redis!.Get(key);
                if (value is not null)
                {
                    _redisHealthy = true;
                    return value;
                }
            }
            catch
            {
                OnRedisFailure();
            }
        }
        return _memory.Get<byte[]?>(key);
    }

    public async Task<byte[]?> GetAsync(string key, CancellationToken token = default)
    {
        if (RedisAvailable())
        {
            try
            {
                var value = await _redis!.GetAsync(key, token).ConfigureAwait(false);
                if (value is not null)
                {
                    _redisHealthy = true;
                    return value;
                }
            }
            catch
            {
                OnRedisFailure();
            }
        }
        return _memory.Get<byte[]?>(key);
    }

    public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
    {
        if (RedisAvailable())
        {
            try
            {
                _redis!.Set(key, value, options);
                _redisHealthy = true;
                return;
            }
            catch
            {
                OnRedisFailure();
            }
        }
        _memory.Set(key, value, ToMemoryOptions(options));
    }

    public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
    {
        if (RedisAvailable())
        {
            try
            {
                await _redis!.SetAsync(key, value, options, token).ConfigureAwait(false);
                _redisHealthy = true;
                return;
            }
            catch
            {
                OnRedisFailure();
            }
        }
        _memory.Set(key, value, ToMemoryOptions(options));
    }

    public void Refresh(string key)
    {
        if (RedisAvailable())
        {
            try
            {
                _redis!.Refresh(key);
                _redisHealthy = true;
                return;
            }
            catch
            {
                OnRedisFailure();
            }
        }
    }

    public async Task RefreshAsync(string key, CancellationToken token = default)
    {
        if (RedisAvailable())
        {
            try
            {
                await _redis!.RefreshAsync(key, token).ConfigureAwait(false);
                _redisHealthy = true;
                return;
            }
            catch
            {
                OnRedisFailure();
            }
        }
    }

    public void Remove(string key)
    {
        _memory.Remove(key);
        if (RedisAvailable())
        {
            try
            {
                _redis!.Remove(key);
                _redisHealthy = true;
            }
            catch
            {
                OnRedisFailure();
            }
        }
    }

    public async Task RemoveAsync(string key, CancellationToken token = default)
    {
        _memory.Remove(key);
        if (RedisAvailable())
        {
            try
            {
                await _redis!.RemoveAsync(key, token).ConfigureAwait(false);
                _redisHealthy = true;
            }
            catch
            {
                OnRedisFailure();
            }
        }
    }
}
