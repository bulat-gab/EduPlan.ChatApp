using EduPlan.ChatApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduPlan.ChatApp.Infrastructure.Config;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder
            .HasKey(p => new { p.ChatId, p.Id })
            .IsClustered(true);

        builder.Property(p => p.Id)
            .IsRequired(true)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.FromId)
            .IsRequired(true);
        
        builder.Property(p => p.ToId)
            .IsRequired(true);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(p => p.FromId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(p => p.ToId)
            .OnDelete(DeleteBehavior.NoAction);

        // If the chat has been deleted, all the messages in that chat will be deleted as well
        builder.HasOne<Chat>()
            .WithMany()
            .HasForeignKey(message => message.ChatId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}
