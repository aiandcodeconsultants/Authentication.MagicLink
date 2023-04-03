using Microsoft.Extensions.Options;

namespace Authentication.MagicLink.Services;

/// <summary>The magic-link service.</summary>
public class MagicLinkService : IMagicLinkService
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;
    private readonly MagicLinkOptions _options;

    public MagicLinkService(ITokenGenerator tokenGenerator, IUserService userService, IEmailService emailService, IOptions<MagicLinkOptions> options)
    {
        _tokenGenerator = tokenGenerator;
        _userService = userService;
        _emailService = emailService;
        _options = options.Value;
    }

    public async Task<string> GenerateMagicLinkAsync(string userId)
    {
        var token = _tokenGenerator.GenerateToken(userId);
        var magicLink = $"{_options.MagicLinkBaseUrl}?token={token}";
        
        var user = await _userService.GetUserByIdAsync(userId);
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

        return magicLink;
    }

    public async Task<bool> ValidateMagicLinkAsync(string token)
    {
        //var tokenValidator = new JwtTokenValidator();
        var tokenValidator = new JwtTokenValidator(Options.Create(_options));
        if (tokenValidator.ValidateToken(token, out string userId))
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user != null)
            {
                // Perform additional authentication steps if necessary.
                return true;
            }
        }

        return false;
    }
}