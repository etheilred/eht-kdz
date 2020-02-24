namespace CalculatorBackend
{
    /// <summary>
    /// Определяет тип токена для числового литерала
    /// </summary>
    public class NumToken : Token
    {
        public string Literal { get; }

        public NumToken(string literal) : base(TokenType.Number)
            => Literal = literal;

        public override string ToString()
            => base.ToString() + $" : {Literal}";
    }
}