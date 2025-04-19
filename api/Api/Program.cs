using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect("redis:6379"));

var app = builder.Build();

app.MapPost("/increment", async (IConnectionMultiplexer redis) =>
{
    var enabled = bool.TryParse(
        Environment.GetEnvironmentVariable("INCREMENT"), 
        out var flag
    ) && flag;

    if (!enabled)
        return Results.Forbid();

    var db = redis.GetDatabase();
    var newValue = await db.StringIncrementAsync("counter");
    return Results.Ok(newValue);
})
.WithName("Increment")
.WithOpenApi();

app.Run();