using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect("redis:6379"));

var app = builder.Build();

app.MapGet("/features", () =>
    {
        var features = builder.Configuration.GetSection("Features").Get<Dictionary<string, bool>>();
        return Results.Ok(features);
    })
    .WithName("Features")
    .WithOpenApi()
    .Produces(StatusCodes.Status200OK);

app.MapGet("/counter", async (IConnectionMultiplexer redis) =>
    {
        var db = redis.GetDatabase();
        var value = await db.StringGetAsync("counter");
        return value.TryParse(out int count) ? Results.Ok(count) : Results.BadRequest();
    })
    .WithName("Get")
    .WithOpenApi()
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest);

app.MapPost("/counter/increment", async (IConnectionMultiplexer redis) =>
    {
        var enabled = builder.Configuration.GetValue<bool>("Features:Increment");

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