using EduPlan.ChatApp.Api.Exceptions;
using EduPlan.ChatApp.Api.Models;
using EduPlan.ChatApp.Domain;
using EduPlan.ChatApp.Infrastructure.Repositories;
using Serilog;

namespace EduPlan.ChatApp.Api.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository messageRepository;
    private readonly Serilog.ILogger logger = Log.ForContext<MessageService>();

    public MessageService(IMessageRepository messageRepository)
    {
        this.messageRepository = messageRepository;
    }

    public async Task Create(MessageDTO messageDTO)
    {
        if (messageDTO.FromId == messageDTO.ToId)
        {
            logger.Error($"Cannot send messages to yourself. UserId: {messageDTO.FromId}");
            throw new ChatAppInvalidInputException($"Cannot send messages to yourself. UserId: {messageDTO.FromId}");
        }

        try
        {
            // TODO: Ensure recipient exists
            var message = new Message(messageDTO.ChatId, messageDTO.FromId, messageDTO.ToId, messageDTO.Text);
            var result = await messageRepository.CreateAsync(message);
        }
        catch (Exception exception)
        {
            logger.Error(exception, "Message creation failed");
            throw new ChatAppException("Message creation failed", exception);
        }
    }

    public async Task<IEnumerable<MessageDTO>> Get(int chatId, int userId)
    {
        try
        {
            var messages = await messageRepository.GetMessagesForChatId(chatId);
            logger.Information($"Found {messages.Count()} messages. ChatId: {chatId}");

            return messages.Select(x => new MessageDTO
            {
                ChatId = chatId,
                FromId = x.FromId,
                ToId = x.ToId,
                Text = x.Text,
                CreatedAt = x.CreatedAt,
            });

        }
        catch (Exception exception)
        {
            string errorMessage = $"Failed to get the messages for chatId {chatId}";
            this.logger.Error(exception, errorMessage);
            throw new ChatAppException(errorMessage, exception);
        }
    }

    public async Task<IEnumerable<ChatDTO>> GetChats(int userId)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception exception)
        {
            string errorMessage = $"Failed to get the messages for userId {userId}";
            this.logger.Error(exception, errorMessage);
            throw new ChatAppException(errorMessage, exception);
        }
    }
}
