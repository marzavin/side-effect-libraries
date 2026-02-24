using SideEffect.Messaging.RPC;

namespace SideEffect.Demo.Common;

public class SendMessageResponse : ResponseBase
{
    public string Message { get; set; }
}
