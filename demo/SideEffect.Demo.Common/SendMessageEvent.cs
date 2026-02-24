using SideEffect.Messaging.PubSub;

namespace SideEffect.Demo.Common;

public class SendMessageEvent : IEvent
{
    public string Message { get; set; }
}
