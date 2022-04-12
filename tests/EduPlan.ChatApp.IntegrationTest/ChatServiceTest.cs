using EduPlan.ChatApp.Api.Services;
using EduPlan.ChatApp.Common.Exceptions;
using EduPlan.ChatApp.Infrastructure.Repositories;
using NUnit.Framework;

namespace EduPlan.ChatApp.IntegrationTest;

[TestFixture]
public class ChatServiceTest : TestBase
{
    private IChatService chatService;
    private IChatRepository chatRepository;

    [SetUp]
    public void SetUp()
    {
        base.SetUp();

        chatRepository = new ChatRepository(dbContext);
        chatService = new ChatService(chatRepository);
    }

    [Test]
    public async Task ChatService_Create_ShouldReturnNewChat()
    {
        var (user1, user2) = base.CreateTwoUsersInDatabse();

        var chat = await chatService.Create(user1.Id, user2.Id);

        Assert.IsNotNull(chat);
        Assert.AreEqual(1, chat.Id);
        Assert.That(chat.CreatedAt, Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromMinutes(1)));
        Assert.AreEqual(user1.Id, chat.CreatedBy);
        Assert.NotNull(chat.ChatParticipants);
        Assert.AreEqual(2, chat.ChatParticipants.Count());

        var chatParticipants = chat.ChatParticipants.ToList();
        var first = chatParticipants[0];
        var second = chatParticipants[1];

        Assert.AreEqual(user1.Id, first.UserId);
        Assert.AreEqual(user2.Id, second.UserId);
    }

    [Test]
    public async Task ChatService_Create_ShouldReturnError_WhenUsersDontExist()
    {
        Assert.ThrowsAsync<ChatAppUserDoesNotExistException>(async () => await chatService.Create(1, 2));
    }

    [Test]
    public async Task GetAll_ShouldReturnAllChatsForUser()
    {
        var user1 = base.CreateUserInDatabase("mark");
        var user2 = base.CreateUserInDatabase("bulat");
        var user3 = base.CreateUserInDatabase("oleg");

        var chatUser1ToUser2 = base.CreateOneToOneChat(user1, user2);
        var chatUser1ToUser3 = base.CreateOneToOneChat(user1, user3);

        var chatUser2ToUser3 = base.CreateOneToOneChat(user2, user3);

        var chatDTOs = (await chatService.GetAll(user1.Id)).ToList();

        Assert.AreEqual(2, chatDTOs.Count());

        var chat1 = chatDTOs[0];
        Assert.AreEqual(user1.Id, chat1.User1.Id);
        Assert.AreEqual(user2.Id, chat1.User2.Id);

        var chat2 = chatDTOs[1];
        Assert.AreEqual(user1.Id, chat2.User1.Id);
        Assert.AreEqual(user3.Id, chat2.User2.Id);
    }
}
