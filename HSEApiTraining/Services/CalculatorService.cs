using System;
using CalculatorBackend;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace HSEApiTraining
{
    public interface ICalculatorService
    {
        double CalculateExpression(string expression);

        IEnumerable<(double result, string errorMessage)> CalculateBatchExpressions(IEnumerable<string> expressions);
    }

    public class CalculatorService : ICalculatorService
    {
        private readonly Regex _exprBarrier;

        public CalculatorService()
        {
            string numPattern = @"-?[0-9\.,\s]+";
            _exprBarrier = new Regex($"^\\s*{numPattern}\\s*[+*/%-]\\s*{numPattern}\\s*$");
        }

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
            if (!CheckExpression(expression))
                throw new FormatException($"`{expression}` does not match pattern `A + B`");

            double res = CalculateString(expression);

            return res;
        }

        private double CalculateString(string expression)
        {
            ExpressionLexer lexer = new ExpressionLexer(new StringReader(expression));
            ExpressionCompiler compiler = new ExpressionCompiler(lexer);

            return compiler.GetExpression().Compile()();
        }

        private bool CheckExpression(string expression) => _exprBarrier.IsMatch(expression);
    }
}
