using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EduPlan.ChatApp.Domain;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace EduPlan.ChatApp.Api.Controllers.v1;

[Route("api/v1/account")]
[ApiController]
[AllowAnonymous]
public class AccountController : ControllerBase
{
    private readonly Serilog.ILogger logger = Log.ForContext<AccountController>();
    private SignInManager<ApplicationUser> signInManager;
    private UserManager<ApplicationUser> userManager;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
    }

    [Route("external-login")]
    [HttpGet]
    /// <summary>
    /// External authentication (Google, Facebook and etc.). Only Google works at the moment.
    /// </summary>
    /// <param name="provider">'Google' - for google auth provider</param>
    public IActionResult ExternalLogin(string provider, string returnUrl)
    {
        var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "account", new { returnUrl });
        var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        properties.AllowRefresh = true;

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("external-auth-callback")]
    public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
    {
        if (remoteError != null)
        {
            logger.Error($"Error from external provider: {remoteError}");
            //ErrorMessage = $"Error from external provider: {remoteError}";
            return RedirectToAction(nameof(ExternalLogin));
        }

        // Sign in the user with this external login provider if the user already has a login.
        ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            //logger.Information()
            return RedirectToAction(nameof(ExternalLogin));
        }

        var email = info.Principal.FindFirst(ClaimTypes.Email).Value;

        var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
        if (signInResult.Succeeded)
        {
            logger.Information("User {Email} logged in with {Provider} provider.", email, info.LoginProvider);
            return Redirect(returnUrl);
        }

        var user = new ApplicationUser
        {
            Email = email,
            UserName = email,
            CreatedAt = DateTime.UtcNow,
        };
        IdentityResult identityResult = await userManager.CreateAsync(user);
        if (identityResult.Succeeded)
        {
            logger.Information($"User {user.Email} has been created");

            await userManager.AddLoginAsync(user, info);
            logger.Information($"User {user.Email} login added");

            await signInManager.SignInAsync(user, false);
            logger.Information($"User {user.Email} signed in");

            //var jwtResult = await jwtAuthManager.GenerateTokens(user, claims, DateTime.UtcNow);
            var accessToken = IssueJwt(user);

            //sucess
            await userManager.SetAuthenticationTokenAsync(
                user,
                info.LoginProvider,
                "AccessToken",
                accessToken);

            var response = new
            {
                accessToken,
                tokenType = "bearer",
                email,
            };
            return Ok(response);
        }

        return this.Forbid();
    }

    private string IssueJwt(ApplicationUser user)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.JwtSecret));
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
            }),
            Expires = DateTime.UtcNow.AddDays(30),
            Issuer = Constants.JwtIssuer,
            Audience = Constants.JwtAudience,
            SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature),
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}
