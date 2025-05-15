using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace TutorialProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorialController : ControllerBase
    {
        //bool, byte, sbyte, short, ushort, int, uint, long, ulong, float, double, decimal, char, string, object

        /* this is also a comment */

        [HttpGet]
        public Object GetBoolData()
        {
            Random random = new();

            return new {
                randomValue = random.Next(2) == 1,
                aboutType = "bool is a value type that can be either true or false. It is used to represent binary states.",
            };
        }
    }
}
