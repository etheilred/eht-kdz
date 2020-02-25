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
        /// <summary>
        /// Входной поток токенов
        /// </summary>
        private readonly ILexer _lexer;

        /// <summary>
        /// Содержит токен, который дожен быть обработан в данный момент
        /// </summary>
        private Token CurrentToken { get; set; }

        public ExpressionCompiler(ILexer lexer)
        {
            _lexer = lexer;
            CurrentToken = _lexer.NextToken();
        }

        /// <summary>
        /// Считывает очередной токен
        /// </summary>
        private void ReadToken() => CurrentToken = _lexer.NextToken();

        /// <summary>
        /// Если тип текущего токена совпадает с переданным типом, то считывает очередной токен
        /// Иначе бросает исключение
        /// </summary>
        /// <param name="type"></param>
        private void CheckToken(TokenType type)
        {
            if (CurrentToken.Type == type)
                ReadToken();
            else
                throw new FormatException($"Unexpected {CurrentToken.Type} at {CurrentToken.Position}");
        }

        /// <summary>
        /// Возвращает дерево выражений для лямбда функции, которая вычисляет выражение
        /// </summary>
        /// <returns></returns>
        public Expression<Func<double>> GetExpression()
        {
            var expression = Expression.Lambda<Func<double>>(S());

            CheckToken(TokenType.Eof);

            return expression;
        }

        /// <summary>
        /// Методы S, T, P соответствуют нетерминальным символам грамматики
        /// </summary>
        /// <returns></returns>
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
                case TokenType.Modulo:
                case TokenType.Div:
                    while (CurrentToken.Type == TokenType.Div || CurrentToken.Type == TokenType.Modulo)
                    {
                        TokenType cType = CurrentToken.Type;
                        ReadToken();
                        var tExprRight = P();
                        tExpr = cType == TokenType.Div ? 
                            Expression.Divide(tExpr, tExprRight) :
                            Expression.Modulo(tExpr, tExprRight);
                    }

                    if (CurrentToken.Type == TokenType.Mul)
                    {
                        ReadToken();
                        return Expression.Multiply(tExpr, T());
                    }
                    break;

                case TokenType.Mul:
                    ReadToken();
                    return Expression.Multiply(tExpr, T());
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