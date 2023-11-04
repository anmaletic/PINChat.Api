using PINChat.Api.Library.Models;

namespace PINChat.Api.Library.DataAccess;

public interface IUserData
{
    UserModel GetUserById(string id);
    List<UserDBModel> GetAllUsers();
    void CreateNewUser(dynamic p);
}