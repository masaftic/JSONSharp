using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JSONSharp.Visitor;

namespace JSONSharp.types;




public abstract class JSONValue : JSON
{
}

public class JSONArray : JSONValue
{
    public List<JSON> Values { get; } = new List<JSON>();

    public int Count {get => Values.Count; }

    public override R Accept<R>(IVisitor<R> visitor)
    {
        return visitor.VisitJSONArray(this);
    }
}

public class JSONNumber : JSONValue
{
    public double Value { get; }
    public JSONNumber(double value)
    {
        Value = value;
    }

    public override R Accept<R>(IVisitor<R> visitor)
    {
        return visitor.VisitJSONNumber(this);
    }
}

public class JSONBool : JSONValue
{
    public bool Value { get; }
    public JSONBool(bool value)
    {
        Value = value;
    }

    public override R Accept<R>(IVisitor<R> visitor)
    {
        return visitor.VisitJSONBool(this); 
    }
}

public class JSONString : JSONValue
{
    public string Value { get; }
    public JSONString(string value)
    {
        Value = value;
    }

    public override R Accept<R>(IVisitor<R> visitor)
    {
        return visitor.VisitJSONString(this);
    }
}

public class JSONNull : JSONValue
{
    public object? Value { get; } = null;

    public override R Accept<R>(IVisitor<R> visitor)
    {
        return visitor.VisitJSONNUll(this);
    }
}