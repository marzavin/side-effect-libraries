using Microsoft.AspNetCore.Mvc;
using SideEffect.Demo.Common;
using SideEffect.Messaging;

namespace SideEffect.Demo.Web.Controllers;

[ApiController]
[Route("servicebus/rabbim-mq")]
public class ServiceBusController : ControllerBase
{
    private IMessageHubClient RabbitClient { get; }

    private ILogger Logger { get; }

    public ServiceBusController(IMessageHubClient rabbitClient, ILogger<ServiceBusController> logger)
    {
        RabbitClient = rabbitClient ?? throw new ArgumentNullException(nameof(rabbitClient));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("pub-sub")]
    public async Task<IActionResult> PubSubThroughRabbitMQAsync(CancellationToken cancellationToken = default)
    {
        await RabbitClient.PublishEventAsync(new SendMessageEvent { Message = "Pub/Sub through RabbitMQ" }, cancellationToken);
        return Ok();
    }

    [HttpGet("rpc")]
    public async Task<IActionResult> RPCThroughRabbitMQAsync(CancellationToken cancellationToken = default)
    {
        var request = new SendMessageRequest { Message = "RPC through RabbitMQ" };
        var response = await RabbitClient.ExecuteRequestAsync<SendMessageRequest, SendMessageResponse>(request, cancellationToken);

        Logger.LogInformation("Response {responseType} from RPC service via RabbitMQ: {message}.", response.GetType().FullName, response.Message);

        return Ok();
    }
}
