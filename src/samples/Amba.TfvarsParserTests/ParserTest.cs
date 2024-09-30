using Amba.TfvarsParser;
using Amba.TfvarsParser.Model;
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
    public void SimpleCase()
    {
        var parser = BuildParser();
        var source = "a = 1";
        
        var result = parser.Parse(source);
        Assert.False(result.IsError, result?.Errors?.Aggregate("", (acc, x) => acc + x.ErrorMessage + Environment.NewLine));
        var json = ParseUtils.Traverse(result.Result);
        Assert.Equal("{\n    a = 1,\n}", json.ToString());
    }
    
    [Theory]
    [InlineData("./data/common.tfvars", "./data/common.result")]
    [InlineData("./data/inner_object.tfvars", "./data/inner_object.result")]
    [InlineData("./data/simple_values.tfvars", "./data/simple_values.result")]
    public void FileTest(string inputFile, string outputFile)
    {
        var parser = BuildParser();
        var source = File.ReadAllText(inputFile);
        var expected = File.ReadAllText(outputFile).Replace("\r\n", "\n");
        
        var result = parser.Parse(source);
        Assert.False(result.IsError, result?.Errors?.Aggregate("", (acc, x) => acc + x.ErrorMessage + Environment.NewLine));
        var json = ParseUtils.Traverse(result.Result);
        Assert.Equal(expected, json.ToString());
    }
}