using PINChat.Api.Library.Models;

namespace PINChat.Api.Library.DataAccess.Interfaces;

public interface IMessageData
{
    List<MessageModel> GetMessagesByUserId(MessageQueryModel p);
    List<MessageModel> GetMessagesByGroupId(MessageQueryModel p);
    void CreateNewMessage(MessageModel msg);
}