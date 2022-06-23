using EduPlan.ChatApp.Common.Exceptions;
using EduPlan.ChatApp.Domain;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EduPlan.ChatApp.Infrastructure.Repositories;

public class ChatRepository : AbstractRepository<Chat>, IChatRepository
{
    private readonly ILogger logger = Log.ForContext<ChatRepository>();

    private readonly DbSet<ApplicationUser> users;

    public ChatRepository(ChatAppDbContext context) : base(context)
    {
        users = context.Set<ApplicationUser>();
    }

    public async Task<Chat> CreateOneToOneChat(Chat chat)
    {
        try
        {
            dbContext.Add(chat);
            dbContext.SaveChanges();

            logger.Information($"Chat with id: {chat.Id} has been created.");

            var createdChat = await dbSet
           .Include(p => p.ChatParticipants)
           .ThenInclude(p => p.User)
           .Where(c => c.Id == chat.Id)
           .SingleOrDefaultAsync();

            return createdChat;
        }
        catch (DbUpdateException exception) when (exception.InnerException != null)
        {
            if (exception.InnerException.Message.Contains("FK_ChatParticipant_AspNetUsers_UserId"))
            {
                throw new ChatAppUserDoesNotExistException("User does not exist");
            }

            throw;
        }
    }

    public async Task<IEnumerable<Chat>> GetChats(int userId)
    {
        List<Chat>? result = await dbSet
            .Include(p => p.ChatParticipants)
            .ThenInclude(p => p.User)
            .Where(x => x.ChatParticipants.Any(cp => cp.UserId == userId))
            .ToListAsync();

        return result;
    }
}
