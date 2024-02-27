using PINChat.Api.Library.DataAccess.Interfaces;
using PINChat.Api.Library.Models;

namespace PINChat.Api.Library.DataAccess;

public class MessageData : IMessageData
{
    private readonly ISqlDataAccess _sql;

    public MessageData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public List<MessageModel> GetMessagesByUserId(MessageQueryModel p)
    {
        var output = _sql.LoadData<MessageModel, dynamic>("[PINChat].[spMessages_GetByUserId]", p, "PINChatData");

        return output;
    }
    
    public List<MessageModel> GetMessagesByGroupId(MessageQueryModel msg)
    {
        var p = new { msg.TargetId };
        var output = _sql.LoadData<MessageModel, dynamic>("[PINChat].[spMessages_GetByGroupId]", p, "PINChatData");

        return output;
    }

    public void CreateNewMessage(MessageModel msg)
    {
        var p = new
        {
            SourceId = msg.SourceId,
            TargetId = msg.TargetId,
            Content = msg.Content,
            Image = msg.Image,
            AvatarPath = msg.AvatarPath
        };
            
        _sql.SaveData("[PINChat].[spMessages_Insert]", p, "PINChatData");
    }
    
}