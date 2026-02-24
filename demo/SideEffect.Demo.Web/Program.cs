using SideEffect.Messaging.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var rabbitConnection = builder.Configuration.GetConnectionString("rabbitmq");
var settings = new MessageHubSettings { ConnectionString = rabbitConnection };

builder.Services.AddRabbitMQMessageHub(settings);

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
