namespace PINChat.Api.Library.Models;

public class GroupDbModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public byte[]? Avatar { get; set; }
    public string? AvatarPath { get; set; }
    public List<string> ContactIds { get; set; } = new();
}