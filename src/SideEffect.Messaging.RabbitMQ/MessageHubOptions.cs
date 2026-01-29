namespace SideEffect.Messaging;

public class MessageHubOptions
{
    public IHandlerRegistry Registry { get; }

    public MessageHubOptions()
    {
        Registry = new HandlerRegistry();
    }
}
