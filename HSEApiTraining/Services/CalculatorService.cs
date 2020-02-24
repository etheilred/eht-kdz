using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CalculatorBackend;

namespace HSEApiTraining
{
    public interface ICalculatorService
    {
        double CalculateExpression(string expression);
        IEnumerable<double> CalculateBatchExpressions(IEnumerable<string> expressions);
    }

    public class CalculatorService : ICalculatorService
    {
        public IEnumerable<double> CalculateBatchExpressions(IEnumerable<string> expressions)
        {
            return expressions.Select(CalculateExpression);
        }

        public double CalculateExpression(string expression)
        {
            ExpressionLexer lexer = new ExpressionLexer(new StringReader(expression));
            ExpressionCompiler compiler = new ExpressionCompiler(lexer);
            return compiler.GetExpression().Compile()();
        }
    }
}
