using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JSONSharp.lexer;
using JSONSharp.types;
using JSONSharp.Visitor;
using JSONSharp;
using System.Runtime.Serialization;


namespace JSONSharp.Parser;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8605 // Unboxing a possibly null value.


public class Parser
{
    private readonly List<Token> _tokens;
    private int _current = 0;
    private bool _inObjectOrArray = false;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    public JSON Parse()
    {
        JSON json;
        if (Match(TokenType.LEFT_CURLY_BRACKET))
        {
            _inObjectOrArray = true;
            json = ParseJSONObject();
		}
        else if (Match(TokenType.LEFT_SQUARE_BRACKET))
        {
			_inObjectOrArray = true;
			json = ParseJSONArray();
		}
        else if (_inObjectOrArray)
        {
            json = ParseJSONValue();
        }
        else
        {
            Log.Error(Peek(), "Invalid Json. Expected '{' or '['");
            throw new ParseError();
        }

        return json;
    }

    private JSON ParseJSONValue()
    {
        Token token = Peek();
        switch (token.type)
        {
            case TokenType.NUMBER:
                return ParseJSONNumber();
            case TokenType.BOOL:
                return ParseJSONBool();
            case TokenType.STRING:
                return ParseJSONString();
            case TokenType.NULL:
                return ParseJSONNull();
            default:
                Log.Error(Peek(), "Expected value");
                throw new ParseError();
        }
    }

    private JSONArray ParseJSONArray()
    {
        JSONArray array = new JSONArray();
        bool empty = true;
        do
        {
			if (Check(TokenType.RIGHT_SQUARE_BRACKET) && empty) break; // empty array
            empty = false;
			array.Values.Add(Parse());
        } while (Match(TokenType.COMMA));

        Consume(TokenType.RIGHT_SQUARE_BRACKET, "Expected ']' at end of array");

        return array;
    }

    private JSONObject ParseJSONObject()
    {
        JSONObject obj = new();
		bool empty = true;
		do
        {
            if (Check(TokenType.RIGHT_CURLY_BRACKET) && empty) break; // empty object
			empty = false;
			Token identifierToken = Consume(TokenType.IDENTIFIER, "Expected Identifer or '}'");
            string identifierName = (string)identifierToken.literal;
            Consume(TokenType.COLON, "Expected ':' after identifier");
            obj[identifierName] = Parse();

        } while (Match(TokenType.COMMA));

        Consume(TokenType.RIGHT_CURLY_BRACKET, "Expected '}' at end of object");

        return obj;
    }

    public JSONValue ParseJSONBool()
    {
        JSONValue json = new JSONBool((bool)Peek().literal);
        Advance();
        return json;
    }
    public JSONValue ParseJSONNumber()
    {
        JSONValue json = new JSONNumber((double)Peek().literal);
        Advance();
        return json;
    }
    public JSONValue ParseJSONNull()
    {
        JSONValue json = new JSONNull();
        Advance();
        return json;
    }

    public JSONValue ParseJSONString()
    {
        JSONValue json = new JSONString((string)Peek().literal);
        Advance();
        return json;
    }


    private Token Consume(TokenType type, string message)
    {
        if (Check(type)) return Advance();

        Log.Error(Peek(), message);
        throw new ParseError(message);
    }

    private bool Check(TokenType type)
    {
        if (IsAtEnd()) return false;
        return Peek().type == type;
    }

    private Token Advance()
    {
        if (!IsAtEnd()) _current++;
        return Previous();
    }

    private bool IsAtEnd()
    {
        return Peek().type == TokenType.EOF;
    }

    private Token Peek()
    {
        return _tokens.ElementAt(_current);
    }

    private Token Previous()
    {
        return _tokens.ElementAt(_current - 1);
    }

    private bool Match(TokenType type)
    {
        if (Check(type))
        {
            Advance();
            return true;
        }
        return false;  
    }

}


internal class ParseError : Exception
{
    public ParseError()
    {
    }

    public ParseError(string? message) : base(message)
    {
    }

    public ParseError(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}