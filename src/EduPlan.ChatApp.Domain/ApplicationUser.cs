using Microsoft.AspNetCore.Identity;

namespace EduPlan.ChatApp.Domain;

public class ApplicationUser : IdentityUser<int>
{
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// EF Core navigation property
    /// </summary>
    public ICollection<ChatParticipant> ChatParticipants { get; set; }
}
