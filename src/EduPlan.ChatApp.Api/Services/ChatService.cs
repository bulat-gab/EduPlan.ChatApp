using EduPlan.ChatApp.Api.Models;
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

    public async Task<IEnumerable<ChatDTO>> GetAll(int userId)
    {
        var chats = await chatRepository.GetChats(userId);

        var chatDTOs = new List<ChatDTO>();

        foreach (var chat in chats)
        {
            var chatDTO = new ChatDTO
            {
                Id = chat.Id,
                Name = chat.Name,
                Type = chat.ChatType
            };

            var user1 = chat.ChatParticipants.FirstOrDefault(x => x.UserId == userId).User;
            chatDTO.User1 = new UserDTO
            {
                Id = user1.Id,
                Email = user1.Email,
                Username = user1.Email,
            };

            var user2 = chat.ChatParticipants.FirstOrDefault(x => x.UserId != userId).User;
            chatDTO.User2 = new UserDTO
            {
                Id = user2.Id,
                Email = user2.Email,
                Username = user2.Email,
            };

            chatDTOs.Add(chatDTO);
        }

        return chatDTOs;
    }
}
