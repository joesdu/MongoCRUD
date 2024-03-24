using Microsoft.AspNetCore.Mvc;
using MongoCRUD.Models;
using MongoDB.Driver;

namespace MongoCRUD.Controllers;

[Route("api/[controller]"), ApiController]
public class IndexesController(DbContext db) : ControllerBase
{
    [HttpPut("CreateIndexes")]
    public async Task CreateIndexes()
    {
        var indexModel = new CreateIndexModel<Cat>(Builders<Cat>.IndexKeys.Ascending(c => c.Id).Descending(c => c.No),
            new()
            {
                Name = "Id 1 No -1",
                Background = true
            });
        _ = await db.Cat.Indexes.CreateOneAsync(indexModel);
    }

    [HttpPut("DeleteIndexes")]
    public async Task DeleteIndexes()
    {
        await db.Cat.Indexes.DropOneAsync("Id 1 No -1");
    }
}