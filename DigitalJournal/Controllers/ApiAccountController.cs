namespace DigitalJournal.Controllers;

[ApiController, Microsoft.AspNetCore.Mvc.Route("/api/account")]
public class ApiAccountController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    public ApiAccountController(SignInManager<User> signInManager, UserManager<User> userManager, IConfiguration configuration)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]Credentials model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
        if (result.Succeeded)
            return Ok();
        return Unauthorized();
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
    [HttpPost("token")]
    public async Task<IActionResult> Token([FromBody]Credentials credentials)
    {
        if (await CheckPassword(credentials))
        {
            var handler = new JwtSecurityTokenHandler();
            var secret = Encoding.ASCII.GetBytes(_configuration["jwtSecret"]);
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, credentials.UserName) }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = handler.CreateToken(descriptor);
            return Ok(new { 
                success = true, 
                token = handler.WriteToken(token) 
            });
        }
        return Unauthorized();
    }
    private async Task<bool> CheckPassword(Credentials credentials)
    {
        var user = await _userManager.FindByNameAsync(credentials.UserName);
        if (user is { })
        {
            foreach (var v in _userManager.PasswordValidators)
            {
                if ((await v.ValidateAsync(_userManager, user, credentials.Password)).Succeeded)
                {
                    return true;
                }
            }
        }
        return false;
    }
}

public class Credentials
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}
