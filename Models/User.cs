using Blazor.SubtleCrypto;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SpeakBot.Models;

public class User
{
    public Guid Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
    public string UserImage { get; set; } = "cacti";
    public string Language { get; set; } = "sv-SE";

    public User()
    {
        Id = Guid.NewGuid();
    }
}
