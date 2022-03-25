using EduPlan.ChatApp.Api;
using EduPlan.ChatApp.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Initialize logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    // Add services to the container.
    services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    // Add serilog
    builder.Host.UseSerilog((context, loggerConfiguration) => loggerConfiguration
        .WriteTo.Console()
        .ReadFrom.Configuration(context.Configuration));

    services.AddDbContext<ChatAppDbContext>(options => options.UseInMemoryDatabase("TestDatabase"));

    if (builder.Environment.IsDevelopment())
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
            options.ClientId = configuration["Authentication__Google__ClientId"];
            options.ClientSecret = configuration["Authentication__Google__ClientSecret"];
            options.SignInScheme = Microsoft.AspNetCore.Identity.IdentityConstants.ExternalScheme;
        });

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

