using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JSONSharp.Visitor;

namespace JSONSharp.types;

public class JSONObject : JSON
{
    private readonly Dictionary<string, JSON> values;
    public JSON this[string key] { get => values[key]; set => values[key] = value; }

    public int Count {get => values.Count; }

    public JSONObject()
    {
        values = new Dictionary<string, JSON>();
    }

    public IEnumerable<(string, JSON)> GetValues()
    {
        foreach (var it in values)
        {
            yield return (it.Key, it.Value);
        }
    }

    public override R Accept<R>(IVisitor<R> visitor)
    {
        return visitor.VisitJSONObject(this);
    }
}
