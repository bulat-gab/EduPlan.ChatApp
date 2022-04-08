using EduPlan.ChatApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduPlan.ChatApp.Infrastructure.Config;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.HasKey(x => x.Id)
            .IsClustered(true);

        builder.Property(x => x.ChatType)
            .HasConversion<int>();
    }
}
