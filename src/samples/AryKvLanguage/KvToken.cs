using sly.lexer;

namespace AryKvLanguage;

[Lexer(IgnoreEOL = false)]
public enum KvToken
{
    [Lexeme(GenericToken.SugarToken, "\n",IsLineEnding = true)]
    [Lexeme(GenericToken.SugarToken, "\r\n",IsLineEnding = true)]
    [Lexeme(GenericToken.SugarToken, "\r",IsLineEnding = true)]
    EOL = 1,
    
    [Lexeme(GenericToken.Identifier)] IDENTIFIER = 2,
    [Lexeme(GenericToken.String)] STRING = 3,
    [Lexeme(GenericToken.Int)] INT = 4,
    [Lexeme(GenericToken.Double)] DOUBLE = 5,
    
    [Lexeme(GenericToken.SugarToken, "=")] EQUALS = 6,
}