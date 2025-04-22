using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect("redis:6379"));

var app = builder.Build();

app.MapGet("/counter", async (IConnectionMultiplexer redis) =>
    {
        var db = redis.GetDatabase();
        var value = await db.StringGetAsync("counter");
        return value.TryParse(out int count) ? Results.Ok(count) : Results.BadRequest();
    })
    .WithName("Get")
    .WithOpenApi()
    .Produces(StatusCodes.Status200OK);

app.MapPost("/counter/increment", async (IConnectionMultiplexer redis) =>
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
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status403Forbidden);

app.Run();