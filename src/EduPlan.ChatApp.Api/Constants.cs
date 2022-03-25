namespace EduPlan.ChatApp.Api;

public static class Constants
{
    public const string JwtSecret = "ThisSecretMustBeAtLeast16CharactersLong"; // TODO: Move secret out of the code

    public const string JwtIssuer = "https://localhost:5111"; // TODO: change to real domain name (example: api.mywebsite.com)

    public const string JwtAudience = "https://localhost:5111"; // TODO: change to real domain name (example: api.mywebsite.com)
}
