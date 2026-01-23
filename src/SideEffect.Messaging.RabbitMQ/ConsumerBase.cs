using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SideEffect.Messaging.Serialization;

namespace SideEffect.Messaging.RabbitMQ;

/// <summary>
/// Base consumer of messaging hub.
/// </summary>
public abstract class ConsumerBase : MessagingClientBase, IAsyncDisposable
{
    /// <summary>
    /// Gets service provider.
    /// </summary>
    protected IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Gets or sets message consumer.
    /// </summary>
    protected AsyncEventingBasicConsumer Consumer { get; set; }

    /// <summary>
    /// Gets or sets message handler.
    /// </summary>
    protected AsyncEventHandler<BasicDeliverEventArgs> Handler { get; set; }

    /// <summary>
    /// Gets or sets messaging channel.
    /// </summary>
    protected IChannel Channel { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="ConsumerBase"/>.
    /// </summary>
    /// <param name="settings">See <see cref="MessagingSettings"/> for more information.</param>
    /// <param name="serializer">See <see cref="IObjectSerializer"/> for more information.</param>
    /// <param name="logger">See <see cref="ILogger"/> for more information.</param>
    /// <param name="serviceProvider">See <see cref="IServiceProvider"/> for more information.</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected ConsumerBase(MessagingSettings settings, IObjectSerializer serializer, ILogger logger, IServiceProvider serviceProvider)
        : base(settings, serializer, logger)
    {
        ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <summary>
    /// Creates connection to channel.
    /// </summary>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    protected async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        await EstablishConnectionAsync(cancellationToken);

        Channel ??= await Connection.CreateChannelAsync(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Closes connection to channel.
    /// </summary>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    protected async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        if (Channel is not null && Channel.IsOpen)
        {
            await Channel.CloseAsync(cancellationToken: cancellationToken);
        }

        Channel = null;

        await CloseConnectionAsync(cancellationToken);
    }

    /// <summary>
    /// Unsubscribes from messages and closes connection to channel.
    /// </summary>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    protected async Task UnsubscribeAsync(CancellationToken cancellationToken = default)
    {
        if (Consumer is null)
        {
            return;
        }

        Consumer.ReceivedAsync -= Handler;
        Consumer = null;

        await DisconnectAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        await UnsubscribeAsync(CancellationToken.None);
    }
}
