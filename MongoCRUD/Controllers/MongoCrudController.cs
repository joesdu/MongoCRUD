using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace MongoCRUD.Controllers;
[ApiController, Route("[controller]")]
public class MongoCrudController : ControllerBase
{
    private readonly DbContext _db;
    public MongoCrudController(DbContext db)
    {
        _db = db;
    }

    private readonly FilterDefinitionBuilder<Person> _bf = Builders<Person>.Filter;
    private readonly UpdateDefinitionBuilder<Person> _bu = Builders<Person>.Update;

    [HttpPost("one")]
    public async Task<Person> InsertOne()
    {
        // �������ǲ���ҪΪ Id �ֶθ�ֵ,��Ϊ����ɹ���,���Զ�ΪId�ֶΰ�ֵ.
        var person = new Person
        {
            Name = "����",
            Age = 20,
            Birthday = DateOnly.FromDateTime(DateTime.Now),
            Gender = Gender.Ů,
            Index = 0
        };
        await _db.Person.InsertOneAsync(person);
        Console.WriteLine(person.Id);
        return person;
    }

    [HttpPost("many")]
    public async Task<IEnumerable<Person>> InsertMany()
    {
        var list = new List<Person>();
        for (var i = 0; i < 100; i++)
        {
            list.Add(new()
            {
                Name = "����",
                Age = 20 + i,
                Birthday = DateOnly.FromDateTime(DateTime.Now),
                Gender = i % 2 == 0 ? Gender.Ů : Gender.��,
                Index = i
            });
        }
        await _db.Person.InsertManyAsync(list);
        return list;
    }

    [HttpGet("all")]
    public async Task<IEnumerable<Person>> FindAll()
    {
        // return await _db.Person.Find("{}").ToListAsync();
        // ����д����Ч,���ǲ���������C#��ֱ��дJSON�ַ�����ѯ,����һЩ�������.
        return await _db.Person.Find(_bf.Empty).ToListAsync();
    }

    [HttpGet("IndexIs0")]
    public async Task<IEnumerable<Person>> FindIndexIs0()
    {
        return await _db.Person.Find(_bf.Eq(c => c.Index, 0)).ToListAsync();
    }

    [HttpPut("one/{index:int}")]
    public async Task UpdateOne(int index)
    {
        // չʾ��ķ����ʽ��������ʽ,�Լ���������ѡ����������.
        _ = await _db.Person.UpdateOneAsync(c => c.Index == index, _bu.Set(c => c.Age, 17), new() { IsUpsert = true });
    }

    [HttpPut("IndexIs0")]
    public async Task UpdateIndexIs0()
    {
        _ = await _db.Person.UpdateManyAsync(_bf.Eq(c => c.Index, 0), _bu.Set(c => c.Name, "����"));
    }

    [HttpDelete("one/{index:int}")]
    public async Task DeleteOne(int index)
    {
        _ = await _db.Person.DeleteOneAsync(c => c.Index == index);
        // ����д����Ч
        //_ = await _db.Person.DeleteOneAsync(_bf.Eq(c => c.Index, index));
    }

    [HttpDelete("many")]
    public async Task DeleteMany()
    {
        var indexs = new[] { 12, 25, 14, 36, 95, 42 };
        _ = await _db.Person.DeleteManyAsync(c => indexs.Contains(c.Index));
        // ����д����Ч.
        //_ = await _db.Person.DeleteManyAsync(_bf.In(c => c.Index, indexs));
    }
}
