using HSEApiTraining.Models.Calculator;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HSEApiTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculatorService _calculatorService;
        //В конструкторе контроллера происходит инъекция сервисов через их интерфейсы
        public CalculatorController(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        [HttpGet]
        public CalculatorResponse Calculate([FromQuery] string expression)
        {
            //Тут нужно подключить реализованную в сервисе calculatorService логику вычисления выражений
            //В нижнем методе - аналогично
            try
            {
                var result = _calculatorService.CalculateExpression(expression);
                return new CalculatorResponse
                {
                    Value = result
                };
            }
            catch (Exception e)
            {
                return new CalculatorResponse
                {
                    Error = e.Message
                };
            }
        }

        [HttpPost]
        public CalculatorBatchResponse CalculateBatch([FromBody] CalculatorBatchRequest Request)
        {
            IEnumerable<CalculatorResponse> responses = Request.Expressions.Select(x =>
            {
                try
                {
                    return new CalculatorResponse { Value = _calculatorService.CalculateExpression(x) };
                }
                catch (FormatException e)
                {
                    return new CalculatorResponse { Error = e.Message };
                }
            });
            string error = null;
            if (responses.Any(x => x.Error != null))
            {
                int i = 0;
                foreach (var calculatorResponse in responses)
                {
                    ++i;
                    if (calculatorResponse.Error != null) break;
                }
                error = $"Could not calculate expression at position {i}";
            }
            //Тут организуйте подсчет и формирование ответа сами
            return new CalculatorBatchResponse
            {
                Values = responses,
                Error = error
            };
        }

        //Примеры-пустышки
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
