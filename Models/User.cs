using System.ComponentModel.DataAnnotations;

namespace SpeakBot.Models;

public class User
{
    public Guid Id { get; set; }

    [Required]
    [RegularExpression(@"^[a-zA-Z0-9!@#$%^&*()_+=,.?-]{2,50}$", ErrorMessage = "Invalid characters.")]
    public string Username { get; set; }

    [Required]
    [RegularExpression(@"^[a-zA-Z0-9!@#$%^&*()_+=,.?-]{2,50}$", ErrorMessage = "Invalid characters.")]
    public string Password { get; set; }
    public string UserImage { get; set; } = "cacti";
    public string Language { get; set; } = "sv-SE";

    public User()
    {
        Id = Guid.NewGuid();
    }
}
