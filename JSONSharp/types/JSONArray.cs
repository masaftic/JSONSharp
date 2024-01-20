using JSONSharp.Visitor;

namespace JSONSharp.types;

public class JSONArray : JSON
{
    public List<JSON> Values { get; } = new List<JSON>();

    public int Count {get => Values.Count; }

    public override R Accept<R>(IVisitor<R> visitor)
    {
        return visitor.VisitJSONArray(this);
    }
}
