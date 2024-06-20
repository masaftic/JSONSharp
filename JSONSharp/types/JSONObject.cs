using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JSONSharp.PrettyPrint;
using JSONSharp.Visitor;

namespace JSONSharp.types;

public class JSONObject : JSONValue
{
    public Dictionary<string, JSONValue> Values { get; }
    public JSONValue this[string key] 
    { 
        get 
        {
            string[] keys = key.Split(":");
            JSONValue? current = this;
            foreach (string to in keys)
            {
                Console.WriteLine(current.GetType());
                if (int.TryParse(to, out int index))
                {
                    current = ((JSONArray)current).Values[index];
                }
                else if (((JSONObject)current).Values.TryGetValue(to, out var value))
                {
                    current = ((JSONObject)current).Values[to];
                }
                else
                {
                    throw new KeyNotFoundException($"Key '{to}' not found or index out of range.");
                }
            }
            return current;
        }
        set 
        {
            // string[] keys = key.Split(":");
            // string lastKey = keys[^1];
            // JSONValue? current = this;

            // foreach (string to in keys)
            // {
            //     if (int.TryParse(to, out int index))
            //     {
            //         if (current is not JSONArray) current = new JSONArray();
            //         current = (JSONArray)current;

            //         while (current.Count <= index)
            //         {
            //             current.Values.Add(new JSONObject());
            //         }
            //         current = current.Values[index];
            //     }
            //     else if (current is JSONObject obj)
            //     {
            //         if (!obj.Values.TryGetValue(to, out var val))
            //         {
            //             val = new JSONObject();
            //             obj.Values.Add(to, val);
            //         }
            //         current = val;
            //     }
            //     //else if (current is JSONArray arr && int.TryParse(to, out int index))
            //     //{
            //     //    while (arr.Count <= index)
            //     //    {
            //     //        arr.Values.Add(new JSONObject());
            //     //    }
            //     //    current = arr.Values[index];
            //     //}
            //     else
            //     {
            //         throw new InvalidOperationException($"Cannot access key '{to}' in non-JsonObject");
            //     }
            // }

            //if (current is JSONObject finalobj) 
            //{
            //    finalobj.Values[lastKey] = value;
            //}
            //else if (current is JSONArray arr && int.TryParse(lastKey, out int index))
            //{
            //    while (arr.Count <= index)
            //    {
            //        arr.Values.Add(new JSONObject());
            //    }
            //    current = arr.Values[index];
            //}
            //else
            //{
            //}
                Values[key] = value;
        }
    }

    public int Count {get => Values.Count; }

    public JSONObject()
    {
        Values = new Dictionary<string, JSONValue>();
    }

    public IEnumerable<(string, JSONValue)> GetValues()
    {
        foreach (var it in Values)
        {
            yield return (it.Key, it.Value);
        }
    }

    public override R Accept<R>(IVisitor<R> visitor)
    {
        return visitor.VisitJSONObject(this);
    }

    public override string ToString()
    {
        StringBuilder builder = new();

        builder.Append("{ ");

        int index = 0;
        foreach ((string identifier, JSONValue child) in Values)
        {
            builder.Append('"' + identifier + '"');
            builder.Append(": ");
            builder.Append(child.ToString());
            if (index + 1 < Values.Count) builder.Append(", ");
            builder.Append(" ");
            index += 1;
        }

        builder.Append("} ");

        return builder.ToString();
    }
}
