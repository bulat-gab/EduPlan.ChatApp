using EduPlan.ChatApp.Domain;

namespace EduPlan.ChatApp.Api.Services;

public interface IChatService
{
    Task<Chat> Create(int currentUserId, int userId);
}
