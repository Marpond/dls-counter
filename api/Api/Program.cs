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
            return Results.StatusCode(StatusCodes.Status403Forbidden);

        var db = redis.GetDatabase();
        var newValue = await db.StringIncrementAsync("counter");
        return Results.Ok(newValue);
    })
    .WithName("Increment")
    .WithOpenApi()
    .Produces<long>(StatusCodes.Status200OK) //the default response is 200 OK, but I explicitly write it here for improved readability.
    .Produces(StatusCodes.Status403Forbidden);

app.Run();