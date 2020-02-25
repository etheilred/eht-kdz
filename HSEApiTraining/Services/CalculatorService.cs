using System;
using CalculatorBackend;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HSEApiTraining
{
    public interface ICalculatorService
    {
        double CalculateExpression(string expression);

        IEnumerable<(double result, string errorMessage)> CalculateBatchExpressions(IEnumerable<string> expressions);
    }

    public class CalculatorService : ICalculatorService
    {
        public IEnumerable<(double result, string errorMessage)> CalculateBatchExpressions(IEnumerable<string> expressions)
        {
            return expressions.Select(x =>
            {
                try
                {
                    var d = CalculateExpression(x);
                    return (d, null);
                }
                catch (Exception ex)
                {
                    return (0.0, ex.Message);
                }
            });
        }

        public double CalculateExpression(string expression)
        {
            ExpressionLexer lexer = new ExpressionLexer(new StringReader(expression));
            ExpressionCompiler compiler = new ExpressionCompiler(lexer);
            return compiler.GetExpression().Compile()();
        }
    }
}
