namespace EduPlan.ChatApp.Domain;

/// <summary>
/// Intermediate table to connect <see cref="ApplicationUser"/> and <see cref="Chat"/> tables.
/// Users and Chats have Many-to-Many relation.
/// </summary>
public class ChatParticipant
{
    public int ChatId { get; set; }
    public Chat Chat { get; set; }

    public int UserId { get; set; }
    public ApplicationUser User { get; set; }
}
