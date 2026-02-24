using Microsoft.Extensions.DependencyInjection;
using SideEffect.Messaging.PubSub;
using SideEffect.Messaging.RabbitMQ.PubSub;
using SideEffect.Messaging.RabbitMQ.RPC;
using SideEffect.Messaging.RPC;

namespace SideEffect.Messaging;

/// <inheritdoc/>
internal class HandlerRegistry : IHandlerRegistry
{
    public Dictionary<string, Action<IServiceCollection>> ServiceRegistrations { get; } = [];

    /// <inheritdoc/>
    public void AddPublishSubscribeHandler<TEvent, THandler>()
        where TEvent : IEvent
        where THandler : EventHandlerBase<TEvent>
    {
        var key = $"{typeof(TEvent).FullName}/{typeof(THandler).FullName}";
        if (ServiceRegistrations.ContainsKey(key))
        {
            throw new ApplicationException($"Publish/Subscribe handler '{typeof(THandler).FullName}' have already been registered.");
        }

        ServiceRegistrations.Add(key, (serviceCollection) =>
        {
            serviceCollection.AddScoped<THandler>();
            serviceCollection.AddHostedService<EventHandlerService<TEvent, THandler>>();
        });
    }

    /// <inheritdoc/>
    public void AddRemoteProcedureCallHandler<TRequest, TResponse, THandler>()
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse, new()
        where THandler : RequestHandlerBase<TRequest, TResponse>
    {
        var key = $"{typeof(TRequest).FullName}/{typeof(TResponse).FullName}/{typeof(THandler).FullName}";
        if (ServiceRegistrations.ContainsKey(key))
        {
            throw new ApplicationException($"RPC handler '{typeof(THandler).FullName}' have already been registered.");
        }

        ServiceRegistrations.Add(key, (serviceCollection) =>
        {
            serviceCollection.AddScoped<THandler>();
            serviceCollection.AddHostedService<RequestHandlerService<TRequest, TResponse, THandler>>();
        });
    }
}
