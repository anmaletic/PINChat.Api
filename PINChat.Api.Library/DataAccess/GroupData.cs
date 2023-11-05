using PINChat.Api.Library.DataAccess.Interfaces;
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

        var output = _sql.LoadData<GroupModel, dynamic>("[PINChat].[spGroups_GetAll]", p, "PINChatData");

        return output;
    }
}