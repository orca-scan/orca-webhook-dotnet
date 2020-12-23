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

                // get the name of the action that triggered this request (add, update, delete, test)
                string action = (data.___orca_action != null) ? (string)data.___orca_action : "";

                // get the name of the sheet this action impacts
                string sheetName = (data.___orca_sheet_name != null) ? (string)data.___orca_sheet_name : "";

                // get the email address of the user who preformed the action (this will be empty if the request is not secure HTTPS)
                string userEmail = (data.___orca_user_email != null) ? (string)data.___orca_user_email : "";

                // NOTE:
                // orca system fields start with ___
                // you can access the value of each field using the fieldname (data.Name, data.Barcode, data.Location)

                // decide on what action to take based on orca action
                switch (action) {

                    case "add": {
                        // TODO: do something when a row has been added
                    } break;

                    case "update": {
                        // TODO: do something when a row has been updated
                    } break;

                    case "delete": {
                        // TODO: do something when a row has been deleted
                    } break;

                    case "test": {
                        // TODO: do something when the user in the web app hits the test button
                    } break;
                }
            }

            // always return a 200 (ok)
            return new OkResult();
        }
    }
}