using PINChat.Api.Library.Models;

namespace PINChat.Api.Library.DataAccess.Interfaces;

public interface IUserData
{
    UserModel GetUserById(string id);
    List<UserDBModel> GetAllUsers();
    void CreateNewUser(dynamic p);
    void UpdateUser(UserModel user);
    void AddContact(UserContactModel user);
    void RemoveContact(UserContactModel user);
    void AddGroup(UserGroupModel group);
    void RemoveGroup(UserGroupModel group);
}