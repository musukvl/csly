using Amba.TfvarsParser.Model;
using sly.lexer;
using sly.parser.generator;

namespace Amba.TfvarsParser;

public class TfvarsParser
{
    [Production("root : members")]
    public JSon Root(JSon value)
    {
        return value;
    }
    
    [Production("property: IDENTIFIER EQ value")]
    public JSon PropertyIdentifier(Token<TfvarsToken> key, Token<TfvarsToken> colon, JSon value)
    {
        return new JObject(key.StringWithoutQuotes, value);
    }

    [Production("property: STRING EQ value")]
    public JSon PropertyString(Token<TfvarsToken> key, Token<TfvarsToken> colon, JSon value)
    {
        return new JObject(key.StringWithoutQuotes, value);
    }

    [Production("members : property EOL members")]
    public JSon ManyMembers(JObject pair, Token<TfvarsToken> comma, JObject tail)
    {
        var members = new JObject();
        members.Merge(pair);
        members.Merge(tail);
        return members;
    }
     
    
    [Production("members : property EOL")]
    public JSon SingleMember(JObject pair, Token<TfvarsToken> comma)
    {
        var members = new JObject();
        members.Merge(pair);
        return members;
    }
    
    [Production("members : property")]
    public JSon SingleMember1(JObject pair)
    {
        var members = new JObject();
        members.Merge(pair);
        return members;
    }

    
    [Production("value : STRING")]
    public JSon StringValue(Token<TfvarsToken> stringToken)
    {
        return new JValue(stringToken.StringWithoutQuotes);
    }

    [Production("value : INT")]
    public JSon IntValue(Token<TfvarsToken> intToken)
    {
        return new JValue(intToken.IntValue);
    }
}