# Basic JSON Parsing in C# from Scratch

A lightweight C# project for basic JSON parsing, providing functionality to lex JSON files, parse them into C# objects, and stringify C# objects.

## Features

- **Lexer:**
  - Lex JSON files to tokenize them for parsing.

- **Parser:**
  - Parse JSON files into C# objects.
  - Support for basic data types such as numbers, bools, strings, arrays, and objects.

- **Stringify:**
  - Convert C# objects back to JSON strings.

## Classes Usage

1. **Lexer:**
    ```csharp
    var lexer = new Lexer(@" {"id": 4} "); // init lexer with source
    Console.WriteLine(lexer.GetTokens());  // get tokens
    ```

2. **Parser:**
    ```csharp
    var parser = new Parser(lexer.GetTokens()); // init parser with tokens
    JSON json = parser.Parse();                 
    ```

3. **Stringify:**
    ```csharp
    var printer = new PrettyPrinter();
    Console.WriteLine(printer.Stringifiy(json));
    ```

4. **Nested Access:**
   ```
   Console.WriteLine(json["properties:age:title"]);
   Console.WriteLine(json["required:1"]);
   ```


## Getting Started
### Prerequisites
- .NET SDK [Link](https://dotnet.microsoft.com/en-us/download)

### Installation
- Clone the repository:

```bash
> git clone https://github.com/masaftic/JSONSharp
```

- Run the project:

```bash
> cd JSONSharp/
> dotnet run <File>
```