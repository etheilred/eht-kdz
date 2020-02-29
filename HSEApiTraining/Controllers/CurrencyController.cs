using HSEApiTraining.Models.Currency;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HSEApiTraining.Controllers
{
    //Тут все методы-хендлеры вам нужно реализовать самим
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : Controller
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
            => _currencyService = currencyService;

        [HttpPost]
        public async Task<CurrencyResponce> GetRates(CurrencyRequest request)
        {
            try
            {
                return new CurrencyResponce
                {
                    Rates = await _currencyService.GetRates(request.Symbol,
                        request.DateStart,
                        request.DateEnd),
                    Error = null,
                };
            }
            catch (Exception e) 
            {
                return new CurrencyResponce
                {
                    Rates = null,
                    Error = e.Message,
                };
            }
        }
    }
}
