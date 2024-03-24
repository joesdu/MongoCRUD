// ReSharper disable ClassNeverInstantiated.Global

namespace MongoCRUD.Models;

public class FamilyInfo
{
    /// <summary>
    /// 数据ID
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 家庭成员
    /// </summary>
    public List<Person> Members { get; set; } = [];
}