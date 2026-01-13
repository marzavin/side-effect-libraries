using Microsoft.AspNetCore.Mvc;
using SideEffect.Demo.Common;
using RabbitMQServiceBusPublisher = SideEffect.Messaging.RabbitMQ.ServiceBusPublisher;
using RedisServiceBusPublisher = SideEffect.Messaging.Redis.ServiceBusPublisher;

namespace SideEffect.Demo.Web.Controllers;

[ApiController]
[Route("servicebus")]
public class ServiceBusController : ControllerBase
{
    private RedisServiceBusPublisher RedisServiceBusPublisher { get; }

    private RabbitMQServiceBusPublisher RabbitMQServiceBusPublisher { get; }

    public ServiceBusController(
        RedisServiceBusPublisher redisServiceBusPublisher,
        RabbitMQServiceBusPublisher rabbitMQServiceBusPublisher)
    {
        RedisServiceBusPublisher = redisServiceBusPublisher ?? throw new ArgumentNullException(nameof(redisServiceBusPublisher));
        RabbitMQServiceBusPublisher = rabbitMQServiceBusPublisher ?? throw new ArgumentNullException(nameof(rabbitMQServiceBusPublisher));
    }

    [HttpGet("pub-sub/redis")]
    public async Task<IActionResult> PubSubThroughRedisAsync(CancellationToken cancellationToken = default)
    {
        await RedisServiceBusPublisher.PublishAsync(new PubSubMessage(new() { Text = "Pub/Sub through Redis" }), cancellationToken);
        return Ok();
    }

    [HttpGet("pub-sub/rabbit-mq")]
    public async Task<IActionResult> PubSubThroughRabbitMQAsync(CancellationToken cancellationToken = default)
    {
        await RabbitMQServiceBusPublisher.PublishAsync(new PubSubMessage(new() { Text = "Pub/Sub through RabbitMQ" }), cancellationToken);
        return Ok();
    }
}
