using System.Reflection;
using EduPlan.ChatApp.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduPlan.ChatApp.Infrastructure;

public class ChatAppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public ChatAppDbContext()
    {
    }

    public ChatAppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
