namespace CalculatorBackend
{
    /// <summary>
    /// Определяет базовый класс для всех токенов
    /// </summary>
    public class Token
    {
        public int Position { get; set; }

        public TokenType Type { get; }

        public Token(TokenType type) => Type = type;

        public override string ToString() => $"{Type}";
    }
}