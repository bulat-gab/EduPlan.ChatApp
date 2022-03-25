using Microsoft.AspNetCore.Identity;

namespace EduPlan.ChatApp.Infrastructure;

public class ApplicationUser : IdentityUser<int>
{
    public DateTime CreatedAt { get; set; }
}
