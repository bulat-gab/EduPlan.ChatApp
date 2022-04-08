using EduPlan.ChatApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduPlan.ChatApp.Infrastructure.Config;

public class ChatParticipantConfiguration : IEntityTypeConfiguration<ChatParticipant>
{
    public void Configure(EntityTypeBuilder<ChatParticipant> builder)
    {
        builder.HasKey(p => new { p.ChatId, p.UserId });

        builder
            .HasOne(chatParticipant => chatParticipant.Chat)
            .WithMany(chat => chat.ChatParticipants)
            .HasForeignKey(chatParticipant => chatParticipant.ChatId);

        builder.
            HasOne(chatParticipant => chatParticipant.User)
            .WithMany(user => user.ChatParticipants)
            .HasForeignKey(chatParticipant => chatParticipant.UserId);
    }
}
