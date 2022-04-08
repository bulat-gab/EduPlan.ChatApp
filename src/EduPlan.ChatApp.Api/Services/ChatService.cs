using EduPlan.ChatApp.Domain;
using EduPlan.ChatApp.Infrastructure.Repositories;
using Serilog;

namespace EduPlan.ChatApp.Api.Services;

public class ChatService : IChatService
{
    private readonly Serilog.ILogger logger = Log.ForContext<ChatService>();
    private readonly IChatRepository chatRepository;

    public ChatService(IChatRepository chatRepository)
    {
        this.chatRepository = chatRepository;
    }

    public async Task<Chat> Create(int currentUserId, int userId)
    {
        try
        {
            var createdChat = await chatRepository.CreateOneToOneChat(currentUserId, userId);
            logger.Information($"Chat between {currentUserId} and {userId} has been created.");

            return createdChat;
        }
        catch (Exception exception)
        {
            logger.Error(exception, $"Failed to create a chat.");
            return null;
        }
    }
}
