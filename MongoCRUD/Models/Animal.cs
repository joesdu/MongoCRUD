// ReSharper disable ClassNeverInstantiated.Global

namespace MongoCRUD.Models;

public class Animal
{
    /// <summary>
    /// 数据ID
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 序号
    /// </summary>
    public int No { get; set; }

    /// <summary>
    /// 名字
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// 可爱猫咪
/// </summary>
public sealed class Cat : Animal;

/// <summary>
/// 可爱狗狗
/// </summary>
public sealed class Dog : Animal;