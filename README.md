# orca-webhook-dotnet

Example of how to build an [Orca Scan WebHook](https://orcascan.com/docs/api/webhooks) endpoint in C# using [ASP.NET Core](https://dotnet.microsoft.com/learn/aspnet/what-is-aspnet-core).

## Install

```bash
git clone git@github.com:orca-scan/orca-webhook-dotnet.git
cd orca-webhook-dotnet
dotnet restore
```

## Run

```bash
dotnet run
```

Your WebHook receiver will now be running on port 3000.

You can emulate an Orca Scan WebHook using [cURL](https://dev.to/ibmdeveloper/what-is-curl-and-why-is-it-all-over-api-docs-9mh) by running the following:

```bash
curl --location --request POST 'http://localhost:3000' \
--header 'Content-Type: application/json' \
--data-raw '{
    "___orca_action": "add",
    "___orca_sheet_name": "Vehicle Checks",
    "___orca_user_email": "hidden@requires.https",
    "___orca_row_id": "5cf5c1efc66a9681047a0f3d",
    "Barcode": "4S3BMHB68B3286050",
    "Make": "SUBARU",
    "Model": "Legacy",
    "Model Year": "2011",
    "Vehicle Type": "PASSENGER CAR",
    "Plant City": "Lafayette",
    "Trim": "Premium",
    "Location": "52.2034823, 0.1235817",
    "Notes": "Needs new tires",
}'
```

### Important things to note

1. Only Orca Scan system fields start with `___`
2. Properties in the JSON payload are an exact match to the  field names in your sheet _(case and space)_
3. WebHooks are never retried, regardless of the HTTP response

## How this example works

A simple [ASP.NET Core](https://dotnet.microsoft.com/learn/aspnet/what-is-aspnet-core) controller that listens for HTTP POSTS.

```csharp
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

            // get the email of the user who preformed the action (empty if not HTTPS)
            string userEmail = (data.___orca_user_email != null) ? (string)data.___orca_user_email : "";

            // NOTE:
            // orca system fields start with ___
            // you can access the value of each field using the field name (data.Name, data.Barcode, data.Location)

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
```

## History

For change-log, check [releases](https://github.com/orca-scan/orca-webhook-dotnet/releases).

## License

&copy; Orca Scan - [Barcode Tracking, Simplified.](https://orcascan.com)
