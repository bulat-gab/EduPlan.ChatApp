namespace EduPlan.ChatApp.Domain;

/// <summary>
/// A conversation between 2 users.
/// </summary>
public class Chat : Entity<int, int>
{
    public string Name { get; set; }

    public ChatType ChatType { get; set; }

    private Chat()
    {
    }

    public Chat(string name, ChatType chatType)
    {
        this.Name = name;
        this.ChatType = chatType;
    }

    /// <summary>
    /// EF Core navigation property
    /// </summary>
    public ICollection<ChatParticipant> ChatParticipants { get; set; }
}
