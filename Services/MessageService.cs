using SpeakBot.Models;

namespace SpeakBot.Services;

public interface IMessageService
{
    event Action<IEnumerable<Message>> OnMessageUpdate;
    event Action OnChatUpdate;
    public event Action OnAddChat;
    public event Action OnDeleteChat;
    void SendMessage(IEnumerable<Message> messages);
    Task ChangeChat();
    Task AddChat();
    Task OnChatDelete();
    void ClearMessages();
}

public class MessageService : IMessageService
{
    public event Action<IEnumerable<Message>> OnMessageUpdate;
    public event Action OnChatUpdate;
    public event Action OnAddChat;
    public event Action OnDeleteChat;

    public void SendMessage(IEnumerable<Message> messages)
    {
        OnMessageUpdate?.Invoke(messages);
    }

    public async Task ChangeChat()
    {
        OnChatUpdate?.Invoke();
    }

    public async Task AddChat()
    {
        OnAddChat?.Invoke();
    }
     public async Task OnChatDelete()
    {
        OnDeleteChat?.Invoke();
    }

    public void ClearMessages()
    {
        OnMessageUpdate?.Invoke(null);
    }
}
