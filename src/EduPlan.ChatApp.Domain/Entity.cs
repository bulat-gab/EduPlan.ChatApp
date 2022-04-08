namespace EduPlan.ChatApp.Domain;

public abstract class Entity<TId, TCreatedById>
{
    public TId Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public TCreatedById? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public string? LastModifiedBy { get; set; }
}
