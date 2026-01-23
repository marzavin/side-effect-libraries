using RabbitMQPubSubProducer = SideEffect.Messaging.RabbitMQ.PubSub.Producer;
using RabbitMQRPCProducer = SideEffect.Messaging.RabbitMQ.RPC.Producer;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var rabbitConnection = builder.Configuration.GetConnectionString("rabbitmq");

builder.Services.AddSingleton(new SideEffect.Messaging.RabbitMQ.MessagingSettings { ConnectionString = rabbitConnection });
builder.Services.AddScoped<RabbitMQPubSubProducer>();
builder.Services.AddScoped<RabbitMQRPCProducer>();

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
