using HSEApiTraining.Models.Dummy;
using Microsoft.AspNetCore.Mvc;

namespace HSEApiTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DummyController : Controller
    {
        private readonly IDummyService _dummyService;

        public DummyController(IDummyService dummyService) => _dummyService = dummyService;

        [HttpGet("generate/{number}")]
        public DummyResponse DummyGenerator(int number)
        {
            var q = _dummyService.DummyInt(number);

            return new DummyResponse
            {
                RandomInt = q,
                Formatted = $"Random(0, {number}) : {q}",
            };
        }
    }
}
