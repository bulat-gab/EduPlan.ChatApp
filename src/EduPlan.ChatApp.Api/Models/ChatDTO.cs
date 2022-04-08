using EduPlan.ChatApp.Domain;

namespace EduPlan.ChatApp.Api.Models;

public class ChatDTO
{
    //public ICollection<UserDTO> Users { get; set; }
    public int User1 { get; set; }

    public int User2 { get; set; }

    public List<Message> messages { get; set; }
}
