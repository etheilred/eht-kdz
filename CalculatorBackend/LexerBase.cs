using System.IO;

namespace CalculatorBackend
{
    /// <summary>
    /// Содержит базовый функционал для реализации лексического анализатора
    /// </summary>
    public abstract class LexerBase : ILexer
    {
        private readonly StringReader _reader;
        private int _currentPosition = 1;

        protected int CurrentPosition => _currentPosition;

        public bool Eof => _reader.Peek() == -1;

        public LexerBase(StringReader sr) => _reader = sr;

        protected char Peek => (char)_reader.Peek();

        /// <summary>
        /// Переопределяется в наследниках, возвращает очередной токен
        /// </summary>
        /// <returns></returns>
        public abstract Token NextToken();

        /// <summary>
        /// Берет символ из потока, но не удаляет, и сравнивает с переданным
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected virtual bool Check(char c) => _reader.Peek() == c;

        /// <summary>
        /// Передвигает текущий символ на один вправо
        /// </summary>
        protected virtual void Match()
        {
            _reader.Read();
            ++_currentPosition;
        }

        /// <summary>
        /// Комбинирует метода Match и Check
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected virtual bool MatchChecked(char c)
        {
            if (Check(c))
            {
                Match();
                return true;
            }

            return false;
        }
    }
}