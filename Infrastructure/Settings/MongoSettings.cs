using MongoDB.Driver.Core.Configuration;

namespace Infrastructure.Settings;

public class MongoSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}