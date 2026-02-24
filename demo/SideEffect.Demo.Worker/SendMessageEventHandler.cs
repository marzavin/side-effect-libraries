using SideEffect.Demo.Common;
using SideEffect.Messaging.PubSub;

namespace SideEffect.Demo.Worker;

public class SendMessageEventHandler : EventHandlerBase<SendMessageEvent>
{
    public override Task HandleAsync(SendMessageEvent message, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Message from message hub: {message.Message}");

        return Task.CompletedTask;
    }
}
