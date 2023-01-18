using Microsoft.AspNetCore.Mvc;
using MongoCRUD.Extensions;
using MongoCRUD.Models;
using MongoDB.Driver;

namespace MongoCRUD.Controllers;
[Route("api/[controller]"), ApiController]
public class TransactionController : ControllerBase
{
    private readonly DbContext _db;
    public TransactionController(DbContext db)
    {
        _db = db;
    }

    private readonly FilterDefinitionBuilder<Cat> _cbf = Builders<Cat>.Filter;
    private readonly UpdateDefinitionBuilder<Cat> _cbu = Builders<Cat>.Update;
    private readonly FilterDefinitionBuilder<Dog> _dbf = Builders<Dog>.Filter;
    private readonly UpdateDefinitionBuilder<Dog> _dbu = Builders<Dog>.Update;

    [HttpPost]
    public async Task Demo()
    {
        var cats = new List<Cat>();
        var dogs = new List<Dog>();
        // 这个地方的语法是从Kotlin中学来的,使用CustomIntEnumeratorExtension.cs实现
        foreach (var index in ..99)
        {
            cats.Add(new()
            {
                Name = $"Tom-{index}",
                Description = "Tom Cat"
            });
        }
    }
}
