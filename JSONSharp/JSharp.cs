using JSONSharp.lexer;
using JSONSharp.Parser;
using JSONSharp.PrettyPrint;
using JSONSharp.types;

if (args.Length != 1)
{
    Console.WriteLine($"Usage: dotnet run [file]");
    Environment.Exit(1);
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


//Parser parser = new Parser(tokens);
//JSONValue? json = null;

//try
//{
//    json = parser.Parse();
//}
//catch (ParseError)
//{
//    Environment.Exit(1);
//}

//var printer = new PrettyPrinter(options => options.SpaceCount = 2);
// Console.WriteLine(printer.Stringifiy(json));

// Console.WriteLine(((JSONObject)json)["properties:age:title"]);

//((JSONObject)json)["required:1:age"] = new JSONNumber(69);

//Console.WriteLine(((JSONObject)json)["required:1:age"]);


var jsun = new JSONObject();
jsun["book"] = new JSONNumber(21);

var ss = jsun["book"];


Console.WriteLine(ss);