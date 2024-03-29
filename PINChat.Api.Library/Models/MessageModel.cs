﻿namespace PINChat.Api.Library.Models;

public class MessageModel
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? TargetId { get; set; }
    public string? SourceId { get; set; }
    public string? Content { get; set; }
    public string? Image { get; set; }
    
    public string? AvatarPath { get; set; }
}