using EduPlan.ChatApp.Domain;
using Serilog;

namespace EduPlan.ChatApp.Api.Services;

public class ChatService : IChatService
{
    private readonly Serilog.ILogger logger = Log.ForContext<ChatService>();

    public Task<Chat> Create(string currentUserId, int userId)
    {
        logger.Information($"Chat between {currentUserId} and {userId} has been created.");

        return Task.FromResult(new Chat());
    }
}
