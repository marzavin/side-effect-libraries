using Microsoft.Extensions.Hosting;

namespace SideEffect.Messaging;

internal interface IMessageHandlerService : IHostedService
{
    public string Key { get; }
}
