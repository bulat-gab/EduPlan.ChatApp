using EduPlan.ChatApp.Common.Exceptions;
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

    public async Task<IEnumerable<Message>> GetMessagesForChatId(int chatId, int userId)
    {
        var query = this.dbSet.Where(x => x.ChatId == chatId && (x.FromId == userId || x.ToId == userId))
            .OrderByDescending(x => x.CreatedAt);
        var result = await query.ToListAsync();

        return result;
    }

    public new async Task<Message> Create(Message entity)
    {
        try
        {
            dbSet.Add(entity);
            await this.dbContext.SaveChangesAsync();
        }
        catch(DbUpdateException exception) when (exception.InnerException != null)
        {
            if (exception.InnerException.Message.Contains("FK_Message_AspNetUsers_FromId"))
            {
                throw new ChatAppUserDoesNotExistException($"User {entity.FromId} does not exist.");
            }

            if (exception.InnerException.Message.Contains("FK_Message_AspNetUsers_ToId"))
            {
                throw new ChatAppUserDoesNotExistException($"User {entity.FromId} does not exist.");
            }

            if (exception.InnerException.Message.Contains("FK_Message_Chat_ChatId"))
            {
                throw new ChatAppChatDoesNotExistException($"Chat between users {entity.FromId} and {entity.ToId} does not exist.");
            }

            throw;
        }
        
        return entity;
    }
}
