using System.Security.Claims;
using EduPlan.ChatApp.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EduPlan.ChatApp.Api.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v1/chat")]
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
        var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = this.User.FindFirstValue(ClaimTypes.Email);

        logger.Information($"Chat creation request from UserId: {currentUserId}, Email: {email} to {userId}");

        var createdChat = await chatService.Create(int.Parse(currentUserId), userId);
        if (createdChat != null)
            return Ok();

        return BadRequest();
    }
}
