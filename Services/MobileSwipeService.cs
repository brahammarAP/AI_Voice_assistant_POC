using Microsoft.AspNetCore.Components.Web;
using SpeakBot.Repository.IRepository;

namespace SpeakBot.Services;

public interface IMobileSwipeService
{
    Task HandleTouchEnd(Guid chatId, TouchEventArgs t);
    void HandleTouchStart(TouchEventArgs t);
}

public class MobileSwipeService : IMobileSwipeService
{
    private readonly IChatHistoryRepository chatHistory;
    private readonly IMessageService messageService;
    private readonly IUserSession session;

    public MobileSwipeService(IChatHistoryRepository chatHistory, IUserSession session, IMessageService messageService)
    {
        this.chatHistory = chatHistory;
        this.session = session;
        this.messageService = messageService;
    }

    (TouchPoint ReferencePoint, DateTime StartTime) startPoint;

    public void HandleTouchStart(TouchEventArgs t)
    {
        startPoint.ReferencePoint = t.TargetTouches[0];
        startPoint.StartTime = DateTime.Now;
    }

    public async Task HandleTouchEnd(Guid chatId, TouchEventArgs t)
    {
        var endReference = t.ChangedTouches[0];

        var diffX = startPoint.ReferencePoint.ClientX - endReference.ClientX;
        var diffY = startPoint.ReferencePoint.ClientY - endReference.ClientY;
        var diffTime = DateTime.Now - startPoint.StartTime;
        var velocityX = Math.Abs(diffX / diffTime.Milliseconds);
        var velocityY = Math.Abs(diffY / diffTime.Milliseconds);
        var run = Math.Abs(diffX);
        var rise = Math.Abs(diffY);
        var ang = Math.Atan2(run, rise) * (180 / Math.PI);

        var swipeThreshold = 0.8;

        if (Math.Abs(velocityX - velocityY) < .5 || (velocityX < swipeThreshold && velocityY < swipeThreshold)) return;

        if (ang > 85 && velocityX >= swipeThreshold)
        {
            if (diffX > 0)
                await SwipeLeft(chatId);
            if (diffX > 0)
                await SwipeRight();
        }
    }

    async Task SwipeLeft(Guid chatId)
    {
        await chatHistory.DeleteAsync(x => x.Id == chatId);

        messageService.DeleteChat();
    }

    async Task SwipeRight()
    {
        return;
    }
}
