namespace PINChat.Api.Library.Models;

public class GroupModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public byte[]? Avatar { get; set; }
    public string? AvatarPath { get; set; }
    public List<UserModel>? Contacts { get; set; } = new();
}