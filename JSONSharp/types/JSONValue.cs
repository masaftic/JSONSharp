using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JSONSharp.Visitor;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JSONSharp.types;


public abstract class JSONValue
{
    public abstract R Accept<R>(IVisitor<R> visitor);
    public abstract override string ToString();
}

public class JSONNumber : JSONValue
{
    public double Value { get; }
    public JSONNumber(double value)
    {
        Value = value;
    }

    public static implicit operator double(JSONNumber number) => number.Value;
    public static explicit operator JSONNumber(double number) => new JSONNumber(number);

    public override R Accept<R>(IVisitor<R> visitor)
    {
        return visitor.VisitJSONNumber(this);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

public class JSONBool : JSONValue
{
    public bool Value { get; }
    public JSONBool(bool value)
    {
        Value = value;
    }

    public static implicit operator bool(JSONBool @bool) => @bool.Value;
    public static explicit operator JSONBool(bool @bool) => new JSONBool(@bool);

    public override R Accept<R>(IVisitor<R> visitor)
    {
        return visitor.VisitJSONBool(this); 
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

public class JSONString : JSONValue
{
    public string Value { get; }
    public JSONString(string value)
    {
        Value = value;
    }

    public static implicit operator string(JSONString @string) => @string.Value;
    public static explicit operator JSONString(string @string) => new JSONString(@string);

    public override R Accept<R>(IVisitor<R> visitor)
    {
        return visitor.VisitJSONString(this);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

public class JSONNull : JSONValue
{
    public override R Accept<R>(IVisitor<R> visitor)
    {
        return visitor.VisitJSONNUll(this);
    }

    public override string ToString()
    {
        return "null";
    }
}