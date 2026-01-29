using SideEffect.Demo.Common;
using SideEffect.Messaging.RPC;

namespace SideEffect.Demo.Worker;

public class SendMessageRequestHandler : RequestHandlerBase<SendMessageRequest, SendMessageResponse>
{
    public override Task<SendMessageResponse> HandleAsync(SendMessageRequest message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
