using VSRSystemsBackend.Application.Platform.Chat;
using VSRSystemsBackend.Infrastructure.Platform.Chat;

namespace VSRSystemsBackend.Api.Platform.Chat;

public static class ChatRegistration
{
    public static IServiceCollection AddChat(this IServiceCollection services)
    {
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IChatContextAuthorizer, HomeServicesChatContextAuthorizer>();
        services.AddSingleton<IChatMessageRepository, MongoChatMessageRepository>();
        return services;
    }
}
