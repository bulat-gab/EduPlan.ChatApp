using EduPlan.ChatApp.Domain;

namespace EduPlan.ChatApp.Infrastructure.Repositories;

public interface IMessageRepository : IRepository<Message>
{
    /// <summary>
    /// Retreives all messages sent by the userId.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="sorted">Default value is true. Setting value to true guarantees that the messages
    /// will be sorted by the CreatedAt field in descending order, i.e. from the most recent messages to the oldest
    /// </param>
    /// <returns></returns>
    Task<IEnumerable<Message>> GetMessagesForUserId(int userId, bool sorted = true);

    Task<IEnumerable<Message>> GetMessagesForChatId(int chatId);
}
