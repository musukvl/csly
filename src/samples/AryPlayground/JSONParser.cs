﻿using AryPlayground.JsonModel;
using sly.lexer;
using sly.parser.generator;

namespace AryPlayground
{
    public static class DictionaryExtensionMethods
    {
        public static void Merge<TKey, TValue>(this Dictionary<TKey, TValue> me, Dictionary<TKey, TValue> merge)
        {
            foreach (var item in merge) me[item.Key] = item.Value;
        }
    }

    public class JSONParser
    {
        #region root

        [Production("root : value")]
        public JSon Root(JSon value)
        {
            return value;
        }

        #endregion

        #region VALUE

        [Production("value : STRING")]
        public JSon StringValue(Token<JsonToken> stringToken)
        {
            return new JValue(stringToken.StringWithoutQuotes);
        }

        [Production("value : INT")]
        public JSon IntValue(Token<JsonToken> intToken)
        {
            return new JValue(intToken.IntValue);
        }

        [Production("value : DOUBLE")]
        public JSon DoubleValue(Token<JsonToken> doubleToken)
        {
            var dbl = double.MinValue;
            try
            {
                var doubleParts = doubleToken.Value.Split('.');
                dbl = double.Parse(doubleParts[0]);
                if (doubleParts.Length > 1)
                {
                    var decimalPart = double.Parse(doubleParts[1]);
                    for (var i = 0; i < doubleParts[1].Length; i++) decimalPart = decimalPart / 10.0;
                    dbl += decimalPart;
                }
            }
            catch
            {
                dbl = double.MinValue;
            }

            return new JValue(dbl);
        }

        [Production("value : BOOLEAN")]
        public JSon BooleanValue(Token<JsonToken> boolToken)
        {
            return new JValue(bool.Parse(boolToken.Value));
        }

        [Production("value : NULL")]
        public JSon NullValue(Token<JsonToken> forget)
        {
            return new JNull();
        }

        [Production("value : object")]
        public JSon ObjectValue(JSon value)
        {
            return value;
        }

        [Production("value: list")]
        public JSon ListValue(JList list)
        {
            return list;
        }

        #endregion

        #region OBJECT

        [Production("object: ACCG ACCD")]
        public JSon EmptyObjectValue(Token<JsonToken> accg, Token<JsonToken> accd)
        {
            return new JObject();
        }

        [Production("object: ACCG members ACCD")]
        public JSon AttributesObjectValue(Token<JsonToken> accg, JObject members, Token<JsonToken> accd)
        {
            return members;
        }

        #endregion

        #region LIST

        [Production("list: CROG CROD")]
        public JSon EmptyList(Token<JsonToken> crog, Token<JsonToken> crod)
        {
            return new JList();
        }

        [Production("list: CROG listElements CROD")]
        public JSon List(Token<JsonToken> crog, JList elements, Token<JsonToken> crod)
        {
            return elements;
        }


        [Production("listElements: value COMMA listElements")]
        public JSon ListElementsMany(JSon value, Token<JsonToken> comma, JList tail)
        {
            var elements = new JList(value);
            elements.AddRange(tail);
            return elements;
        }

        [Production("listElements: value COMMA")]
        public JSon ListElementsOneWithComma(JSon element, Token<JsonToken> comma)
        {
            return new JList(element);
        }
        
        [Production("listElements: value")]
        public JSon ListElementsOne(JSon element)
        {
            return new JList(element);
        }

        #endregion

        #region PROPERTIES

        [Production("property: STRING COLON value")]
        public JSon property(Token<JsonToken> key, Token<JsonToken> colon, JSon value)
        {
            return new JObject(key.StringWithoutQuotes, value);
        }


        [Production("members : property COMMA members")]
        public JSon ManyMembers(JObject pair, Token<JsonToken> comma, JObject tail)
        {
            var members = new JObject();
            members.Merge(pair);
            members.Merge(tail);
            return members;
        }

        [Production("members : property COMMA [d]")]
        public JSon SingleMemberWithComma(JObject pair)
        {
            return SingleMember(pair);
        }
        
        [Production("members : property")]
        public JSon SingleMember(JObject pair)
        {
            var members = new JObject();
            members.Merge(pair);
            return members;
        }

        #endregion
    }
}