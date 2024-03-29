﻿using PINChat.Api.Library.DataAccess.Interfaces;
using PINChat.Api.Library.Models;

namespace PINChat.Api.Library.DataAccess;

public class GroupData : IGroupData
{
    private readonly ISqlDataAccess _sql;
 
    public GroupData(ISqlDataAccess sql)
    {
        _sql = sql;
    }
    
    public List<GroupModel> GetAllGroups()
    {
        var p = new { };

        _sql.StartTransaction("PINChatData");
        
        var output = _sql.LoadDataInTransaction<GroupModel, dynamic>("[PINChat].[spGroups_GetAll]", p);
        
        foreach (var group in output)
        {
            var gp = new { Id = group.Id };
                
            group.Contacts = _sql.LoadDataInTransaction<UserModel, dynamic>("[PINChat].[spUserGroups_GetContactsById]", gp);
        }

        _sql.CommitTransaction();
        return output;
    }
    
    public void Create(dynamic p)
    {
        _sql.SaveData("[PINChat].[spGroups_Insert]", p, "PINChatData");
    }
    
    public void Update(GroupDbModel group)
    {
        var p = new
        {
            Id = group.Id,
            Name = group.Name,
            Avatar = group.Avatar,
            AvatarPath = group.AvatarPath
        };
        
        _sql.SaveData("[PINChat].[spGroups_Update]", p, "PINChatData");
    }
}