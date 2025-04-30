using Api;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FeatureFlagRepository>();
builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect("redis:6379"));

var app = builder.Build();

app.MapGet("/features", async (FeatureFlagRepository repo) =>
    {
        var increment = await repo.GetFeatureAsync("Increment");
        var decrement = await repo.GetFeatureAsync("Decrement");

        var features = new Dictionary<string, bool>
        {
            ["Increment"] = increment,
            ["Decrement"] = decrement
        };

        return Results.Ok(features);
    })
    .WithName("Features")
    .WithOpenApi()
    .Produces(StatusCodes.Status200OK);

app.MapGet("/counter", async (IConnectionMultiplexer redis) =>
    {
        var db = redis.GetDatabase();
        var value = await db.StringGetAsync("counter");
        return value.TryParse(out int count)
            ? Results.Ok(count)
            : Results.BadRequest();
    })
    .WithName("GetCounter")
    .WithOpenApi()
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest);

app.MapPost("/counter/increment", async (IConnectionMultiplexer redis, FeatureFlagRepository repo) =>
    {
        if (!await repo.GetFeatureAsync("Increment"))
            return Results.StatusCode(StatusCodes.Status403Forbidden);

        var db = redis.GetDatabase();
        var newValue = await db.StringIncrementAsync("counter");
        return Results.Ok(newValue);
    })
    .WithName("IncrementCounter")
    .WithOpenApi()
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status403Forbidden);

app.MapPost("/counter/decrement", async (IConnectionMultiplexer redis, FeatureFlagRepository repo) =>
    {
        if (!await repo.GetFeatureAsync("Decrement"))
            return Results.StatusCode(StatusCodes.Status403Forbidden);

        var db = redis.GetDatabase();
        var newValue = await db.StringDecrementAsync("counter");
        return Results.Ok(newValue);
    })
    .WithName("DecrementCounter")
    .WithOpenApi()
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status403Forbidden);

app.Run();