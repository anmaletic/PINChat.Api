namespace PINChat.Api.Library.Models;

public class UserDBModel
{
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public byte[]? Avatar { get; set; }
    public string? AvatarPath { get; set; }
    public DateTime LastLoginDate { get; set; }
    public DateTime CreatedDate { get; set; }
}