using MongoDB.Bson.Serialization.Attributes;

namespace MongoCRUD;

public class UnwindObj<T>
{
    /// <summary>
    /// 1.T as List,use for Projection,
    /// 2.T as single Object,use for MongoDB array field Unwind result
    /// </summary>
    [BsonElement("Obj")]
    public T? Obj { get; set; }
    /// <summary>
    /// when T as List,record Count
    /// </summary>
    [BsonElement("Count")]
    public int Count { get; set; }
    /// <summary>
    /// record array field element's index before Unwinds
    /// </summary>
    [BsonElement("Index")]
    public int Index { get; set; }
}