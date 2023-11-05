using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PINChat.Api.Library.DataAccess.Interfaces;
using PINChat.Api.Library.Models;

namespace PINChat.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserData _userData;

    public UserController(IUserData userData)
    {
        _userData = userData;
    }

    [HttpGet]
    public UserModel GetById()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return _userData.GetUserById(userId!);
    }

    [HttpGet]
    [Route("GetAll")]
    public List<UserModel> GetAll()
    {
        var users = new List<UserModel>();
        var usersDb = _userData.GetAllUsers();

        foreach (var userDb in usersDb)
        {
            var user = new UserModel
            {
                Id = userDb.Id,
                DisplayName = userDb.DisplayName,
                FirstName = userDb.FirstName,
                LastName = userDb.LastName,
                LastLoginDate = userDb.LastLoginDate,
                CreatedDate = userDb.CreatedDate
            };
            users.Add(user);
        }
        
        return users;
    }

    [HttpPost]
    [Route("Insert")]
    public void CreateNew(UserModel user)
    {
        _userData.CreateNewUser(user);
    }
            
    [HttpPost]
    [Route("Update")]
    public void Update(UserModel user)
    {
        _userData.UpdateUser(user);
    }

    [HttpPost]
    [Route("Contact/Insert")]
    public void AddContact(UserContactModel contact)
    {
        _userData.AddContact(contact);
    }

    [HttpPost]
    [Route("Contact/Delete")]
    public void RemoveContact(UserContactModel contact)
    {
        _userData.RemoveContact(contact);
    }
    
    
    [HttpPost]
    [Route("Group/Insert")]
    public void AddGroup(UserGroupModel group)
    {
        _userData.AddGroup(group);
    }

    [HttpPost]
    [Route("Group/Delete")]
    public void RemoveGroup(UserGroupModel group)
    {
        _userData.RemoveGroup(group);
    }
}