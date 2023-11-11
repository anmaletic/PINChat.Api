using PINChat.Api.Library.Models;

namespace PINChat.Api.Library.DataAccess.Interfaces;

public interface ISettingData
{
    SettingModel GetValueByKey(string key);
}