using System;
using System.Collections.Generic;
using sly.lexer;
using sly.parser.syntax.grammar;
using sly.parser.syntax.tree;

namespace sly.parser.llparser;

public partial class EBNFRecursiveDescentSyntaxParser<IN, OUT>
{
    #region parsing

    public SyntaxParseResult<IN> ParseZeroOrMore(IList<Token<IN>> tokens, ZeroOrMoreClause<IN> clause, int position,
        SyntaxParsingContext<IN> parsingContext)
    {
        if (parsingContext.TryGetParseResult(clause, position, out var parseResult))
        {
            return parseResult;
        }

        var result = new SyntaxParseResult<IN>();
        var manyNode = new ManySyntaxNode<IN>($"{clause.Clause.ToString()}*");
        var currentPosition = position;
        var innerClause = clause.Clause;
        var stillOk = true;


        SyntaxParseResult<IN> lastInnerResult = null;

        var innerErrors = new List<UnexpectedTokenSyntaxError<IN>>();

        bool hasByPasNodes = false;
        while (stillOk)
        {
            SyntaxParseResult<IN> innerResult = null;
            switch (innerClause)
            {
                case TerminalClause<IN> term:
                    manyNode.IsManyTokens = true;
                    innerResult = ParseTerminal(tokens, term, currentPosition, parsingContext);
                    hasByPasNodes = hasByPasNodes || innerResult.HasByPassNodes;
                    break;
                case NonTerminalClause<IN> nonTerm:
                {
                    innerResult = ParseNonTerminal(tokens, nonTerm, currentPosition, parsingContext);
                    hasByPasNodes = hasByPasNodes || innerResult.HasByPassNodes;
                    if (nonTerm.IsGroup)
                        manyNode.IsManyGroups = true;
                    else
                        manyNode.IsManyValues = true;
                    break;
                }
                case GroupClause<IN> _:
                    manyNode.IsManyGroups = true;
                    innerResult = ParseNonTerminal(tokens, innerClause as NonTerminalClause<IN>, currentPosition,
                        parsingContext);
                    hasByPasNodes = hasByPasNodes || innerResult.HasByPassNodes;
                    break;
                case ChoiceClause<IN> choice:
                    manyNode.IsManyTokens = choice.IsTerminalChoice;
                    manyNode.IsManyValues = choice.IsNonTerminalChoice;
                    innerResult = ParseChoice(tokens, choice, currentPosition, parsingContext);
                    hasByPasNodes = hasByPasNodes || innerResult.HasByPassNodes;
                    break;
                default:
                    throw new InvalidOperationException("unable to apply repeater to " + innerClause.GetType().Name);
            }

            if (innerResult != null && !innerResult.IsError)
            {
                manyNode.Add(innerResult.Root);
                currentPosition = innerResult.EndingPosition;
                lastInnerResult = innerResult;
                hasByPasNodes = hasByPasNodes || innerResult.HasByPassNodes;
                innerErrors.AddRange(lastInnerResult.Errors);
            }
            else
            {
                if (innerResult != null)
                {
                    innerErrors.AddRange(innerResult.Errors);
                }
            }

            stillOk = innerResult != null && !innerResult.IsError && currentPosition < tokens.Count;
        }


        result.EndingPosition = currentPosition;
        result.IsError = false;
        result.Errors = innerErrors;
        result.Root = manyNode;
        result.IsEnded = lastInnerResult != null && lastInnerResult.IsEnded;
        result.HasByPassNodes = hasByPasNodes;
        parsingContext.Memoize(clause, position, result);
        return result;
    }

    public SyntaxParseResult<IN> ParseOneOrMore(IList<Token<IN>> tokens, OneOrMoreClause<IN> clause, int position,
        SyntaxParsingContext<IN> parsingContext)
    {
        if (parsingContext.TryGetParseResult(clause, position, out var parseResult))
        {
            return parseResult;
        }

        var result = new SyntaxParseResult<IN>();
        var manyNode = new ManySyntaxNode<IN>($"{clause.Clause.ToString()}+");
        var currentPosition = position;
        var innerClause = clause.Clause;
        bool isError;

        SyntaxParseResult<IN> lastInnerResult = null;

        bool hasByPasNodes = false;
        SyntaxParseResult<IN> firstInnerResult = null;
        var innerErrors = new List<UnexpectedTokenSyntaxError<IN>>();

        switch (innerClause)
        {
            case TerminalClause<IN> terminalClause:
                manyNode.IsManyTokens = true;
                firstInnerResult = ParseTerminal(tokens, terminalClause, currentPosition, parsingContext);
                hasByPasNodes = hasByPasNodes || firstInnerResult.HasByPassNodes;
                break;
            case NonTerminalClause<IN> nonTerm:
            {
                firstInnerResult = ParseNonTerminal(tokens, nonTerm, currentPosition, parsingContext);
                hasByPasNodes = hasByPasNodes || firstInnerResult.HasByPassNodes;
                if (nonTerm.IsGroup)
                    manyNode.IsManyGroups = true;
                else
                    manyNode.IsManyValues = true;
                break;
            }
            case ChoiceClause<IN> choice:
                manyNode.IsManyTokens = choice.IsTerminalChoice;
                manyNode.IsManyValues = choice.IsNonTerminalChoice;
                firstInnerResult = ParseChoice(tokens, choice, currentPosition, parsingContext);
                hasByPasNodes = hasByPasNodes || firstInnerResult.HasByPassNodes;
                break;
            default:
                throw new InvalidOperationException("unable to apply repeater to " + innerClause.GetType().Name);
        }

        if (firstInnerResult != null && !firstInnerResult.IsError)
        {
            manyNode.Add(firstInnerResult.Root);
            lastInnerResult = firstInnerResult;
            currentPosition = firstInnerResult.EndingPosition;
            var more = new ZeroOrMoreClause<IN>(innerClause);
            var nextResult = ParseZeroOrMore(tokens, more, currentPosition, parsingContext);
            if (nextResult != null && !nextResult.IsError)
            {
                currentPosition = nextResult.EndingPosition;
                var moreChildren = (ManySyntaxNode<IN>)nextResult.Root;
                manyNode.Children.AddRange(moreChildren.Children);
            }

            if (nextResult != null)
            {
                innerErrors.AddRange(nextResult.Errors);
            }

            isError = false;
        }
        else
        {
            if (firstInnerResult != null)
            {
                innerErrors.AddRange(firstInnerResult.Errors);
            }

            isError = true;
        }

        result.EndingPosition = currentPosition;
        result.IsError = isError;
        result.Errors = innerErrors;
        result.Root = manyNode;
        result.IsEnded = lastInnerResult != null && lastInnerResult.IsEnded;
        result.HasByPassNodes = hasByPasNodes;
        parsingContext.Memoize(clause, position, result);
        return result;
    }

    #endregion
}