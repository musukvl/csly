// See https://aka.ms/new-console-template for more information

using AryPlayground;
using AryPlayground.JsonModel;
using sly.parser;
using sly.parser.generator;

var jsonParser = new JSONParser();
var builder = new ParserBuilder<JsonToken, JSon>();
var parser = builder.BuildParser(jsonParser, ParserType.LL_RECURSIVE_DESCENT, "root").Result;


var source = @"{
    'one': 1,
    'str': 'hello'
}".Replace("'", "\"");
var r = parser.Parse(source);

var isError = r.IsError; // true
var root = r.Result; // null;
var errors = r.Errors; // !null & count > 0
var error = errors[0] as UnexpectedTokenSyntaxError<JsonToken>; // 
var token = error.UnexpectedToken.TokenID; // comma
var line = error.Line; // 3
var column = error.Column; // 12