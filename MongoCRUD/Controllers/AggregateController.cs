using Microsoft.AspNetCore.Mvc;
using MongoCRUD.Models;
using MongoDB.Driver;

namespace MongoCRUD.Controllers;

[Route("api/[controller]"), ApiController]
public class AggregateController(DbContext db) : ControllerBase
{
    private readonly FilterDefinitionBuilder<Person> _bf = Builders<Person>.Filter;
    private readonly SortDefinitionBuilder<Person> _bs = Builders<Person>.Sort;

    [HttpGet("Match")]
    public async Task<dynamic> GetMatch()
    {
        var result = await db.Person.Aggregate()
                             .Match(_bf.Eq(c => c.Name, "张三") & _bf.Gt(c => c.Age, 40))
                             .ToListAsync();
        return result;
    }

    [HttpGet("Group")]
    public async Task<dynamic> GetGroup()
    {
        var result = await db.Person.Aggregate()
                             .Match(_bf.Gt(c => c.Age, 40))
                             .Group(c => new { c.Gender }, g => new
                             {
                                 g.Key,
                                 Count = g.Sum(c => 1)
                             })
                             .Project(c => new
                             {
                                 性别 = c.Key.Gender,
                                 人数 = c.Count
                             }).ToListAsync();
        return result;
    }

    [HttpPost("Page")]
    public async Task<dynamic> Page()
    {
        // 该方法往往需要传入一个对象,因此使用Post
        // 首先我们使用动态类型创建一个分页数据的对象.
        var post = new { Index = 4, Size = 3 };
        // 这里有多种写法,我先写个两三种.
        var result1 = await db.Person.Aggregate()
                              .Match(_bf.Gt(c => c.Age, 30))
                              .SortByDescending(c => c.Age).ThenBy(c => c.Gender)
                              .Skip((post.Index - 1) * post.Size)
                              .Limit(post.Size)
                              .ToListAsync();
        var result2 = await db.Person.Find(_bf.Gt(c => c.Age, 30))
                              .Skip((post.Index - 1) * post.Size)
                              .Limit(post.Size)
                              .SortByDescending(c => c.Age).ThenBy(c => c.Gender)
                              .ToListAsync();
        var result3 = await db.Person.FindAsync(_bf.Gt(c => c.Age, 30),
                          new FindOptions<Person, Person>
                          {
                              Skip = (post.Index - 1) * post.Size,
                              Limit = post.Size,
                              Sort = _bs.Descending(c => c.Age).Ascending(c => c.Gender)
                          }).Result.ToListAsync();
        return new Tuple<dynamic, dynamic, dynamic>(result1, result2, result3);
    }
}