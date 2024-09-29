using Amba.TfvarsParser;
using Amba.TfvarsParser.Model;
using Newtonsoft.Json;
using sly.lexer;
using sly.parser.generator;

var kvParser = new TfvarsParser();
var builder = new ParserBuilder<TfvarsToken, JSon>();

// try lexer
var lexbuild = LexerBuilder.BuildLexer<TfvarsToken>();
if (lexbuild.IsError)
{
    foreach (var error in lexbuild.Errors)
    {
        Console.WriteLine(error.Message);
    }
    return;
}

var test1 = File.ReadAllText("test1.tfvars");

var lexer = lexbuild.Result;
var result = lexer.Tokenize(test1);
if (result.IsError)
{
    Console.WriteLine(result.Error);
    return;
}

foreach (var token in result.Tokens)
{
    Console.Write(token.TokenID);
    Console.Write(" ");
}
Console.WriteLine();
 

Console.WriteLine("=========================== parser ======================");


// build the parser
var parserBuildResult = builder.BuildParser(kvParser, ParserType.EBNF_LL_RECURSIVE_DESCENT, "root");

if (parserBuildResult.IsError)
{
    foreach (var error in parserBuildResult.Errors)
    {
        Console.WriteLine(error.Message);
    }
    return;
}

// try use parser
var parser = parserBuildResult.Result; 
 

var r = parser.Parse(test1);
var isError = r.IsError; // true
var root = r.Result; // null;

Console.WriteLine("IsError: " + isError);
Console.WriteLine("Root: " + JsonConvert.SerializeObject(r, Formatting.Indented));