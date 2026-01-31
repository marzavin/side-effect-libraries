using Microsoft.Extensions.DependencyInjection;
using SideEffect.Messaging.PubSub;
using SideEffect.Messaging.RPC;

namespace SideEffect.Messaging;

/// <summary>
/// A registry of message handlers.
/// </summary>
public interface IHandlerRegistry
{
    /// <summary>
    /// Gets the list of service registrations for DI engine.
    /// </summary>
    public Dictionary<string, Action<IServiceCollection>> ServiceRegistrations { get; }

    /// <summary>
    /// Adds RPC handler to the registry.
    /// </summary>
    /// <typeparam name="TRequest">See <see cref="IRequest{TResponse}"/> for more information.</typeparam>
    /// <typeparam name="TResponse">See <see cref="IResponse"/> for more information.</typeparam>
    /// <typeparam name="THandler">See <see cref="RequestHandlerBase{TRequest, TResponse}"/> for more information.</typeparam>
    public void AddRemoteProcedureCallHandler<TRequest, TResponse, THandler>()
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse, new()
        where THandler : RequestHandlerBase<TRequest, TResponse>;

    /// <summary>
    /// Adds Publish/Subscribe handler to the registry.
    /// </summary>
    /// <typeparam name="TEvent">See <see cref="IEvent"/> for more information.</typeparam>
    /// <typeparam name="THandler">See <see cref="EventHandlerBase{TEvent}"/> for more information.</typeparam>
    public void AddPublishSubscribeHandler<TEvent, THandler>()
        where TEvent : IEvent
        where THandler : EventHandlerBase<TEvent>;
}
