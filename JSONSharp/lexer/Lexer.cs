using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;

namespace JSONSharp.lexer;


public class Lexer
{
    private readonly string _source;
    private int _current;
    private int _start;
    private int _line = 1;

    private List<Token> _tokens = new();

    private bool _lookingForIdentifier = true;
    private bool _inObjectLexing = true;

    private static readonly Dictionary<string, TokenType> _valueWords = new()
    {
        {"null", TokenType.NULL},
        {"true", TokenType.BOOL},
        {"false", TokenType.BOOL},
    };

    public Lexer(string source)
    {
        _source = source.Trim();
    }

    public List<Token> GetTokens()
    {
        while (!IsAtEnd())
        {
            _tokens.Add(NextToken());
        }
        _tokens.Add(new Token(TokenType.EOF, "", null, _line));
        return _tokens;
    }

    public Token NextToken()
    {
        IgnoreWhiteSpace();
        if (IsAtEnd()) return new Token(TokenType.EOF, "", null, _line);
		_start = _current;
        char c = Advance();
        switch (c)
        {
            case '{':
                _inObjectLexing = true;
                _lookingForIdentifier = true;
                return Tokenize(TokenType.LEFT_CURLY_BRACKET);
            case '}':
                return Tokenize(TokenType.RIGHT_CURLY_BRACKET);
            case '[':
                _inObjectLexing = false;
                return Tokenize(TokenType.LEFT_SQUARE_BRACKET);
            case ']':
                _inObjectLexing = true;
                return Tokenize(TokenType.RIGHT_SQUARE_BRACKET);
            case '"':
                return TokenizeString(_lookingForIdentifier && _inObjectLexing ? TokenType.IDENTIFIER : TokenType.STRING);
            case ':':
                _lookingForIdentifier = false;
                return Tokenize(TokenType.COLON);
            case ',':
                if (_inObjectLexing)
                    _lookingForIdentifier = true;
                return Tokenize(TokenType.COMMA);
            default:
                if (char.IsNumber(c))
                {
                    return TokenizeNumber();
                }
                if (char.IsAsciiLetterLower(c))
                {
                    string valueString = GetValueString();
                    if (_valueWords.TryGetValue(valueString, out TokenType type))
                    {
                        return Tokenize(type, type == TokenType.BOOL ? bool.Parse(valueString) : null);
                    }
                    Error("Unexpected character");
                    throw new LexError();
                }
                else
                {
                    Error("Unexpected character");
                    throw new LexError();
                }
        }
    }


    private Token TokenizeString(TokenType type)
    {
        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\n') _line++;
            Advance();
        }
        if (IsAtEnd())
        {
            Error("Unterminated string");
        }
        Advance();

        string textValue = _source[(_start + 1)..(_current - 1)];
        return Tokenize(type, textValue);
    }

    private string GetValueString()
    {
        while (char.IsAsciiLetterLower(Peek())) Advance();

        return GetLexeme();
    }

    private Token TokenizeNumber()
    {
        while (char.IsNumber(Peek()) || Peek() == 'e') Advance();

        if (Peek() == '.' && char.IsNumber(Peek(1)))
        {
            Advance();
            while (char.IsNumber(Peek()) || Peek() == 'e') Advance();
        }
        if (double.TryParse(GetLexeme(), out double result))
        {
            return Tokenize(TokenType.NUMBER, result);
        }
        Error("invalid number");
        throw new LexError();
    }

    private void IgnoreWhiteSpace()
    {
        bool isWhiteSpace = true;
        while (isWhiteSpace)
        {
            char c = Peek();
            switch (c)
            {
                case ' ':
                case '\r':
                case '\t':
                    _current++;
                    break;
                case '\n':
                    _current++;
                    _line++;
                    break;
                default:
                    isWhiteSpace = false;
                    break;
            }
        }
    }

    private char Advance()
    {
        return _source[_current++];
    }

    private string GetLexeme()
    {
        return _source[_start.._current];
    }

    private Token Tokenize(TokenType type, object? obj = null)
    {
        string lexeme = GetLexeme();
        return new Token(type, lexeme, obj, _line);
    }



    private char Peek(int ahead = 0)
    {
        if (IsAtEnd(ahead)) return '\0';
        return _source[_current + ahead];
    }

    private bool IsAtEnd(int ahead = 0)
    {
        return _current + ahead >= _source.Length;
    }


    private void Error(string message)
    {
        Console.Error.WriteLine($"[line {_line}] Error {message}");
    }
}

internal class LexError : Exception
{
	public LexError()
	{
	}

	public LexError(string? message) : base(message)
	{
	}

	public LexError(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}