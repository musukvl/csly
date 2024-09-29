using System.Globalization;
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

    [Production("list: LBRACKET[d] RBRACKET[d]")]
    public JSon EmptyList()
    {
        return new JList();
    }

    [Production("list: LBRACKET[d] listElements RBRACKET[d]")]
    public JSon List(JList elements)
    {
        return elements;
    }

    [Production("listElements: value COMMA[d] listElements")]
    public JSon ListElementsMany(JSon value, JList tail)
    {
        var elements = new JList(value);
        elements.AddRange(tail);
        return elements;
    }

    [Production("listElements: value COMMA*")]
    public JSon ListElementsOneWithComma(JSon element, List<Token<TfvarsToken>> comma)
    {
        return new JList(element);
    }
    #endregion
    
    #region OBJECT

    [Production("object: LBRACE[d] RBRACE[d]")]
    public JSon EmptyObjectValue()
    {
        return new JObject();
    }

    [Production("object: LBRACE[d] members RBRACE[d]")]
    public JSon AttributesObjectValue(JObject members)
    {
        return members;
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
    [Production("members : property COMMA* members")]
    public JSon ManyMembers1(JObject pair, List<Token<TfvarsToken>> comm,  JObject tail)
    {
        var members = new JObject();
        members.Merge(pair);
        members.Merge(tail);
        return members;
    }
    
    [Production("members : property COMMA*")]
    public JSon SingleMember(JObject pair, List<Token<TfvarsToken>> comma)
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
    
    [Production("value : object")]
    public JSon ObjectValue(JSon value)
    {
        return value;
    }
    
    [Production("value : BOOLEAN")]
    public JSon BooleanValue(Token<TfvarsToken> boolToken)
    {
        return new JValue(bool.Parse(boolToken.Value));
    }

    [Production("value : NULL")]
    public JSon NullValue(Token<TfvarsToken> forget)
    {
        return new JNull();
    }
    
    [Production("value : DECIMAL")]
    public JSon DoubleValue(Token<TfvarsToken> decimalToken)
    {
        if (Decimal.TryParse(decimalToken.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out var value))
        {
            return new JValue(value);
        }
        
        return new JValue(0);
    }
    #endregion
}