using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
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
            var existingUser = await userManager.FindByEmailAsync(email);
            var accessToken = IssueJwt(existingUser);

            await userManager.SetAuthenticationTokenAsync(
                existingUser,
                info.LoginProvider,
                "AccessToken",
                accessToken);

            return Redirect($"{returnUrl}?access_token={accessToken}");
        }

        var newUser = new ApplicationUser
        {
            Email = email,
            UserName = email,
            CreatedAt = DateTime.UtcNow,
        };
        IdentityResult identityResult = await userManager.CreateAsync(newUser);
        if (identityResult.Succeeded)
        {
            logger.Information($"User {newUser.Email} has been created");

            await userManager.AddLoginAsync(newUser, info);
            logger.Information($"User {newUser.Email} login added");

            await signInManager.SignInAsync(newUser, false);
            logger.Information($"User {newUser.Email} signed in");

            //var jwtResult = await jwtAuthManager.GenerateTokens(user, claims, DateTime.UtcNow);
            var accessToken = IssueJwt(newUser);

            //sucess
            await userManager.SetAuthenticationTokenAsync(
                newUser,
                info.LoginProvider,
                "AccessToken",
                accessToken);

            return Redirect($"{returnUrl}?access_token={accessToken}");
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
