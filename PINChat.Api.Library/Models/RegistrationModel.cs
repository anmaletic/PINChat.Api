using System.ComponentModel.DataAnnotations;

namespace PINChat.Api.Library.Models;

public class RegistrationModel
{
    [Required]
    public string? UserName { get; set; }

    [Required]
    [MinLength(6)]
    public string? Password { get; set; }
}