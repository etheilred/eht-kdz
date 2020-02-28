#define Calc

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CalculatorBackend;
using Newtonsoft.Json.Linq;

namespace CalcTester
{
    class Program
    {
#if Calc
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
#elif Http
        static async Task Main()
        {
            var response = await new HttpClient().GetAsync("https://api.ratesapi.io/api/2010-01-12?base=RUB&symbols=USD");
            var jObect = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            Console.WriteLine(jObect["rates"]["USD"].Type);
            //Console.WriteLine(t.Date);
        }
#endif
    }

}
