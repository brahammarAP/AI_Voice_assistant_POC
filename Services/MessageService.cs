using SpeakBot.Models;

namespace SpeakBot.Services;

public interface IMessageService
{
    event Action<IEnumerable<Message>> OnMessageUpdate;
    event Action OnChatUpdate;
    event Action OnNewChat;
    event Action OnDeleteChat;
    void MessageUpdate(IEnumerable<Message> messages);
    void UpdateChat();
    void NewChat();
    void DeleteChat();
    void ClearMessages();
}

public class MessageService : IMessageService
{
    public event Action<IEnumerable<Message>> OnMessageUpdate;
    public event Action OnChatUpdate;
    public event Action OnNewChat;
    public event Action OnDeleteChat;

    public void MessageUpdate(IEnumerable<Message> messages)
    {
        OnMessageUpdate?.Invoke(messages);
    }

    public void UpdateChat()
    {
        OnChatUpdate?.Invoke();
    }

    public void NewChat()
    {
        OnNewChat?.Invoke();
    }
     public void DeleteChat()
    {
        OnDeleteChat?.Invoke();
    }

    public void ClearMessages()
    {
        OnMessageUpdate?.Invoke(null);
    }
}
