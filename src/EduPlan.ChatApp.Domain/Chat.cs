namespace EduPlan.ChatApp.Domain;

/// <summary>
/// A conversation between 2 users.
/// </summary>
public class Chat : Entity<int, int>
{
    public string Name { get; set; }

    public ChatType ChatType { get; set; }

    /// <summary>
    /// EF Core navigation property
    /// </summary>
    public IList<ChatParticipant> ChatParticipants { get; set; }
}
