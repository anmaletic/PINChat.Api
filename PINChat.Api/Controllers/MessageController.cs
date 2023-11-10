using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PINChat.Api.Library.DataAccess.Interfaces;
using PINChat.Api.Library.Models;

namespace PINChat.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MessageController : ControllerBase
{
    private readonly IMessageData _messageData;

    public MessageController(IMessageData messageData)
    {
        _messageData = messageData;
    }
    
    [HttpPost]
    [Route("GetByUser")]
    public List<MessageModel> GetByUserId(MessageQueryModel msg)
    {
        var messages = _messageData.GetMessagesByUserId(msg);

        return messages;
    }
    
    [HttpPost]
    [Route("GetByGroup")]
    public List<MessageModel> GetByGroupId(MessageQueryModel msg)
    {
        var messages = _messageData.GetMessagesByGroupId(msg);

        return messages;
    }
    
    [HttpPost]
    [Route("Insert")]
    public void CreateNew(MessageModel msg)
    {
        _messageData.CreateNewMessage(msg);
    }
}