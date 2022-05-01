using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace OrcaWebHookDotNet.Controllers
{
    [ApiController]
    [Route("/trigger-webhook-in")]
    public class OrcaWebHookInDotNet : ControllerBase
    {
        [HttpGet]
        public async Task<OkResult> WebHookTrigger()
        {
            // the following example adds a new row to a sheet, setting the value of Barcode, Name, Quantity and Description
            // TODO: change url to https://api.orcascan.com/sheets/{id}
            string url = "https://httpbin.org/post";
            string json = "{\"___orca_action\":\"add\",\"___orca_sheet_name\":\"Sheet1\",\"Barcode\":\"123456789\",\"Name\":\"Test\",\"Quantity\":\"1\",\"Description\":\"Test\"}";

            //send post request
            var client = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync(url, content);

            //if response ok print it
            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadAsStringAsync();
                Console.WriteLine(response);
            }
            //return ok
            return Ok();
        }
    }
}