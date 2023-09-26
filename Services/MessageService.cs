using SpeakBot.Models;

namespace SpeakBot.Services;

public interface IMessageService
{
    event Action<IEnumerable<Message>> OnMessageUpdate;
    event Action OnChatUpdate;
    void SendMessage(IEnumerable<Message> messages);
    Task ChangeChat();
    void ClearMessages();
}

public class MessageService : IMessageService
{
    public event Action<IEnumerable<Message>> OnMessageUpdate;
    public event Action OnChatUpdate;

    public void SendMessage(IEnumerable<Message> messages)
    {
        OnMessageUpdate?.Invoke(messages);
    }

    public async Task ChangeChat()
    {
        OnChatUpdate?.Invoke();
    }

    public void ClearMessages()
    {
        OnMessageUpdate?.Invoke(null);
    }
}
