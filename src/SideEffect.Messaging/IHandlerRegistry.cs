using Microsoft.Extensions.DependencyInjection;
using SideEffect.Messaging.PubSub;
using SideEffect.Messaging.RPC;

namespace SideEffect.Messaging;

public interface IHandlerRegistry
{
    public Dictionary<string, Action<IServiceCollection>> ServiceRegistrations { get; }

    public void AddRemoteProcedureCallHandler<TRequest, TResponse, THandler>()
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse, new()
        where THandler : RequestHandlerBase<TRequest, TResponse>;

    public void AddPublishSubscribeHandler<TEvent, THandler>()
        where TEvent : IEvent
        where THandler : EventHandlerBase<TEvent>;
}
