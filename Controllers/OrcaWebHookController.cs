using Microsoft.AspNetCore.Mvc;

namespace OrcaWebHookDotNet.Controllers
{
    [ApiController]
    [Route("/")]
    [Produces("application/json")]
    public class OrcaWebHookDotNet : ControllerBase
    {
        [HttpPost]
        public IActionResult SaveChanges()
        {
            // get the incoming barcode from Orca Scan
            string barcode = HttpContext.Request.Query["barcode"].ToString();

            // go get the data from a database

            // return it as a JSON object with 200 response
            return new OkObjectResult(new { Id = 123, barcode = barcode, Name = "Hero" });
        }
    }
}