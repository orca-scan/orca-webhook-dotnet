using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OrcaWebHookDotNet.Controllers
{
    [ApiController]
    [Route("/")]
    public class OrcaWebHookDotNet : ControllerBase
    {
        [HttpPost]
        [Consumes("application/json")]
        public async Task<OkResult> WebHookReceiver()
        {
            // WebHooks send data as a HTTP POST, so extract JSON string from Request.Body
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {  
                // get the raw JSON string
                string json = await reader.ReadToEndAsync();

                // convert into .net object
                dynamic data = JObject.Parse(json);

                // TEST: write out one of the properties
                Console.WriteLine(data.firstName);
            }

            // always return a 200 (ok)
            return new OkResult();
        }
    }
}