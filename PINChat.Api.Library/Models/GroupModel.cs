﻿namespace PINChat.Api.Library.Models;

public class GroupModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public List<UserModel>? Contacts { get; set; } = new();
}