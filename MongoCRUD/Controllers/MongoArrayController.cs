using Microsoft.AspNetCore.Mvc;
using MongoCRUD.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoCRUD.Controllers;
[Route("api/[controller]"), ApiController]
public class MongoArrayController : ControllerBase
{
    private readonly DbContext _db;
    public MongoArrayController(DbContext db)
    {
        _db = db;
    }

    private readonly FilterDefinitionBuilder<FamilyInfo> _bf = Builders<FamilyInfo>.Filter;
    private readonly UpdateDefinitionBuilder<FamilyInfo> _bu = Builders<FamilyInfo>.Update;

    [HttpPost("Init")]
    public async Task<FamilyInfo> Init()
    {
        var obj = new FamilyInfo()
        {
            Name = "野比家",
            Members = new()
            {
                new() 
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Index = 0,
                    Name = "野比大助",
                    Age = 40,
                    Gender = Gender.男,
                    Birthday = DateOnly.ParseExact("1943-01-24","yyyy-MM-dd")
                },
                new()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Index = 1,
                    Name = "野比玉子",
                    Age = 34,
                    Gender = Gender.女,
                    Birthday = DateOnly.ParseExact("1941-09-30","yyyy-MM-dd")
                },
                new()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Index = 2,
                    Name = "野比大雄",
                    Age = 10,
                    Gender = Gender.男,
                    Birthday = DateOnly.ParseExact("1964-08-07","yyyy-MM-dd")
                },
                new()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Index = 3,
                    Name = "哆啦A梦",
                    Age = 1,
                    Gender = Gender.男,
                    Birthday = DateOnly.ParseExact("2112-09-03","yyyy-MM-dd")
                }
            }
        };
        await _db.FamilyInfo.InsertOneAsync(obj);
        return obj;
    }
}
