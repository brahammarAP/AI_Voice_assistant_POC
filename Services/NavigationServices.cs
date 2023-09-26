namespace SpeakBot.Services;

public interface INavigationServices
{
    bool GetChatSettingsStatus();
    void ToggleChatSettings(bool IsChatSettings);
}

public class NavigationServices : INavigationServices
{
    private bool isChatSettings;

    public bool GetChatSettingsStatus()
    {
        return isChatSettings;
    }

    public void ToggleChatSettings(bool IsChatSettings)
    {
        isChatSettings = IsChatSettings;
    }
}
