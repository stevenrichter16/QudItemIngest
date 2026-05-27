using Domain.Entities;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Infrastructure.Context;

public class MongoContext
{
    private IMongoDatabase Database { get; }

    public MongoContext(IOptions<MongoSettings> settings)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        var client = new MongoClient(settings.Value.ConnectionString);
        Database = client.GetDatabase(settings.Value.DatabaseName);
    }

    // TODO: Make a WeaponDocument and use a Mapper later
    // GetCollection creates the collection if it doesn't exist
    public IMongoCollection<Weapon> Weapons => Database.GetCollection<Weapon>("weapons");
}