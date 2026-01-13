using SideEffect.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddConfiguration<SideEffect.Messaging.Redis.ServiceBusSettings>(builder.Configuration, "RedisServiceBus");
builder.Services.AddScoped<SideEffect.Messaging.Redis.ServiceBusSubscriber>();

builder.Services.AddConfiguration<SideEffect.Messaging.RabbitMQ.ServiceBusSettings>(builder.Configuration, "RabbitServiceBus");
builder.Services.AddScoped<SideEffect.Messaging.RabbitMQ.ServiceBusSubscriber>();

var app = builder.Build();

app.MapDefaultEndpoints();

await app.RunAsync();
