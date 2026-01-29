using SideEffect.Demo.Common;
using SideEffect.Demo.Worker;
using SideEffect.Messaging.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var rabbitConnection = builder.Configuration.GetConnectionString("rabbitmq");
var settings = new MessageHubSettings { ConnectionString = rabbitConnection };

builder.Services.AddRabbitMQMessageHub(settings, (options) => 
{
    options.Registry.AddPublishSubscribeHandler<SendMessageEvent, SendMessageEventHandler>();
    options.Registry.AddRemoteProcedureCallHandler<SendMessageRequest, SendMessageResponse, SendMessageRequestHandler>();
});
    
var app = builder.Build();

app.MapDefaultEndpoints();

await app.RunAsync();
