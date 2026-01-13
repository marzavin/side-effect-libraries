using RabbitMQServiceBusPublisher = SideEffect.Messaging.RabbitMQ.ServiceBusPublisher;
using RedisServiceBusPublisher = SideEffect.Messaging.Redis.ServiceBusPublisher;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var rabbitConnection = builder.Configuration.GetConnectionString("rabbitmq");
builder.Services.AddSingleton(new SideEffect.Messaging.RabbitMQ.ServiceBusSettings { ConnectionString = rabbitConnection });
builder.Services.AddScoped<RabbitMQServiceBusPublisher>();

var redisConnection = builder.Configuration.GetConnectionString("redis");
builder.Services.AddSingleton(new SideEffect.Messaging.Redis.ServiceBusSettings { ConnectionString = redisConnection });
builder.Services.AddScoped<RedisServiceBusPublisher>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
    });
}

app.MapControllers();

await app.RunAsync();
