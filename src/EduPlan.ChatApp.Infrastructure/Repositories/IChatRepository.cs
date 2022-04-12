using EduPlan.ChatApp.Domain;

namespace EduPlan.ChatApp.Infrastructure.Repositories;

public interface IChatRepository : IRepository<Chat>
{
    Task<Chat> CreateOneToOneChat(int FromUserId, int toUserId);

    Task<IEnumerable<Chat>> GetChats(int userId);
}
