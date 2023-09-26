namespace SpeakBot.Models;

public class ChatHistory
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Heading { get; set; }
    public List<Message>? Prompts { get; set; }
    public DateTime TimeStamp { get; set; }
    public bool IsLocked { get; set; }
    public string? CardHeading { get; set; }
    public string? CardImage { get; set; }
    public string SystemMessage { get; set; }

    public ChatHistory()
    {
        TimeStamp = DateTime.UtcNow;
    }
}