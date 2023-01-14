namespace MongoCRUD;
/// <summary>
/// 人
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class Person
{
    /*
     *
     * "index": i,
        "name": "张三",
        "age": i + 1,
        "gender": "女",
        "birthday": new Date('2023-01-14')
     */

    /// <summary>
    /// 数据ID
    /// </summary>
    public string Id { get; set; } = string.Empty;
    /// <summary>
    /// 和例子中的数据结构同步
    /// </summary>
    public int Index { get; set; }
    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// 年龄
    /// </summary>
    public int Age { get; set; }
    /// <summary>
    /// 性别
    /// </summary>
    public Gender Gender { get; set; } = Gender.男;
    /// <summary>
    /// 临时决定,使用.Net 6新增类型保存生日,同时让例子变得丰富,明白如何将MongoDB不支持的数据类型序列化
    /// </summary>
    public DateOnly Birthday { get; set; }
}

public enum Gender
{
    男,
    女
}