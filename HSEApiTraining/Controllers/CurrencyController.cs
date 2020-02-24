using Microsoft.AspNetCore.Mvc;

namespace HSEApiTraining.Controllers
{
    //Тут все методы-хендлеры вам нужно реализовать самим
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : Controller
    {
        [HttpGet]
        public IActionResult DummyMethod()
        {
            return View();
        }
    }
}
