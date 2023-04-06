using Authentication.MagicLink.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Authentication.MagicLink.Services;

/// <summary>The magic-link service.</summary>
public class MagicLinkService : IMagicLinkService
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;
    private readonly MagicLinkSettings _options;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MagicLinkService(ITokenGenerator tokenGenerator, IUserService userService, IEmailService emailService, IOptions<MagicLinkSettings> options, IHttpContextAccessor httpContextAccessor)
    {
        _tokenGenerator = tokenGenerator;
        _userService = userService;
        _emailService = emailService;
        _options = options.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string?> GenerateMagicLinkAsync(string userId)
    {
        var token = _tokenGenerator.GenerateToken(userId);
        var baseUrl = _options.MagicLinkBaseUrl;
        var request = _httpContextAccessor.HttpContext?.Request;
        if (baseUrl.StartsWith("/") && request != null)
            baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}{baseUrl}";
        var magicLink = $"{baseUrl}?token={token}";
        
        var user = await _userService.GetUserByIdAsync(userId);
        
        if(_options.AutoCreateUsers)
            user = await _userService.CreateUserAsync(userId);

        if (user != null)
        {
            var emailMessage = new EmailMessage
            {
                To = user.Email,
                Subject = "Magic Link",
                Body = $"Click the following link to sign in: {magicLink}"
            };
            await _emailService.SendEmailAsync(emailMessage);
        }
        else magicLink = null;

        return magicLink;
    }

    public async Task<bool> ValidateMagicLinkAsync(string token)
    {
        //var tokenValidator = new JwtTokenValidator();
        var tokenValidator = new JwtTokenValidator(Options.Create(_options));
        //if (tokenValidator.ValidateToken(token, out string userId))
        //var isValid = tokenValidator.ValidateToken(token, out string userId);
        var isValid = tokenValidator.ValidateToken(token, out string email);
        if (isValid)
        {
            //var user = await _userService.GetUserByIdAsync(userId);
            var user = await _userService.GetUserByEmailAsync(email);
            if (user != null)
            {
                _httpContextAccessor.HttpContext!.User = user.ToClaimsPrincipal();
                // Perform additional authentication steps if necessary.
                return true;
            }
        }

        return false;
    }

    public async Task<ClaimsPrincipal?> GetClaimsPrincipal(string token)
    {
        var tokenValidator = new JwtTokenValidator(Options.Create(_options));
        //var isValid = tokenValidator.ValidateToken(token, out string userId);
        var isValid = tokenValidator.ValidateToken(token, out string email);
        if (isValid)
        {
            //var user = await _userService.GetUserByIdAsync(userId);
            var user = await _userService.GetUserByEmailAsync(email);
            if (user != null)
            {
                return user.ToClaimsPrincipal();
            }
        }
        return null;
    }
}