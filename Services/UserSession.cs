using ChatBotAI.Models;
using Microsoft.Extensions.Configuration;

namespace ChatBotAI.Services;

public interface IUserSession
{
    ChatHistory CurrentChat { get; set; }
    string? Key { get; set; }
    PromptCard PromptCard { get; set; }
    string? Reigion { get; set; }
    User User { get; set; }

    bool IsLoggedIn();
    string ReFraser(string text, int wordCount);
}

public class UserSession : IUserSession
{
    public User User { get; set; } = new User();
    public ChatHistory CurrentChat { get; set; } = new ChatHistory();
    public PromptCard PromptCard { get; set; } = new PromptCard();
    public string? Key { get; set; }
    public string? Reigion { get; set; }

    public UserSession(IConfiguration configuration)
    {
        Key = configuration.GetSection("Azure:SpeechServiceKey").Value;
        Reigion = configuration.GetSection("Azure:SpeechServiceLocation").Value;
    }

    public bool IsLoggedIn()
    {
        return string.IsNullOrEmpty(User.Username) ? false : true;
    }

    public string ReFraser(string text, int wordCount)
    {
        var words = text.Split(' ');

        if (text.Length <= 30 && words.Length < wordCount)
        {
            return text;
        }

        string newString = string.Join(" ", words.Take(wordCount));

        if (newString.Length > 30)
        {
            newString = newString.Substring(0, 30);
        }

        return $"{newString}...";
    }
}

