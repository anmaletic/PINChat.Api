using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PINChat.Api.Library.DataAccess.Interfaces;
using PINChat.Api.Library.Models;

namespace PINChat.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegistrationController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserData _userData;

    public RegistrationController(UserManager<IdentityUser> userManager, IUserData userData)
    {
        _userManager = userManager;
        _userData = userData;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegistrationModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var userExists = _userManager.Users.FirstOrDefault(x => x.UserName == model.UserName);
            if (userExists is not null)
                return BadRequest("Korisnik sa upisanim imenom već postoji.");

            var newUser = new IdentityUser{ UserName = model.UserName};
            var result = await _userManager.CreateAsync(newUser, model.Password!);
            
            if (result.Succeeded)
            {
                _userData.CreateNewUser(new { newUser.Id });
                return Ok("Korisnik registriran.");
            }
            
            return BadRequest(result.Errors);
        }
        catch (Exception e)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.UserName == model.UserName);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            
            // return BadRequest("Došlo je do pogreške kod registracije. Pokušajte ponovo.");
            Console.WriteLine(e);
            throw;
        }
    }
}