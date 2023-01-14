using MongoDB.Driver;

namespace MongoCRUD;

public sealed class DbContext : BaseDbContext
{
    /// <summary>
    /// 
    /// </summary>
    public IMongoCollection<Person> Person => Database.GetCollection<Person>("person");
}