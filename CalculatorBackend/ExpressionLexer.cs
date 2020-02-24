using System;
using System.IO;
using System.Text;
using System.Threading;

namespace CalculatorBackend
{
    /// <summary>
    /// Реализует лексический анализатор по грамматике:
    /// 
    ///     S -> NUM S | {space}* S | [+-/*] S | ( S | ) S
    ///     NUM -> [0-9]* ((.|,)[0-9]+)?
    /// </summary>
    public class ExpressionLexer : LexerBase
    {
        public ExpressionLexer(StringReader sr) : base(sr)
        {
        }

        /// <summary>
        /// Возвращает очередной токен
        /// </summary>
        /// <returns></returns>
        public override Token NextToken() => S();

        /// <summary>
        /// Начальный нетерминал грамматики токенов
        /// </summary>
        /// <returns></returns>
        private Token S()
        {
            // Если конец потока - то возвращаем EOF
            if (Eof)
                return new Token(TokenType.Eof);

            while (char.IsWhiteSpace(Peek)) Match();

            // Проверяем односимвольные терминалы
            switch (Peek)
            {
                case '+': Match(); return new Token(TokenType.Plus) { Position = CurrentPosition };
                case '-': Match(); return new Token(TokenType.Minus) { Position = CurrentPosition };
                case '*': Match(); return new Token(TokenType.Mul) { Position = CurrentPosition };
                case '/': Match(); return new Token(TokenType.Div) { Position = CurrentPosition };
                case '(': Match(); return new Token(TokenType.ParenLeft) { Position = CurrentPosition };
                case ')': Match(); return new Token(TokenType.ParenRight) { Position = CurrentPosition };
            }

            // Осталось только число
            if (char.IsDigit(Peek))
                return ReadNumber();

            // Неожиданный символ
            throw new FormatException($"Unexpected char `{Peek}` at {CurrentPosition}");
        }

        /// <summary>
        /// Считывает число с потока, соответствует NUM
        /// </summary>
        /// <returns></returns>
        private NumToken ReadNumber()
        {
            StringBuilder sb = new StringBuilder();
            int position = CurrentPosition;

            while (char.IsDigit(Peek))
            {
                sb.Append(Peek);
                Match();
            }

            if (Peek == '.' || Peek == ',')
            {
                sb.Append(
                    Convert.ToChar(
                        Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator
                        ));
                Match();
                if (!char.IsDigit(Peek))
                    throw new FormatException($"Expected to find a digit at {CurrentPosition}, got `{Peek}`");

                while (char.IsDigit(Peek))
                {
                    sb.Append(Peek);
                    Match();
                }
            }

            return new NumToken(sb.ToString()) { Position = position };
        }
    }
}