using AryKvLanguage.Model;
using sly.lexer;
using sly.parser.generator;

namespace AryKvLanguage;

public class KvParser
{
    [Production("root : members")]
    public JSon Root(JSon value)
    {
        return value;
    }
    
    [Production("property: IDENTIFIER EQUALS value")]
    public JSon PropertyIdentifier(Token<KvToken> key, Token<KvToken> colon, JSon value)
    {
        return new JObject(key.StringWithoutQuotes, value);
    }

    [Production("property: STRING EQUALS value")]
    public JSon PropertyString(Token<KvToken> key, Token<KvToken> colon, JSon value)
    {
        return new JObject(key.StringWithoutQuotes, value);
    }

    [Production("members : property EOL members")]
    public JSon ManyMembers(JObject pair, Token<KvToken> comma, JObject tail)
    {
        var members = new JObject();
        members.Merge(pair);
        members.Merge(tail);
        return members;
    }
     
    
    [Production("members : property EOL")]
    public JSon SingleMember(JObject pair, Token<KvToken> comma)
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
    public JSon StringValue(Token<KvToken> stringToken)
    {
        return new JValue(stringToken.StringWithoutQuotes);
    }

    [Production("value : INT")]
    public JSon IntValue(Token<KvToken> intToken)
    {
        return new JValue(intToken.IntValue);
    }
}