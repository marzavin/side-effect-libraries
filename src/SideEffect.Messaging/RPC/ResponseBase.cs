namespace SideEffect.Messaging.RPC;

public abstract class ResponseBase : IResponse
{
    public List<ErrorModel> Errors { get; set; }
}
