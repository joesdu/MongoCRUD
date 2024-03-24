using EasilyNET.Mongo.Core;
using Microsoft.AspNetCore.Mvc;
using MongoCRUD.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MongoCRUD.Controllers;

[Route("api/[controller]"), ApiController]
public class MongoArrayController(DbContext db) : ControllerBase
{
    private readonly UpdateDefinitionBuilder<FamilyInfo> _bu = Builders<FamilyInfo>.Update;

    [HttpPost("Init")]
    public async Task<FamilyInfo> Init()
    {
        var obj = new FamilyInfo
        {
            Name = "野比家",
            Members =
            [
                new()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Index = 0,
                    Name = "野比大助",
                    Age = 40,
                    Gender = Gender.男,
                    Birthday = DateOnly.ParseExact("1943-01-24", "yyyy-MM-dd")
                },
                new()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Index = 1,
                    Name = "野比玉子",
                    Age = 34,
                    Gender = Gender.女,
                    Birthday = DateOnly.ParseExact("1941-09-30", "yyyy-MM-dd")
                },
                new()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Index = 2,
                    Name = "野比大雄",
                    Age = 10,
                    Gender = Gender.男,
                    Birthday = DateOnly.ParseExact("1964-08-07", "yyyy-MM-dd")
                },
                new()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Index = 3,
                    Name = "哆啦A梦",
                    Age = 1,
                    Gender = Gender.男,
                    Birthday = DateOnly.ParseExact("2112-09-03", "yyyy-MM-dd")
                }
            ]
        };
        await db.FamilyInfo.InsertOneAsync(obj);
        return obj;
    }

    [HttpPost("Create")]
    public async Task<Person> AddOneElement()
    {
        var dorami = new Person
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Index = 4,
            Name = "哆啦美",
            Age = 1,
            Gender = Gender.女,
            Birthday = DateOnly.ParseExact("2114-12-02", "yyyy-MM-dd")
        };
        await db.FamilyInfo.UpdateOneAsync(c => c.Name == "野比家", _bu.Push(c => c.Members, dorami));
        return dorami;
    }

    [HttpPut("UpdateOne")]
    public async Task UpdateOneElement()
    {
        // 这里我们举得例子是将哆啦美的名字变更为日文名字.
        // 这里我们假设查询参数同样是通过参数传入的,所以我们写出了如下代码.
        await db.FamilyInfo.UpdateOneAsync(c => (c.Name == "野比家") & c.Members.Any(s => s.Index == 4),
            _bu.Set(c => c.Members.FirstMatchingElement().Name, "ドラミ"));
    }

    [HttpDelete("DeleteOne")]
    public async Task DeleteOneElement()
    {
        await db.FamilyInfo.UpdateOneAsync(c => c.Name == "野比家", _bu.PullFilter(c => c.Members, f => f.Index == 4));
    }

    [HttpGet("OneElement")]
    public async Task<Person> GetOneElement()
    {
        //var sql = _db.FamilyInfo.Find(c => c.Name == "野比家")
        //    .Project(c => c.Members.First()).ToString();
        return await db.FamilyInfo.Find(c => c.Name == "野比家").Project(c => c.Members.First()).SingleOrDefaultAsync();
        //return await _db.FamilyInfo.Find(c => c.Name == "野比家")
        //    .Project(c => c.Members.Find(s => s.Name == "哆啦A梦")).SingleOrDefaultAsync();
    }

    [HttpGet("AllElement")]
    public async Task<IEnumerable<Person>> GetAllElement()
    {
        return await db.FamilyInfo.Find(c => c.Name == "野比家").Project(c => c.Members).SingleOrDefaultAsync();
    }

    [HttpGet("Unwind")]
    public async Task<dynamic> GetUnwind()
    {
        // Project中的UnwindObj我们往往使用子类,这样可以将一些不必要的数据屏蔽或者丢弃
        var query = db.FamilyInfo.Aggregate().Match(c => c.Name == "野比家")
                      .Project(c => new UnwindObj<List<Person>>
                      {
                          Obj = c.Members,
                          Count = c.Members.Count
                      })
                      .Unwind(c => c.Obj, new AggregateUnwindOptions<UnwindObj<Person>> { IncludeArrayIndex = "Index" });
        //var sql = query.ToString();
        var total = query.Count().FirstOrDefaultAsync().Result.Count;
        var list = await query.Skip(2).Limit(1).ToListAsync();
        return new Tuple<long?, List<UnwindObj<Person>>>(total, list);
    }
}