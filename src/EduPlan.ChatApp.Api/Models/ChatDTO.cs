using System.Runtime.Serialization;
using EduPlan.ChatApp.Domain;

namespace EduPlan.ChatApp.Api.Models;

[DataContract(Name = "Chat")]
public class ChatDTO
{
    public int Id { get; set; }

    public UserDTO User1 { get; set; }

    public UserDTO User2 { get; set; }

    public string Name { get; set; }

    public ChatType Type { get; set; }

    public List<Message> Messages { get; set; }
}
