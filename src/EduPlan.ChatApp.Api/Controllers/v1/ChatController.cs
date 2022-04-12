using System.Security.Claims;
using EduPlan.ChatApp.Api.Models;
using EduPlan.ChatApp.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EduPlan.ChatApp.Api.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v1/chat")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class ChatController : ControllerBase
{   
    private readonly Serilog.ILogger logger = Log.ForContext<ChatController>();
    private readonly IChatService chatService;

    public ChatController(IChatService chatService)
    {
        this.chatService = chatService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create([FromQuery] int userId)
    {
        var currentUserId = GetCurrentUserId();
        var email = GetEmail();

        logger.Information($"Chat creation request from UserId: {currentUserId}, Email: {email} to {userId}");

        var createdChat = await chatService.Create(currentUserId, userId);
        if (createdChat != null)
            return Ok();

        return BadRequest();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IEnumerable<ChatDTO>> Get()
    {
        var userId = GetCurrentUserId();
        var email = GetEmail();

        logger.Information($"Chat retrieval request from UserId: {userId}, Email: {email}");

        var chats = await chatService.GetAll(userId);

        logger.Information($"Chat retrieval succeded. Chats count: {chats.Count()}");

        return chats;
    }

    private int GetCurrentUserId()
    {
        string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.Parse(userId);
    }

    private string GetEmail() => this.User.FindFirstValue(ClaimTypes.Email);
}
