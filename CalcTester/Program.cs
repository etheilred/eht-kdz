using System;
using System.IO;
using CalculatorBackend;

namespace CalcTester
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                try
                {
                    Console.Write("> ");
                    ExpressionLexer lexer = new ExpressionLexer(new StringReader(Console.ReadLine() ?? ""));
                    ExpressionCompiler compiler = new ExpressionCompiler(lexer);

                    var expression = compiler.GetExpression();
                    Console.WriteLine(expression);
                    Console.WriteLine($"= {expression.Compile()()}");
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
            } while (true);
        }
    }
}
