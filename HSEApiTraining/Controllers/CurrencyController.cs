using Microsoft.AspNetCore.Mvc;

namespace HSEApiTraining.Controllers
{
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
