using System;
using System.Linq.Expressions;

namespace CalculatorBackend
{
    /// <summary>
    /// Реализует транслятор арифметических выражений в дерево выражений Linq
    /// Используемая грамматика:
    ///     S -> S + T | S - T | T
    ///     T -> T * P | T / P | P
    ///     P -> (S) | NUM | -P
    /// </summary>
    public class ExpressionCompiler
    {
        /*
         * S -> S + T | S - T | T
         * ***
         * S -> S Q | T
         * Q -> + T | - T
         * ***
         * S -> T S' | T
         * S' -> Q S' | Q
         */

        private readonly ILexer _lexer;

        private Token CurrentToken { get; set; }

        public ExpressionCompiler(ILexer lexer)
        {
            _lexer = lexer;
            CurrentToken = _lexer.NextToken();
        }

        private void ReadToken() => CurrentToken = _lexer.NextToken();

        private void CheckToken(TokenType type)
        {
            if (CurrentToken.Type == type) ReadToken();
            else
            {
                throw new FormatException($"Unexpected {CurrentToken.Type} at {CurrentToken.Position}");
            }
        }

        public Expression<Func<double>> GetExpression()
        {
            var expression = Expression.Lambda<Func<double>>(S());

            CheckToken(TokenType.Eof);

            return expression;
        }

        private Expression S()
        {
            var tExpr = T();
            switch (CurrentToken.Type)
            {
                case TokenType.Minus:
                    while (CurrentToken.Type == TokenType.Minus)
                    {
                        ReadToken();
                        var tExprRight = T();
                        tExpr = Expression.Subtract(tExpr, tExprRight);
                    }

                    if (CurrentToken.Type == TokenType.Plus)
                    {
                        ReadToken();
                        return Expression.Add(tExpr, S());
                    }
                    break;

                case TokenType.Plus:
                    ReadToken();
                    return Expression.Add(tExpr, S());
            }

            return tExpr;
        }

        private Expression T()
        {
            var tExpr = P();
            switch (CurrentToken.Type)
            {
                case TokenType.Div:
                    while (CurrentToken.Type == TokenType.Div)
                    {
                        ReadToken();
                        var tExprRight = P();
                        tExpr = Expression.Divide(tExpr, tExprRight);
                    }

                    if (CurrentToken.Type == TokenType.Mul)
                    {
                        ReadToken();
                        return Expression.Multiply(tExpr, S());
                    }
                    break;

                case TokenType.Mul:
                    ReadToken();
                    return Expression.Multiply(tExpr, S());
            }

            return tExpr;
        }

        private Expression P()
        {
            Expression expression;
            switch (CurrentToken.Type)
            {
                case TokenType.Minus:
                    ReadToken();
                    return Expression.Negate(P());
                case TokenType.ParenLeft:
                    ReadToken();
                    expression = S();
                    CheckToken(TokenType.ParenRight);
                    return expression;
                case TokenType.Number:
                    expression = Expression.Constant(
                        double.Parse((CurrentToken as NumToken)?.Literal
                                     ?? throw new FormatException("Invalid number format")));
                    ReadToken();
                    return expression;
            }

            throw new FormatException($"Unexpected token {CurrentToken.Type} at {CurrentToken.Position}");
        }
    }
}