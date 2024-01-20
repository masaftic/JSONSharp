using System.Text.Json.Nodes;
using JSONSharp.lexer;
using JSONSharp.Parser;
using JSONSharp.PrettyPrint;
using JSONSharp.types;

if (args.Length != 1)
{
    Console.WriteLine($"Usage: dotnet run [file]");
    System.Environment.Exit(1);
}


Lexer lexer = new(File.ReadAllText(args[0]));

List<Token> tokens = [];
try
{
    tokens = lexer.GetTokens();
}
catch (LexError)
{
    Environment.Exit(1);
}


Parser parser = new Parser(tokens);
JSON json = null!;

try
{
    json = parser.Parse();
}
catch (ParseError)
{
    Environment.Exit(1);
}

var printer = new PrettyPrinter();
Console.WriteLine(printer.Stringifiy(json));

