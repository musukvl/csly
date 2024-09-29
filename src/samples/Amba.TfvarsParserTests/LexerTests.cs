using Amba.TfvarsParser;
using sly.lexer;

namespace Amba.TfvarsParserTests;

public class LexerTests
{
    private static ILexer<TfvarsToken> BuildLexer()
    {
        // try lexer
        var lexbuild = LexerBuilder.BuildLexer<TfvarsToken>();
        if (lexbuild.IsError)
        {
            var errorMessages = string.Join(Environment.NewLine, lexbuild.Errors.Select(e => e.Message));

            throw new Exception(errorMessages);
        }
        return lexbuild.Result;
    }
    
    private static string GetTokensString(IEnumerable<Token<TfvarsToken>> tokens)
    {
        return string.Join(" ", tokens.Select(t => t.TokenID));
    }
    
    [Fact]
    public void PropertyStringTest()
    {
        var lexer = BuildLexer();
        var result = lexer.Tokenize("key = \"value\"");
        Assert.False(result.IsError);
        Assert.Equal("IDENTIFIER EQ STRING EOF", GetTokensString(result.Tokens));
    }
    
    [Fact]
    public void PropertyIntTest()
    {
        var lexer = BuildLexer();
        var result = lexer.Tokenize("key = 42");
        Assert.False(result.IsError);
        Assert.Equal("IDENTIFIER EQ INT EOF", GetTokensString(result.Tokens));
    }

    [Fact]
    public void ManyMembersTest()
    {
        var lexer = BuildLexer();
        var result = lexer.Tokenize(@"key_1 = 42
            key2 = ""value""");
        Assert.False(result.IsError);
        Assert.Equal("IDENTIFIER EQ INT IDENTIFIER EQ STRING EOF", GetTokensString(result.Tokens));
    }
    
    [Fact]
    public void StringIdentifierTest()
    {
        var lexer = BuildLexer();
        var result = lexer.Tokenize(@"""key"" = 42");
        Assert.False(result.IsError);
        Assert.Equal("STRING EQ INT EOF", GetTokensString(result.Tokens));
    }
    
    [Fact]
    public void ArraySingleElement()
    {
        var lexer = BuildLexer();
        var result = lexer.Tokenize(@"arr = [1]");
        Assert.False(result.IsError, result.Error?.ToString());
        Assert.Equal("IDENTIFIER EQ LBRACKET INT RBRACKET EOF", GetTokensString(result.Tokens));
    }
    
    [Fact]
    public void ArrayTest()
    {
        var lexer = BuildLexer();
        var result = lexer.Tokenize(@"arr = [1, 2]");
        Assert.False(result.IsError, result.Error?.ToString());
        Assert.Equal("IDENTIFIER EQ LBRACKET INT COMMA INT RBRACKET EOF", GetTokensString(result.Tokens));
    }
    
}