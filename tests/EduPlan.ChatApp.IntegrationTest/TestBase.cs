using EduPlan.ChatApp.Domain;
using EduPlan.ChatApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace EduPlan.ChatApp.IntegrationTest;

[TestFixture]
public class TestBase
{
    protected ChatAppDbContext dbContext;

    protected DbSet<ApplicationUser> users;
    protected DbSet<Chat> chats;
    protected DbSet<Message> messages;
    protected DbSet<ChatParticipant> chatParticipants;


    [SetUp]
    public void SetUp()
    {
        var builder = new DbContextOptionsBuilder<ChatAppDbContext>()
            .UseSqlServer("Server=127.0.0.1,20031;Database=chatapp_test;User Id=SA;Password=Your_password123;MultipleActiveResultSets=true",
            x => x.MigrationsAssembly("EduPlan.ChatApp.Infrastructure"));

        DbContextOptions options = builder.Options;

        dbContext = new ChatAppDbContext(options);
        dbContext.Database.Migrate();

        users = dbContext.Set<ApplicationUser>();
        chats = dbContext.Set<Chat>();
        messages = dbContext.Set<Message>();
        chatParticipants = dbContext.Set<ChatParticipant>();
    }

    [TearDown]
    public void TearDown()
    {
        dbContext.Database.EnsureDeleted();
        dbContext = null;
    }

    protected (ApplicationUser user1, ApplicationUser user2) CreateTwoUsersInDatabse()
    {
        var user1 = new ApplicationUser
        {
            Id = 1,
            Email = "bulat@gmail.com",
            UserName = "bulat@gmail.com",
            CreatedAt = DateTime.UtcNow.AddMinutes(-10),
        };

        var user2 = new ApplicationUser
        {
            Id = 2,
            Email = "mark@gmail.com",
            UserName = "mark@gmail.com",
            CreatedAt = DateTime.UtcNow,
        };

        using (var transaction = dbContext.Database.BeginTransaction())
        {
            dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.AspNetUsers ON");
            dbContext.SaveChanges();

            users.Add(user1);
            users.Add(user2);

            dbContext.SaveChanges();

            dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.AspNetUsers OFF");
            dbContext.SaveChanges();
            transaction.Commit();
        }

        return (user1, user2);
    }

    protected ApplicationUser CreateUserInDatabase(string? name)
    {
        string generatedName = name ?? Guid.NewGuid().ToString().Substring(10);

        var user = new ApplicationUser
        {
            Email = generatedName,
            UserName = generatedName,
            CreatedAt = DateTime.UtcNow,
        };

        users.Add(user);
        dbContext.SaveChanges();
        
        return user;
    }

    protected Chat CreateOneToOneChat(ApplicationUser user1, ApplicationUser user2)
    {
        var chat = new Chat($"{user1.Id} -> {user2.Id}", ChatType.Private, user1.Id);

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

        chat.ChatParticipants = new List<ChatParticipant> { chatParticipant1, chatParticipant2 };

        dbContext.Add(chat);
        dbContext.SaveChanges();

        return chat;
    }
}
