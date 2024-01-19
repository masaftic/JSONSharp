

using JSONSharp.PrettyPrint;
using JSONSharp.types;

namespace JSONTest
{
	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			JSONObject root = new JSONObject();

			JSONValue abc = new JSONNumber(69);
			JSONValue vcs = new JSONString("123fva");
			JSONValue we = new JSONArray();
			((JSONArray)we).Values.Add(new JSONNumber(122.2));
			((JSONArray)we).Values.Add(new JSONString("asdv"));
			((JSONArray)we).Values.Add(new JSONNumber(12.22));

			JSONObject nest = new JSONObject();


			nest["des"] = new JSONNumber(1);
			nest["es"] = new  JSONNumber(2);
			nest["boolt"] = new JSONBool(true);
			nest["boolf"] = new JSONBool(false);

			root["abc"] = abc;
			root["vcs"] = vcs;
			root["we"] = we;

			root["nest"] = nest;

			PrettyPrint printer = new();
			string json = printer.Stringifiy(root);
			Console.WriteLine(json);

			string t = """
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
			t = t.Replace("\r\n", "\n");

			if (t != json)
				throw new Exception();
		}
	}
}