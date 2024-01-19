using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSONSharp.types;
using JSONSharp.Visitor;

namespace JSONSharp.PrettyPrint;


public class PrettyPrint : IVisitor<string>
{
    private int _nestLevel = 0;
    public int SpaceCount { get; set; } = 4;
    
    public string Stringifiy(JSON json)
    {
        return json.Accept(this);
    }

    public string VisitJSONArray(JSONArray json)
    {
        StringBuilder builder = new();

        builder.Append("[\n");

        _nestLevel += 1;

        int index = 0;
        foreach (JSON value in json.Values)
        {
            builder.AppendWithIndent(value.Accept(this), _nestLevel * SpaceCount);
            if (index + 1 < json.Count) builder.Append(',');
            builder.Append('\n');

            index += 1;
        }

        _nestLevel -= 1;

        builder.AppendWithIndent("]", _nestLevel * SpaceCount);

        return builder.ToString();
    }

    public string VisitJSONBool(JSONBool json)
    {
        return json.Value ? "true" : "false";
    }

    public string VisitJSONNUll(JSONNull json)
    {
        return "null";
    }

    public string VisitJSONNumber(JSONNumber json)
    {
        return json.Value.ToString();
    }

    public string VisitJSONObject(JSONObject json)
    {
        StringBuilder builder = new();

        builder.Append("{\n");

        _nestLevel += 1;

        int index = 0;
        foreach ((string identifier, JSON child) it in json.GetValues())
        {
            builder.AppendWithIndent(QuoteString(it.identifier), _nestLevel * SpaceCount);
            builder.Append(": ");
            builder.Append(it.child.Accept(this));
            if (index + 1 < json.Count) builder.Append(',');
            builder.Append('\n');
            index += 1;
        }

        _nestLevel -= 1;

        builder.AppendWithIndent("}", _nestLevel * SpaceCount);

        return builder.ToString();
    }

    public string VisitJSONString(JSONString json)
    {
        return QuoteString(json.Value);
    }

    public static string QuoteString(string value)
    {
        return string.Concat('"', value, '"');
    }
}


public static class StringBuilderExtensions
{
    public static StringBuilder AppendWithIndent(this StringBuilder builder, string value, int spaceCount)
    {
        builder.Append(GetSpaceString(spaceCount));
        builder.Append(value);
        return builder;
    }

    public static StringBuilder AppendWithSpace(this StringBuilder builder, char value, int spaceCount)
    {

        builder.Append(GetSpaceString(spaceCount));
        builder.Append(value);

        return builder;
    }

    private static string GetSpaceString(int spaceCount)
    {
        return string.Concat(Enumerable.Repeat(" ", spaceCount));
    }
}

