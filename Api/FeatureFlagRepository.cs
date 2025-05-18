using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Api;

public class FeatureSettings
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }

    [BsonElement("Features")]
    public FeatureFlags Features { get; set; } = null!;
}

public class FeatureFlags
{
    [BsonElement("Increment")]
    public bool Increment { get; set; }

    [BsonElement("Decrement")]
    public bool Decrement { get; set; }
}

public class FeatureFlagRepository
{
    private readonly IMongoCollection<FeatureSettings> Collection;

    public FeatureFlagRepository(
        string connectionString = "mongodb://mongo:27017",
        string databaseName = "myappdb",
        string collectionName = "settings"
    )
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        Collection = database.GetCollection<FeatureSettings>(collectionName);
    }

    public async Task<bool> GetFeatureAsync(string featureName)
    {
        var settings = await Collection
            .Find(_ => true)
            .FirstOrDefaultAsync();

        if (settings == null)
            throw new InvalidOperationException("No feature settings found.");

        return featureName.Trim().ToLowerInvariant() switch
        {
            "increment" => settings.Features.Increment,
            "decrement" => settings.Features.Decrement,
            _ => throw new ArgumentException($"Unknown feature '{featureName}'.", nameof(featureName))
        };
    }
}
