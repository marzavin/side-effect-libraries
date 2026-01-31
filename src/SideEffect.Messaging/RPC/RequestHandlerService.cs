using Microsoft.Extensions.Logging;

namespace SideEffect.Messaging.RPC;

internal class RequestHandlerService<TRequest, TResponse, THandler>
    where TRequest : IRequest<TResponse>
    where TResponse : IResponse, new()
    where THandler : RequestHandlerBase<TRequest, TResponse>
{
    /// <summary>
    /// Gets provider for external dependencies.
    /// </summary>
    protected IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Gets logger.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Gets unique service key.
    /// </summary>
    public string Key { get; } = GetServiceKey();

    public RequestHandlerService(IServiceProvider serviceProvider, ILogger logger)
    {
        ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public static string GetServiceKey()
    {
        return typeof(THandler).FullName;
    }

    /// <inheritdoc/>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
