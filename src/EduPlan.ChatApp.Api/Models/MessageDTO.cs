using System.ComponentModel.DataAnnotations;

namespace EduPlan.ChatApp.Api.Models;

public class MessageDTO
{
    public int ChatId { get; set; }

    public int FromId { get; set; }

    [Required]
    public int ToId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Text { get; set; }

    public DateTime CreatedAt { get; set; }
}
