using PINChat.Api.Library.Models;

namespace PINChat.Api.Library.DataAccess.Interfaces;

public interface IMessageData
{
    List<MessageModel> GetMessagesById(MessageQueryModel p);
    void CreateNewMessage(MessageModel msg);
}