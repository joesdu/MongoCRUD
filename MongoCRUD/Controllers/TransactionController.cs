using Microsoft.AspNetCore.Mvc;
using MongoCRUD.Extensions;
using MongoCRUD.Models;
using MongoDB.Driver;

namespace MongoCRUD.Controllers;
[Route("api/[controller]/[action]"), ApiController]
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
                No = index,
                Name = $"Cat-{index}",
                Description = "Tom Cat"
            });
            dogs.Add(new()
            {
                No = index,
                Name = $"Dog-{index}",
                Description = "Spike Dog"
            });
        }
        // 后期为了模拟异常,这里用一个try catch
        // 比如我们先批量添加 100 个 猫咪 🐱 和 狗狗 🐕,然后将序号大于 50 的猫咪名称改成 Tom,狗狗的名称改成 Spike,然后将序号小于 10 的猫咪和狗狗都删掉.
        // 这里就开始要用事务了.先获取 session
        var session = await _db.Client.StartSessionAsync();
        try
        {
            // 这里记住一定要开始事务,不然也不行.
            session.StartTransaction();
            // 这里的第一个参数一定要传,不然就不会使用事务.
            await _db.Cat.InsertManyAsync(session, cats);
            await _db.Dog.InsertManyAsync(session, dogs);
            _ = await _db.Cat.UpdateManyAsync(session, _cbf.Gt(c => c.No, 50), _cbu.Set(c => c.Name, "Tom"));
            _ = await _db.Dog.UpdateManyAsync(session, _dbf.Gt(c => c.No, 50), _dbu.Set(c => c.Name, "Spike"));
            _ = await _db.Cat.DeleteManyAsync(session, c => c.No < 10);
            _ = await _db.Dog.DeleteManyAsync(session, c => c.No < 10);
            // 完成事务的操作后提交事务.
            await session.CommitTransactionAsync();
        }
        catch (Exception)
        {
            // 若是发生异常,退出事务
            await session.AbortTransactionAsync();
        }
    }

    [HttpPost]
    public async Task WithError()
    {
        var session = await _db.Client.StartSessionAsync();
        try
        {
            // 这里记住一定要开始事务,不然也不行.
            session.StartTransaction();
            _ = await _db.Cat.UpdateManyAsync(session, _cbf.Lte(c => c.No, 50), _cbu.Set(c => c.Name, "Spike"));
            _ = await _db.Dog.UpdateManyAsync(session, _dbf.Lte(c => c.No, 50), _dbu.Set(c => c.Name, "Tom"));
            _ = await _db.Cat.DeleteManyAsync(session, _ => true);
            _ = await _db.Dog.DeleteManyAsync(session, _ => true);
            throw new("error");
            // 完成事务的操作后提交事务.这里的预处理指令是为了消除警告.
            // ReSharper disable once HeuristicUnreachableCode
#pragma warning disable CS0162
            await session.CommitTransactionAsync();
#pragma warning restore CS0162
        }
        catch (Exception)
        {
            // 若是发生异常,退出事务
            await session.AbortTransactionAsync();
        }
    }
}
