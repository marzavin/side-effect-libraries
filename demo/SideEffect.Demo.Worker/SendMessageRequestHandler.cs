using SideEffect.Demo.Common;
using SideEffect.Messaging.RPC;

namespace SideEffect.Demo.Worker;

public class SendMessageRequestHandler : RequestHandlerBase<SendMessageRequest, SendMessageResponse>
{
    public override Task<SendMessageResponse> HandleAsync(SendMessageRequest message, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Message from message hub: {message.Message}");

        return Task.FromResult(new SendMessageResponse { Message = $"Response to message '{message.Message}'." });
    }
}
