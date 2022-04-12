using EduPlan.ChatApp.Api.Models;
using EduPlan.ChatApp.Domain;

namespace EduPlan.ChatApp.Api.Services;

public interface IMessageService
{
    Task<IEnumerable<MessageDTO>> Get(int chatId, int userId);

    Task Create(MessageDTO message);
}
