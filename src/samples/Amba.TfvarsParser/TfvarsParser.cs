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

    #region LIST

    [Production("list: LBRACKET RBRACKET")]
    public JSon EmptyList(Token<TfvarsToken> crog, Token<TfvarsToken> crod)
    {
        return new JList();
    }

    [Production("list: LBRACKET listElements RBRACKET")]
    public JSon List(Token<TfvarsToken> crog, JList elements, Token<TfvarsToken> crod)
    {
        return elements;
    }


    [Production("listElements: value COMMA listElements")]
    public JSon ListElementsMany(JSon value, Token<TfvarsToken> comma, JList tail)
    {
        var elements = new JList(value);
        elements.AddRange(tail);
        return elements;
    }

    [Production("listElements: value COMMA")]
    public JSon ListElementsOneWithComma(JSon element, Token<TfvarsToken> comma)
    {
        return new JList(element);
    }
        
    [Production("listElements: value")]
    public JSon ListElementsOne(JSon element)
    {
        return new JList(element);
    }

    #endregion
    
    
    #region property
    
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

    #endregion
 
    #region Members
    
    [Production("members : property members")]
    public JSon ManyMembers(JObject pair,  JObject tail)
    {
        var members = new JObject();
        members.Merge(pair);
        members.Merge(tail);
        return members;
    }
     
    
    [Production("members : property ")]
    public JSon SingleMember(JObject pair)
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
    #endregion
    
    #region VALUE
    [Production("value: list")]
    public JSon ListValue(JList list)
    {
        return list;
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
    #endregion
}