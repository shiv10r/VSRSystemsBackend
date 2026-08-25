using VSRSystemsBackend.Application.Platform.Chat;
using VSRSystemsBackend.Infrastructure.Platform.Chat;

namespace VSRSystemsBackend.Api.Platform.Chat;

public static class ChatRegistration
{
    public static IServiceCollection AddChat(this IServiceCollection services)
    {
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<HomeServicesChatContextAuthorizer>();
        services.AddScoped<IChatContextAuthorizer, ModuleChatContextAuthorizer>();
        services.AddSingleton<IChatMessageRepository, MongoChatMessageRepository>();
        return services;
    }
}
