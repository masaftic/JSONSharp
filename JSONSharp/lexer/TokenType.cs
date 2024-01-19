namespace JSONSharp.lexer;

public enum TokenType
{
    LEFT_CURLY_BRACKET,
    RIGHT_CURLY_BRACKET,
    LEFT_SQUARE_BRACKET,
    RIGHT_SQUARE_BRACKET,

    COMMA,
    COLON,

    IDENTIFIER,

    NUMBER,
    STRING,
    BOOL,
    NULL,

    EOF
}