using SideEffect.Messaging.RPC;

namespace SideEffect.Demo.Common;

public class SendMessageRequest : IRequest<SendMessageResponse>
{
    public string Message { get; set; }
}
