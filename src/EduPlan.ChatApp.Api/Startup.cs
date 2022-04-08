using EduPlan.ChatApp.Api.Services;
using EduPlan.ChatApp.Domain;
using EduPlan.ChatApp.Infrastructure;
using EduPlan.ChatApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EduPlan.ChatApp.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add services to the container.
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Add serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .ReadFrom.Configuration(Configuration)
            .CreateLogger();

        services.AddLogging(builder =>
        {
            builder.AddSerilog(Log.Logger);
        });

        var connectionString = Configuration.GetConnectionString("SqlServer");
        services.AddDbContext<ChatAppDbContext>(options =>
            {
                options.UseSqlServer(connectionString, x => x.MigrationsAssembly("EduPlan.ChatApp.Infrastructure"));
            });

        services.AddDatabaseDeveloperPageExceptionFilter();

        services
            .AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ChatAppDbContext>()
            .AddSignInManager();

        services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(x =>
            {
                x.LoginPath = "api/v1/account/external-login";
            })
            .AddJwtBearerCustom(Constants.JwtSecret)
            .AddGoogle(options =>
            {
                options.ClientId = Configuration["Authentication__Google__ClientId"];
                options.ClientSecret = Configuration["Authentication__Google__ClientSecret"];
                options.SignInScheme = IdentityConstants.ExternalScheme;
            });
        services.AddScoped<IChatService, ChatService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSerilogRequestLogging();

        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // migrate any database changes on startup (includes initial db creation)
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var dataContext = scope.ServiceProvider.GetRequiredService<ChatAppDbContext>();
            dataContext.Database.Migrate();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        //app.MapControllers();

        //app.Run();
    }
}
