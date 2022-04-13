using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace EduPlan.ChatApp.Api.Models;

[DataContract(Name = "Message")]
public class MessageDTO
{
    public int Id { get; set; }

    public int ChatId { get; set; }

    public int FromId { get; set; }

    [Required]
    public int ToId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Text { get; set; }

    public DateTime CreatedAt { get; set; }
}
