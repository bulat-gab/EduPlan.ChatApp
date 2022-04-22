using EduPlan.ChatApp.Api.Models;
using EduPlan.ChatApp.Api.Services;
using EduPlan.ChatApp.Common.Exceptions;
using EduPlan.ChatApp.Domain;
using EduPlan.ChatApp.Infrastructure;
using NUnit.Framework;

namespace EduPlan.ChatApp.IntegrationTest;

[TestFixture]
public class MessageServiceTest : TestBase
{
    private MessageRepository messageRepository;
    private MessageService messageService;

    [SetUp]
    public void SetUp()
    {
        base.SetUp();

        messageRepository = new MessageRepository(dbContext);
        messageService = new MessageService(messageRepository);
    }

    [Test]
    public async Task Create_ShouldReturn_MessageDTO()
    {
        var user1 = base.CreateUserInDatabase();
        var user2 = base.CreateUserInDatabase();
        var chat = base.CreateOneToOneChat(user1, user2);

        var messageDTO = new MessageDTO
        {
            FromId = user1.Id,
            ToId = user2.Id,
            ChatId = chat.Id,
            Text = "Hello",
        };

        var actualMessage = await messageService.Create(messageDTO);

        Assert.NotNull(actualMessage);
        Assert.AreEqual(actualMessage.ChatId, chat.Id);
        Assert.AreEqual(actualMessage.Text, actualMessage.Text);
        Assert.AreEqual(actualMessage.FromId, user1.Id);
        Assert.AreEqual(actualMessage.ToId, user2.Id);
        Assert.That(actualMessage.CreatedAt, Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromMinutes(1)));
    }

    [Test]
    public async Task Create_ShouldThrow_WhenSenderDoesNotExist()
    {
        var userId1 = 1;
        var userId2 = 2;
        var chat = new Chat($"{userId1} -> {userId2}", ChatType.Private, userId1);
        dbContext.Add(chat);
        dbContext.SaveChanges();

        var messageDTO = new MessageDTO
        {
            FromId = userId1,
            ToId = userId2,
            ChatId = chat.Id,
            Text = "Hello",
        };

        Assert.ThrowsAsync<ChatAppUserDoesNotExistException>(
            async () => await messageService.Create(messageDTO), $"User {userId1} does not exist.");
    }

    [Test]
    public async Task Create_ShouldThrow_WhenRecepientDoesNotExist()
    {
        var user1 = base.CreateUserInDatabase();
        var userId2 = 2;
        
        var chat = new Chat($"{user1.Id} -> {userId2}", ChatType.Private, user1.Id);
        dbContext.Add(chat);
        dbContext.SaveChanges();

        var messageDTO = new MessageDTO
        {
            FromId = user1.Id,
            ToId = userId2,
            ChatId = chat.Id,
            Text = "Hello",
        };

        Assert.AreNotEqual(user1.Id, userId2);

        Assert.ThrowsAsync<ChatAppUserDoesNotExistException>(
            async () => await messageService.Create(messageDTO), $"User {userId2} does not exist.");
    }

    [Test]
    public async Task Create_ShouldThrow_WhenChatDoesNotExist()
    {
        var user1 = base.CreateUserInDatabase();
        var user2 = base.CreateUserInDatabase();

        var messageDTO = new MessageDTO
        {
            FromId = user1.Id,
            ToId = user2.Id,
            ChatId = 1,
            Text = "Hello",
        };

        Assert.ThrowsAsync<ChatAppChatDoesNotExistException>(
            async () => await messageService.Create(messageDTO), $"Chat between users {user1.Id} and {user2.Id} does not exist.");
    }

    [Test]
    public async Task Get_ShouldReturnMessages()
    {
        var user1 = base.CreateUserInDatabase();
        var user2 = base.CreateUserInDatabase();
        var chat = base.CreateOneToOneChat(user1, user2);

        var message1 = new Message(chat.Id, user1.Id, user2.Id, "Hello");
        var message2 = new Message(chat.Id, user2.Id, user1.Id, "Hi there!");

        dbContext.Add(message1);
        dbContext.Add(message2);
        dbContext.SaveChanges();

        var messageDTOs = await messageService.Get(chat.Id, user1.Id);

        Assert.AreEqual(2, messageDTOs.Count());
        
        var messageDTO1 = messageDTOs.First(x => x.FromId == user1.Id);
        Assert.AreEqual(message1.Text, messageDTO1.Text);
        Assert.That(messageDTO1.CreatedAt, Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromMinutes(1)));
        Assert.AreEqual(chat.Id, messageDTO1.ChatId);
        Assert.AreEqual(messageDTO1.ToId, user2.Id);

        var messageDTO2 = messageDTOs.First(x => x.FromId == user2.Id);
        Assert.AreEqual(message2.Text, messageDTO2.Text);
        Assert.That(messageDTO2.CreatedAt, Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromMinutes(1)));
        Assert.AreEqual(chat.Id, messageDTO2.ChatId);
        Assert.AreEqual(messageDTO2.ToId, user1.Id);
    }

    [Test]
    public async Task Get_ShouldReturnEmpty_WhenChatDoesNotExist()
    {
        var user1 = base.CreateUserInDatabase();
        var user2 = base.CreateUserInDatabase();
        var chat = base.CreateOneToOneChat(user1, user2);

        var messageDTOs = await messageService.Get(chat.Id, user1.Id);

        Assert.AreEqual(0, messageDTOs.Count());
    }

    [Test]
    public async Task Get_ShouldReturnEmpty_WhenMessagesDoNotExist()
    {
        var user1 = base.CreateUserInDatabase();
        var user2 = base.CreateUserInDatabase();

        var messageDTOs = await messageService.Get(1, user1.Id);

        Assert.AreEqual(0, messageDTOs.Count());
    }

    [Test]
    public async Task Get_ShouldReturnEmpty_WhenUserDoesNotBelongToChat()
    {
        var user1 = base.CreateUserInDatabase();
        var user2 = base.CreateUserInDatabase();
        var userWithNoAccess = base.CreateUserInDatabase();
        var chat = base.CreateOneToOneChat(user1, user2);

        var message = new Message(chat.Id, user1.Id, user2.Id, "Hello");

        dbContext.Add(message);
        dbContext.SaveChanges();

        var emptyMessageDTOs = await messageService.Get(chat.Id, userWithNoAccess.Id);
        var messagesDTOs1 = await messageService.Get(chat.Id, user1.Id);
        var messagesDTOs2 = await messageService.Get(chat.Id, user2.Id);

        Assert.IsEmpty(emptyMessageDTOs);
        Assert.IsNotEmpty(messagesDTOs1);
        Assert.IsNotEmpty(messagesDTOs2);

        var actualMessage1 = messagesDTOs1.Single();
        var actualMessage2 = messagesDTOs2.Single();
        
        Assert.AreEqual(actualMessage1.Id, actualMessage2.Id);
    }
}
