using Microsoft.Extensions.DependencyInjection;
using SideEffect.Messaging.Serialization;

namespace SideEffect.Messaging.RabbitMQ;

/// <summary>
/// Provides extension methods for RabbitMQ implementation of message hub.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="settings"></param>
    /// <param name="setupAction"></param>
    /// <returns></returns>
    public static IServiceCollection AddRabbitMQMessageHub(
        this IServiceCollection serviceCollection,
        MessageHubSettings settings,
        Action<MessageHubOptions> setupAction = null)
    {
        serviceCollection.AddSingleton(settings);
        serviceCollection.AddSingleton<IObjectSerializer, JsonObjectSerializer>();
        serviceCollection.AddScoped<IMessageHubClient, MessageHubClient>();

        if (setupAction is not null)
        {
            var options = new MessageHubOptions();
            setupAction(options);

            foreach(var registration in options.Registry.ServiceRegistrations)
            {
                registration.Value.Invoke(serviceCollection);
            }
        }

        return serviceCollection;
    }
}
