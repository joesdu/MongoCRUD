using MongoCRUD.Models;
using MongoDB.Driver;
// ReSharper disable ClassNeverInstantiated.Global

namespace MongoCRUD;

public sealed class DbContext : BaseDbContext
{
    /// <summary>
    /// 人员信息
    /// </summary>
    public IMongoCollection<Person> Person => Database.GetCollection<Person>("person");
    /// <summary>
    /// 家庭信息
    /// </summary>
    public IMongoCollection<FamilyInfo> FamilyInfo => Database.GetCollection<FamilyInfo>("family.info");
}