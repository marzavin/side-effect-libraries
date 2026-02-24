var builder = DistributedApplication.CreateBuilder(args);

var rabbitmq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin();

var redis = builder.AddRedis("redis")
    .WithRedisInsight()
    .WithRedisCommander();

builder.AddProject<Projects.SideEffect_Demo_Web>("demo-web")
    .WithReference(rabbitmq).WaitFor(rabbitmq)
    .WithReference(redis).WaitFor(redis);

builder.AddProject<Projects.SideEffect_Demo_Worker>("demo-worker")
    .WithReference(rabbitmq).WaitFor(rabbitmq)
    .WithReference(redis).WaitFor(redis);

await builder.Build().RunAsync();
