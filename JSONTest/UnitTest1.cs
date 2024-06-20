

using System.Text.Json.Nodes;
using JSONSharp.PrettyPrint;
using JSONSharp.types;

namespace JSONTest;

public class UnitTest1
{
	[Fact]
	public void Test1()
	{
		JSONObject root = new JSONObject();

		JSONValue abc = new JSONNumber(69);
		JSONValue vcs = new JSONString("123fva");
		JSONArray we = new JSONArray();
		we.Values.Add(new JSONNumber(122.2));
		we.Values.Add(new JSONString("asdv"));
		we.Values.Add(new JSONNumber(12.22));

		JSONObject nest = new JSONObject();


		nest["des"] = new JSONNumber(1);
		nest["es"] = new JSONNumber(2);
		nest["boolt"] = new JSONBool(true);
		nest["boolf"] = new JSONBool(false);

		root["abc"] = abc;
		root["vcs"] = vcs;
		root["we"] = we;

		root["nest"] = nest;

		PrettyPrinter printer = new();
		string json = printer.Stringifiy(root);
		Console.WriteLine(json);

		string expected = """
{
    "abc": 69,
    "vcs": "123fva",
    "we": [
        122.2,
        "asdv",
        12.22
    ],
    "nest": {
        "des": 1,
        "es": 2,
        "boolt": true,
        "boolf": false
    }
}
""";
		expected = expected.Replace("\r\n", "\n");

		if (expected != json)
			throw new Exception();

		if ((JSONNumber)root["nest:des"] != 1)
		{
            throw new Exception();
        }

        if ((JSONNumber)root["we:0"] != 122.2)
        {
            throw new Exception();
        }

        if ((JSONString)root["we:1"] != "asdv")
        {
            throw new Exception();
        }
    }

	[Fact]
	public void Test2()
	{
		JSONObject obj = new JSONObject();

		obj["apple"] = new JSONNumber(69);
		obj["orange"] = new JSONBool(true);

		var printer = new PrettyPrinter();
		string jsonString = printer.Stringifiy(obj);

		string expected = """
{
    "apple": 69,
    "orange": true
}
""";
		expected = expected.Replace("\r\n", "\n");

		System.Console.WriteLine(jsonString);
		if (jsonString != expected)
			throw new Exception();
	}
}
