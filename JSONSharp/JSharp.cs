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


Parser parser = new Parser(lexer.GetTokens());
JSON son = parser.Parse();

PrettyPrinter printer = new();
Console.WriteLine(printer.Stringifiy(son));

