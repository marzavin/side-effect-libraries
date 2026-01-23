using Microsoft.AspNetCore.Mvc;
using SideEffect.Demo.Common;
using SideEffect.Messaging.PubSub;
using RabbitMQProducer= SideEffect.Messaging.RabbitMQ.PubSub.Producer;

namespace SideEffect.Demo.Web.Controllers;

[ApiController]
[Route("servicebus")]
public class ServiceBusController : ControllerBase
{
    private RabbitMQProducer RabbitMQProducer { get; }

    public ServiceBusController(RabbitMQProducer rabbitMQProducer)
    {
        RabbitMQProducer = rabbitMQProducer ?? throw new ArgumentNullException(nameof(rabbitMQProducer));
    }

    [HttpGet("pub-sub/rabbit-mq")]
    public async Task<IActionResult> PubSubThroughRabbitMQAsync(CancellationToken cancellationToken = default)
    {
        await RabbitMQProducer.PublishEventAsync(new Event<MessageModel>(new() { Text = "Pub/Sub through RabbitMQ" }), cancellationToken);
        return Ok();
    }
}
