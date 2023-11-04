namespace PINChat.Api.Library.Models;

public class UserModel
{
    
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<UserModel>? Contacts { get; set; } = new();
    public List<GroupModel>? Groups { get; set; } = new();
    public DateTime LastLoginDate { get; set; }
    public DateTime CreatedDate { get; set; }
}