using EduPlan.ChatApp.Domain;
using EduPlan.ChatApp.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace EduPlan.ChatApp.IntegrationTest;

[TestFixture]
public class TestBase
{
    private const string SqlServerAddress = "127.0.0.1";
    private const string SqlServerPort = "20031";
    private const string SqlServerDatabaseName = "chatapp_test";
    private const string SqlServerUserId = "SA";
    private const string SqlServerPassword = "Your_password123";

    private const string ConnectionString = $"Server={SqlServerAddress},{SqlServerPort};Database={SqlServerDatabaseName};User Id={SqlServerUserId};Password={SqlServerPassword};MultipleActiveResultSets=true";

    protected ChatAppDbContext dbContext;

    protected DbSet<ApplicationUser> users;
    protected DbSet<Chat> chats;
    protected DbSet<Message> messages;
    protected DbSet<ChatParticipant> chatParticipants;


    [SetUp]
    public void SetUp()
    {
        var builder = new DbContextOptionsBuilder<ChatAppDbContext>()
            .UseSqlServer(ConnectionString,
            x => x.MigrationsAssembly("EduPlan.ChatApp.Infrastructure"));

        DbContextOptions options = builder.Options;

        dbContext = new ChatAppDbContext(options);

        try
        {
            dbContext.Database.Migrate();
        }
        catch(SqlException ex)
        {
            if (ex.Message.Contains("The server was not found or was not accessible"))
            {
                Assert.Fail($"Could not connect to the Database. " +
                    $"Ensure SQL Server is running at {SqlServerAddress}:{SqlServerPort}.");
            }
        }

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

    protected ApplicationUser CreateUserInDatabase(string? name = null)
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
