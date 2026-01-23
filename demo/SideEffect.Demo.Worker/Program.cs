using SideEffect.Demo.Common;
using SideEffect.Demo.Worker;
using SideEffect.Messaging.PubSub;
using SideEffect.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddConfiguration<SideEffect.Messaging.RabbitMQ.MessagingSettings>(builder.Configuration, "RabbitServiceBus");
builder.Services.AddScoped<SideEffect.Messaging.RabbitMQ.PubSub.Consumer<Event<MessageModel>, PubSubHandler>>();

var app = builder.Build();

app.MapDefaultEndpoints();

await app.RunAsync();
