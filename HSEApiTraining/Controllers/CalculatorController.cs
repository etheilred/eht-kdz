using System;
using HSEApiTraining.Models.Calculator;
using Microsoft.AspNetCore.Mvc;
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
            //Тут организуйте подсчет и формирование ответа сами
            return new CalculatorBatchResponse
            {
                Values = Request.Expressions.Select(x => new CalculatorResponse { Value = x.Length })
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
