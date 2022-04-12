using EduPlan.ChatApp.Domain;
using EduPlan.ChatApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EduPlan.ChatApp.Infrastructure;

public class MessageRepository : AbstractRepository<Message>, IMessageRepository
{
    public MessageRepository(ChatAppDbContext context) : base(context) { }

    public async Task<IEnumerable<Message>> GetMessagesForUserId(int userId, bool sorted = true)
    {
        IQueryable<Message> queryable =  this.dbSet.Where(x => x.FromId == userId || x.ToId == userId);
        if (sorted)
        {
            return await queryable.OrderByDescending(x => x.CreatedAt).ToListAsync();
        }

        return await queryable.ToListAsync();
    }

    public async Task<IEnumerable<Message>> GetMessagesForChatId(int chatId)
    {
        var query = this.dbSet.Where(x => x.ChatId == chatId).OrderByDescending(x => x.CreatedAt);

        return await query.ToListAsync();
    }
}
