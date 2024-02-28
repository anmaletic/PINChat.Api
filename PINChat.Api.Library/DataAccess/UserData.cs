using PINChat.Api.Library.DataAccess.Interfaces;
using PINChat.Api.Library.Models;

namespace PINChat.Api.Library.DataAccess;

public class UserData : IUserData
{
    private readonly ISqlDataAccess _sql;

    public UserData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public UserModel GetUserById(string id)
    {
        var p = new { Id = id };

        _sql.StartTransaction("PINChatData");
        
        var userDb = _sql.LoadDataInTransaction<UserDBModel, dynamic>("[PINChat].[spUsers_GetById]", p).FirstOrDefault();

        var contactsDb = _sql.LoadDataInTransaction<UserModel, dynamic>("[PINChat].[spUserContacts_GetById]", p);
        
        var groupsDb = _sql.LoadDataInTransaction<GroupDbModel, dynamic>("[PINChat].[spUserGroups_GetById]", p);

        var groups = new List<GroupModel>();

        foreach (var group in groupsDb)
        {
            var gp = new { Id = group.Id };
                
            var groupContacts = _sql.LoadDataInTransaction<UserModel, dynamic>("[PINChat].[spUserGroups_GetContactsById]", gp);
            
            groups.Add(new GroupModel()
            {
                Id = group.Id,
                Name = group.Name,
                Avatar = group.Avatar,
                AvatarPath = group.AvatarPath,
                Contacts = groupContacts
            });
        }
        
        _sql.CommitTransaction();
        
        if (userDb is null) return null!;
        
        var user = new UserModel
        {
            Id = userDb.Id,
            DisplayName = userDb.DisplayName,
            FirstName = userDb.FirstName,
            LastName = userDb.LastName,
            LastLoginDate = userDb.LastLoginDate,
            Avatar = userDb.Avatar,
            AvatarPath = userDb.AvatarPath,
            CreatedDate = userDb.CreatedDate,
            Contacts = contactsDb,
            Groups = groups
        };
            
        return user;
    }
    
    public List<UserDBModel> GetAllUsers()
    {
        var p = new { };

        var output = _sql.LoadData<UserDBModel, dynamic>("[PINChat].[spUsers_GetAll]", p, "PINChatData");

        return output;
    }

    public void CreateNewUser(dynamic p)
    {
        _sql.SaveData("[PINChat].[spUsers_Insert]", p, "PINChatData");
    }

    public void UpdateUser(UserModel user)
    {
        var p = new
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Avatar = user.Avatar,
            AvatarPath = user.AvatarPath
        };
        
        _sql.SaveData("[PINChat].[spUsers_Update]", p, "PINChatData");
    }

    public void AddContact(UserContactModel user)
    {
        _sql.SaveData("[PINChat].[spUserContacts_Insert]", user, "PINChatData");
    }

    public void RemoveContact(UserContactModel user)
    {
        _sql.SaveData("[PINChat].[spUserContacts_Delete]", user, "PINChatData");
    }

    public void AddGroup(UserGroupModel group)
    {
        _sql.SaveData("[PINChat].[spUserGroups_Insert]", group, "PINChatData");
    }

    public void RemoveGroup(UserGroupModel group)
    {
        _sql.SaveData("[PINChat].[spUserGroups_Delete]", group, "PINChatData");
    }
}