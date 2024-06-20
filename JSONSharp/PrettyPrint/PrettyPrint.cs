using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSONSharp.types;
using JSONSharp.Visitor;

namespace JSONSharp.PrettyPrint;


public class PrettyPrinterOptions
{
    public int SpaceCount { get; set; } = 4;
}

public class PrettyPrinter : IVisitor<string>
{
    private int _nestLevel = 0;
    private readonly PrettyPrinterOptions _options;
    

    public int Identation => _nestLevel * _options.SpaceCount;

    public PrettyPrinter(Action<PrettyPrinterOptions>? configureOptions = null)
    {
        var options = new PrettyPrinterOptions();
        configureOptions?.Invoke(options);
        _options = options;
    }

    public string Stringifiy(JSONValue json)
    {
        return json.Accept(this);
    }

    public string VisitJSONArray(JSONArray json)
    {
        StringBuilder builder = new();

        builder.Append("[\n");

        _nestLevel += 1;

        int index = 0;
        foreach (JSONValue value in json.Values)
        {
            builder.AppendWithIndent(value.Accept(this), Identation);
            if (index + 1 < json.Count) builder.Append(',');
            builder.Append('\n');

            index += 1;
        }

        _nestLevel -= 1;

        builder.AppendWithIndent("]", Identation);

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
        foreach ((string identifier, JSONValue child) in json.GetValues())
        {
            builder.AppendWithIndent(QuoteString(identifier), Identation);
            builder.Append(": ");
            builder.Append(child.Accept(this));
            if (index + 1 < json.Count) builder.Append(',');
            builder.Append('\n');
            index += 1;
        }

        _nestLevel -= 1;

        builder.AppendWithIndent("}", Identation);

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

    public static StringBuilder AppendWithIndent(this StringBuilder builder, char value, int spaceCount)
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

