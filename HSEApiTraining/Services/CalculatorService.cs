using System;
using System.Collections.Generic;

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
            throw new NotImplementedException();
        }

        public double CalculateExpression(string expression)
        {
            return expression.Length;
        }
    }
}
