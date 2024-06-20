using JSONSharp.PrettyPrint;
using JSONSharp.Visitor;
using System.Text;

namespace JSONSharp.types;

public class JSONArray : JSONValue
{
    public JSONValue this[string key]
    {
        get
        {
            string[] keys = key.Split(":");
            JSONValue? current = this;
            foreach (string to in keys)
            {
                if (int.TryParse(to, out var index))
                {
                    current = ((JSONArray)current).Values[index];
                }
                else
                {
                    current = ((JSONObject)current).Values[to];
                }
            }
            return current ?? throw new KeyNotFoundException();
        }
        set => Values[int.Parse(key)] = value;
    }


    public List<JSONValue> Values { get; } = [];

    public int Count { get => Values.Count; }

    public IEnumerable<JSONValue> GetValues()
    {
        foreach (var it in Values)
        {
            yield return it;
        }
    }

    public override R Accept<R>(IVisitor<R> visitor)
    {
        return visitor.VisitJSONArray(this);
    }

    public override string ToString()
    {
        StringBuilder builder = new();

        builder.Append("[ ");

        int index = 0;
        foreach (JSONValue value in Values)
        {
            builder.Append(value.ToString());
            if (index + 1 < Values.Count) builder.Append(", ");
            builder.Append(' ');

            index += 1;
        }
        builder.Append("] ");

        return builder.ToString();
    }
}
