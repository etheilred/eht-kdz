namespace CalculatorBackend
{
    /// <summary>
    /// Содержит определения методов, которые должен релизовывать лексический анализатор
    /// </summary>
    public interface ILexer
    {
        Token NextToken();

        bool Eof { get; }
    }
}
