﻿using PINChat.Api.Library.Models;

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
        
        var groupsDb = _sql.LoadDataInTransaction<GroupModel, dynamic>("[PINChat].[spUserGroups_GetById]", p);
        
        _sql.CommitTransaction();
        
        
        if (userDb is null) return null!;
        
        var user = new UserModel
        {
            Id = userDb.Id,
            DisplayName = userDb.DisplayName,
            FirstName = userDb.FirstName,
            LastName = userDb.LastName,
            LastLoginDate = userDb.LastLoginDate,
            CreatedDate = userDb.CreatedDate,
            Contacts = contactsDb,
            Groups = groupsDb
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
}