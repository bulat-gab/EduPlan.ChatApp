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

    public Task<Chat> CreateOneToOneChat(int userId1, int userId2)
    {
        var chatUsers = users.Where(x => x.Id == userId1 || x.Id == userId2);

        var chat = new Chat($"{userId1} -> {userId2}", ChatType.Private);

        dbContext.Add(chat);
        dbContext.SaveChanges();

        logger.Information($"Chat with id: {chat.Id} has been created.");

        var user1 = users.Single(x => x.Id == userId1);
        var user2 = users.Single(x => x.Id == userId2);

        var chatParticipant1 = new ChatParticipant
        {
            Chat = chat,
            User = user1,
        };
        var chatParticipant2 = new ChatParticipant
        {
            Chat = chat,
            User = user2,
        };

        chat.ChatParticipants = new List<ChatParticipant>();
        chat.ChatParticipants.Add(chatParticipant1);
        chat.ChatParticipants.Add(chatParticipant2);

        dbContext.SaveChanges();

        return Task.FromResult(chat);
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
