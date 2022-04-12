using EduPlan.ChatApp.Domain;

namespace EduPlan.ChatApp.Infrastructure.Repositories;

public interface IChatRepository : IRepository<Chat>
{
    Task<Chat> CreateOneToOneChat(Chat chat);

    Task<IEnumerable<Chat>> GetChats(int userId);
}
