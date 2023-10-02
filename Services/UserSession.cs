using SpeakBot.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace SpeakBot.Services;

public interface IUserSession
{
    ChatHistory CurrentChat { get; set; }
    string? Key { get; set; }
    PromptCard PromptCard { get; set; }
    string? Reigion { get; set; }
    User User { get; set; }

    Task<bool> IsLoggedIn();
}

public class UserSession : IUserSession
{
    private readonly ICookieAPIService cookieStore;
    public User User { get; set; } = new User();
    public ChatHistory CurrentChat { get; set; } = new ChatHistory();
    public PromptCard PromptCard { get; set; } = new PromptCard();
    public string? Key { get; set; }
    public string? Reigion { get; set; }

    public UserSession(IConfiguration configuration, ICookieAPIService cookieStore)
    {
        Key = configuration.GetSection("Azure:SpeechServiceKey").Value;
        Reigion = configuration.GetSection("Azure:SpeechServiceLocation").Value;
        this.cookieStore = cookieStore;
    }

    public async Task<bool> IsLoggedIn()
    {
        try
        {
            var cookieUsername = await cookieStore.GetAsync("Username") ?? null!;

            if (cookieUsername == null) return false;

            var cookieUserId = await cookieStore.GetAsync("UserId");

            this.User.Username = cookieUsername.Value;
            this.User.Id = Guid.Parse(cookieUserId.Value);

            return string.IsNullOrEmpty(this.User.Username) ? false : true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An Error has occured: {ex}");
            return false;
        }
    }
}

