using PINChat.Api.Library.DataAccess.Interfaces;
using PINChat.Api.Library.Models;

namespace PINChat.Api.Library.DataAccess;

public class ImageData : IImageData
{
    private readonly ISqlDataAccess _sql;

    public ImageData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public byte[] GetUserImage(string userId)
    {
        var p = new { Id = userId };
        
        var userDb = _sql.LoadData<UserDBModel, dynamic>("[PINChat].[spUsers_GetById]", p, "PINChatData").FirstOrDefault();

        return userDb!.Avatar!;
    }
}