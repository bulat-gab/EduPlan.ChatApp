
using EduPlan.ChatApp.Domain;
using EduPlan.ChatApp.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EduPlan.ChatApp.Api;

public class Program
{
    public async static Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Starting Web Host");

            var host = CreateHostBuilder(args).Build();
            //MigrateDatabase(host);

            await host.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

    private static void MigrateDatabase(IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<ChatAppDbContext>();

                if (context.Database.IsSqlServer())
                {
                    context.Database.Migrate();
                }

                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                //await ChatAppDbContext.SeedDefaultUserAsync(userManager, roleManager);
                //await ChatAppDbContext.SeedSampleDataAsync(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while migrating or seeding the database.");
                throw;
            }
        }
    }
}
