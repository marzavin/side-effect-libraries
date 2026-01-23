using SideEffect.Demo.Common;
using SideEffect.Messaging.PubSub;

namespace SideEffect.Demo.Worker;

public class PubSubHandler : EventHandlerBase<Event<MessageModel>>
{
    public override Task HandleAsync(Event<MessageModel> message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
