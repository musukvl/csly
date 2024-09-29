using Amba.TfvarsParser;
using Amba.TfvarsParser.Model;
using sly.lexer;
using sly.parser;
using sly.parser.generator;

public class ParserTests
{
    private static Parser<TfvarsToken, JSon> BuildParser()
    {
        var kvParser = new TfvarsParser();
        var builder = new ParserBuilder<TfvarsToken, JSon>();
        // build the parser
        var parserBuildResult = builder.BuildParser(kvParser, ParserType.EBNF_LL_RECURSIVE_DESCENT, "root");

        if (parserBuildResult.IsError)
        {
            throw new Exception(string.Join(Environment.NewLine, parserBuildResult.Errors.Select(x => x.Message)));
        }

        return parserBuildResult.Result;
    }
    
    [Fact]
    public void Test1()
    {
        
        var parser = BuildParser();
        var source = "a = 1";
        
        var result = parser.Parse(source);
        Assert.False(result.IsError, result?.Errors?.Aggregate("", (acc, x) => acc + x.ErrorMessage + Environment.NewLine));
        var json = ParseUtils.Traverse(result.Result);
        Assert.Equal("{\n    a = 1,\n}", json.ToString());
    }
    
    [Fact]
    public void Common()
    {
        
        var parser = BuildParser();
        var source = File.ReadAllText("./data/common.tfvars");
        var expected = File.ReadAllText("./data/common.result").Replace("\r\n", "\n");
        
        var result = parser.Parse(source);
        Assert.False(result.IsError, result?.Errors?.Aggregate("", (acc, x) => acc + x.ErrorMessage + Environment.NewLine));
        var json = ParseUtils.Traverse(result.Result);
        Assert.Equal(expected, json.ToString());
    }
}