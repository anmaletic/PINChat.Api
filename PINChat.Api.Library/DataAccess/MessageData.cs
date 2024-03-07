using PINChat.Api.Library.DataAccess.Interfaces;
using PINChat.Api.Library.Models;
using PINChat.Api.Library.Services;

namespace PINChat.Api.Library.DataAccess;

public class MessageData : IMessageData
{
    private readonly ISqlDataAccess _sql;
    private readonly IEncryptionService _encryptionService;

    public MessageData(ISqlDataAccess sql, IEncryptionService encryptionService)
    {
        _sql = sql;
        _encryptionService = encryptionService;
    }

    public List<MessageModel> GetMessagesByUserId(MessageQueryModel p)
    {
        var output = _sql.LoadData<MessageModel, dynamic>("[PINChat].[spMessages_GetByUserId]", p, "PINChatData");

        foreach (var msg in output)
        {
            msg.Content = _encryptionService.Decrypt(msg.Content!);
        }
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
            Content = _encryptionService.Encrypt(msg.Content!),
            Image = msg.Image
        };
            
        _sql.SaveData("[PINChat].[spMessages_Insert]", p, "PINChatData");
    }
    
}