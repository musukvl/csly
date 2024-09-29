using sly.lexer;

namespace Amba.TfvarsParser;

[Lexer(IgnoreEOL = true)]
public enum TfvarsToken
{
    EOF = 0,
    
    /*
    [Lexeme(GenericToken.SugarToken, "\n",IsLineEnding = true)]
    [Lexeme(GenericToken.SugarToken, "\r\n",IsLineEnding = true)]
    [Lexeme(GenericToken.SugarToken, "\r",IsLineEnding = true)]
    EOL,
    */
    [Lexeme(GenericToken.Identifier, IdentifierType.Custom, "_A-Za-z", "_A-Za-z0-9-")] IDENTIFIER,
    [Lexeme(GenericToken.String)] STRING,
    [Lexeme(GenericToken.Int)] INT,
    [Lexeme(GenericToken.Double)] DECIMAL,
    [Lexeme(GenericToken.KeyWord, "true", "false")] BOOLEAN,
    [Lexeme(GenericToken.SugarToken, "=")] EQ,
    
    [Lexeme(GenericToken.SugarToken, "[")] LBRACKET,
    [Lexeme(GenericToken.SugarToken, "]")] RBRACKET,
    [Lexeme(GenericToken.SugarToken, "{")] LBRACE,
    [Lexeme(GenericToken.SugarToken, "}")] RBRACE,
    [Lexeme(GenericToken.SugarToken, ",")] COMMA,
    [Lexeme(GenericToken.KeyWord, "null")] NULL 
}