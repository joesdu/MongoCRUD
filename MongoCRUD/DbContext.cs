using EasilyNET.Mongo.Core;
using MongoCRUD.Models;
using MongoDB.Driver;

// ReSharper disable ClassNeverInstantiated.Global

namespace MongoCRUD;

public sealed class DbContext : MongoContext
{
    /// <summary>
    /// 人员信息
    /// </summary>
    public IMongoCollection<Person> Person => Database.GetCollection<Person>("person");

    /// <summary>
    /// 家庭信息
    /// </summary>
    public IMongoCollection<FamilyInfo> FamilyInfo => Database.GetCollection<FamilyInfo>("family.info");

    /// <summary>
    /// 可爱猫猫
    /// </summary>
    public IMongoCollection<Cat> Cat => Database.GetCollection<Cat>("cute.cat");

    /// <summary>
    /// 可爱狗狗
    /// </summary>
    public IMongoCollection<Dog> Dog => Database.GetCollection<Dog>("cute.dog");
}