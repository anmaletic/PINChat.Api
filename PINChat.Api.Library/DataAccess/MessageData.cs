﻿using PINChat.Api.Library.DataAccess.Interfaces;
using PINChat.Api.Library.Models;

namespace PINChat.Api.Library.DataAccess;

public class MessageData : IMessageData
{
    private readonly ISqlDataAccess _sql;

    public MessageData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public List<MessageModel> GetMessagesById(MessageQueryModel p)
    {
        var output = _sql.LoadData<MessageModel, dynamic>("[PINChat].[spMessages_GetById]", p, "PINChatData");

        return output;
    }

    public void CreateNewMessage(MessageModel msg)
    {
        var p = new
        {
            SourceId = msg.SourceId,
            TargetId = msg.TargetId,
            Content = msg.Content,
            Image = msg.Image
        };
            
        _sql.SaveData("[PINChat].[spMessages_Insert]", p, "PINChatData");
    }
    
}