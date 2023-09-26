namespace SpeakBot.Models;

public class Message
{
    public DateTime TimeStamp { get; set; }
    public string? Body { get; set; }
    public bool IsPrompt { get; set; }

    public Message(string body, bool isPrompt)
    {
        TimeStamp = DateTime.Now;
        Body = body;
        IsPrompt = isPrompt;
    }

    public Message()
    {
        TimeStamp = DateTime.Now;
    }
}
