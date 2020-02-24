using Microsoft.AspNetCore.Mvc;
using System;

namespace HSEApiTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DummyController : Controller
    {
        private readonly Random rand;

        public DummyController()
        {
            rand = new Random();
        }

        [HttpGet("generate/{number}")]
        public string DummyGenerator(int number)
        {
            return $"Random (0, {number}): {rand.Next(number)}";
        }
    }
}
