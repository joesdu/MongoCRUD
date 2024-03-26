using EasilyNET.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using MongoCRUD.Models;
using MongoDB.Driver;

namespace MongoCRUD.Controllers;

[ApiController, Route("api/[controller]")]
public class MongoCrudController(DbContext db) : ControllerBase
{
    private readonly FilterDefinitionBuilder<Person> _bf = Builders<Person>.Filter;
    private readonly UpdateDefinitionBuilder<Person> _bu = Builders<Person>.Update;

    [HttpPost("One")]
    public async Task<Person> InsertOne()
    {
        // 这里我们不需要为 Id 字段赋值,因为插入成功后,会自动为Id字段绑定值.
        var person = new Person
        {
            Name = "张三",
            Age = 20,
            Birthday = DateOnly.FromDateTime(DateTime.Now),
            Gender = EGender.女,
            Index = 0
        };
        await db.Person.InsertOneAsync(person);
        Console.WriteLine(person.Id);
        return person;
    }

    [HttpPost("Many")]
    public async Task<IEnumerable<Person>> InsertMany()
    {
        var list = new List<Person>();
        for (var i = 0; i < 100; i++)
        {
            list.Add(new()
            {
                Name = "张三",
                Age = 20 + i,
                Birthday = DateOnly.FromDateTime(DateTime.Now),
                Gender = i % 2 == 0 ? EGender.女 : EGender.男,
                Index = i
            });
        }
        await db.Person.InsertManyAsync(list);
        return list;
    }

    [HttpGet("All")]
    public async Task<IEnumerable<Person>> FindAll() =>
        // return await _db.Person.Find("{}").ToListAsync();
        // 两种写法等效,但是并不建议在C#中直接写JSON字符串查询,除非一些特殊情况.
        await db.Person.Find(_bf.Empty).ToListAsync();

    [HttpGet("IndexIs0")]
    public async Task<IEnumerable<Person>> FindIndexIs0()
    {
        return await db.Person.Find(_bf.Eq(c => c.Index, 0)).ToListAsync();
    }

    [HttpPut("One/{index:int}")]
    public async Task UpdateOne(int index)
    {
        // 展示拉姆达表达式的条件方式,以及第三个可选参数的配置.
        _ = await db.Person.UpdateOneAsync(c => c.Index == index, _bu.Set(c => c.Age, 17), new() { IsUpsert = true });
    }

    [HttpPut("IndexIs0")]
    public async Task UpdateIndexIs0()
    {
        _ = await db.Person.UpdateManyAsync(_bf.Eq(c => c.Index, 0), _bu.Set(c => c.Name, "李四"));
    }

    [HttpDelete("One/{index:int}")]
    public async Task DeleteOne(int index)
    {
        _ = await db.Person.DeleteOneAsync(c => c.Index == index);
        // 两种写法等效
        //_ = await _db.Person.DeleteOneAsync(_bf.Eq(c => c.Index, index));
    }

    [HttpDelete("Many")]
    public async Task DeleteMany()
    {
        var indexs = new[] { 12, 25, 14, 36, 95, 42 };
        _ = await db.Person.DeleteManyAsync(c => indexs.Contains(c.Index));
        // 两种写法等效.
        //_ = await _db.Person.DeleteManyAsync(_bf.In(c => c.Index, indexs));
    }
}