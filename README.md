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
    "Notes": "Needs new tires"
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
[Route("/orca-webhook-out")]
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

### WebHook In 

[Orca Scan WebHook In](https://orcascan.com/guides/how-to-update-orca-scan-from-your-system-4b249706)

```csharp
 public static async Task webHookIn(){
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
}
```
Use `http://127.0.0.1:3000/trigger-webhook-in` to trigget the in webhook and send the request.

## Test server locally against Orca Cloud

To expose the server securely from localhost and test it easily against the real Orca Cloud environment you can use [Secure Tunnels](https://ngrok.com/docs/secure-tunnels#what-are-ngrok-secure-tunnels). Take a look at [Ngrok](https://ngrok.com/) or [Cloudflare](https://www.cloudflare.com/).

```bash
ngrok http 3000
```

## Troubleshooting

If you run into any issues not listed here, please [open a ticket](https://github.com/orca-scan/orca-webhook-dotnet/issues).

## Examples in other langauges
* [orca-webhook-dotnet](https://github.com/orca-scan/orca-webhook-dotnet)
* [orca-webhook-python](https://github.com/orca-scan/orca-webhook-python)
* [orca-webhook-go](https://github.com/orca-scan/orca-webhook-go)
* [orca-webhook-java](https://github.com/orca-scan/orca-webhook-java)
* [orca-webhook-php](https://github.com/orca-scan/orca-webhook-php)
* [orca-webhook-node](https://github.com/orca-scan/orca-webhook-node)

## History

For change-log, check [releases](https://github.com/orca-scan/orca-webhook-dotnet/releases).

## License

&copy; Orca Scan, the [Barcode Scanner app for iOS and Android](https://orcascan.com).
