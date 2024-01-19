using JSONSharp.lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONSharp;

class Log
{
	public static void Error(Token token, string message)
	{
        Console.WriteLine($"Error: {token.lexeme} at [line: {token.line}].");
		Console.WriteLine(message);
	}
}

