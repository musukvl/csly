// See https://aka.ms/new-console-template for more information

using AryPlayground;
using AryPlayground.JsonModel;
using Newtonsoft.Json;
using sly.parser;
using sly.parser.generator;
using JsonToken = AryPlayground.JsonToken;

var jsonParser = new JSONParser();
var builder = new ParserBuilder<JsonToken, JSon>();
var parser = builder.BuildParser(jsonParser, ParserType.LL_RECURSIVE_DESCENT, "root").Result;


var source = @"{
    'one': 1,
    'str': 'hello',
    'list': [1, 2, 3,],
    'x': 1,
}".Replace("'", "\"");
var r = parser.Parse(source);

var isError = r.IsError; // true
var root = r.Result; // null;

Console.WriteLine("IsError: " + isError);
Console.WriteLine("Root: " + JsonConvert.SerializeObject(r, Formatting.Indented));