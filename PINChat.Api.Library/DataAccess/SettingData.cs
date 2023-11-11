using PINChat.Api.Library.DataAccess.Interfaces;
using PINChat.Api.Library.Models;

namespace PINChat.Api.Library.DataAccess;

public class SettingData : ISettingData
{
    private readonly ISqlDataAccess _sql;
 
    public SettingData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public SettingModel GetValueByKey(string key)
    {
        var p = new { Key = key };

        var output = _sql.LoadData<SettingModel, dynamic>("[PINChat].[spSettings_GetByKey]", p, "PINChatData").FirstOrDefault();

        return output;
    }
}