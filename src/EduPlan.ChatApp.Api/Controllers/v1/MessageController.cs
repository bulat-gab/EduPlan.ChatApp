using System.Security.Claims;
using EduPlan.ChatApp.Api.Exceptions;
using EduPlan.ChatApp.Api.Models;
using EduPlan.ChatApp.Api.Services;
using EduPlan.ChatApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EduPlan.ChatApp.Api.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v1/chat/{chatId:int}/message")]
public class MessageController : ControllerBase
{
    private readonly Serilog.ILogger logger = Log.ForContext<MessageController>();

    private readonly IMessageService messageService;

    public MessageController(IMessageService messageService)
    {
        this.messageService = messageService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<ChatDTO>> Get(int fromUserId)
    {
        var chats = await messageService.GetChats(fromUserId);

        return chats;
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> SendMessage([FromRoute] int chatId, [FromBody] MessageDTO messageDTO)
    {
        var currentUserId = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

        logger.Information($"Message creation request. " +
            $"ChatId: {chatId}, FromUserId: {currentUserId} ToUserId: {messageDTO.ToId}");

        messageDTO.ChatId = chatId;
        messageDTO.FromId = currentUserId;

        try
        {
            await messageService.Create(messageDTO);
        }
        catch (ChatAppInvalidInputException exception)
        {
            return BadRequest(exception.Message); // TODO: Create an error class with code and decsription
        }

        return Ok();
    }
}
