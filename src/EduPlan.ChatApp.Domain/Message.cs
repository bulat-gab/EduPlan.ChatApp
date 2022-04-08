namespace EduPlan.ChatApp.Domain;

public class Message : Entity<int, int>
{
    public int ChatId { get; set; }

    public int FromId { get; set; }

    public int ToId { get; set; }

    public string Text { get; set; }

    private Message() { }

    public Message(int fromId, int toId, string text)
    {
        this.FromId = fromId;
        this.ToId = toId;
        this.Text = text;
        
        this.CreatedAt = DateTime.UtcNow;
        this.CreatedBy = fromId;
    }
}
