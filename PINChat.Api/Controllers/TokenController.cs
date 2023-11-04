using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PINChat.Api.Data;

namespace PINChat.Api.Controllers;

public class TokenController : Controller
{
    private readonly IConfiguration _config;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public TokenController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IConfiguration config)
    {
        _context = context;
        _userManager = userManager;
        _config = config;
    }

    [Route("/token")]
    [HttpPost]
    public async Task<IActionResult> Create(string username, string password, string grant_type)
    {
        if (await IsValidUsernameAndPassword(username, password))
            return new ObjectResult(await GenerateToken(username));
        return BadRequest();
    }

    private async Task<bool> IsValidUsernameAndPassword(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        return await _userManager.CheckPasswordAsync(user, password);
    }

    private async Task<dynamic> GenerateToken(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        var roles = from ur in _context.UserRoles
            join r in _context.Roles on ur.RoleId equals r.Id
            where ur.UserId == user.Id
            select new { ur.UserId, ur.RoleId, r.Name };

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, username),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
            new(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString())
        };

        foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role.Name));

        var key = _config.GetValue<string>("Secrets:SecurityKey");

        var token = new JwtSecurityToken(
            new JwtHeader(
                new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256)),
            new JwtPayload(claims));

        var output = new
        {
            Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
            UserName = username
        };

        return output;
    }
}