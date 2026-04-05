using System.Text;
using System.Text.Json;

var httpClient = new HttpClient();

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:8888");

var app = builder.Build();

// receive data from Orca Scan (Webhook Out)
app.MapPost("/orca-webhook-out", async (HttpRequest request) =>
{
    using var reader = new StreamReader(request.Body);
    var body = await reader.ReadToEndAsync();

    if (string.IsNullOrWhiteSpace(body))
        return Results.BadRequest("Missing request body");

    JsonElement data;
    try
    {
        data = JsonDocument.Parse(body).RootElement;
    }
    catch (JsonException)
    {
        return Results.BadRequest("Invalid JSON");
    }

    // debug: log raw data received
    Console.WriteLine("Request received:\n" + body);

    // get the event that triggered this request (rows:add, rows:update, rows:delete, test)
    var orcaEvent = data.TryGetProperty("___orca_event", out var eventProp) ? eventProp.GetString() : "";

    // get the name of the sheet this event impacts
    var sheetName = data.TryGetProperty("___orca_sheet_name", out var sheetProp) ? sheetProp.GetString() : "";

    // get the email of the user who performed the action (empty if not HTTPS)
    var userEmail = data.TryGetProperty("___orca_user_email", out var emailProp) ? emailProp.GetString() : "";

    // NOTE:
    // orca system fields start with ___
    // you can access the value of each field using data.GetProperty("fieldname").GetString()

    switch (orcaEvent)
    {
        case "rows:add":
            // TODO: do something when a row has been added
            break;

        case "rows:update":
            // TODO: do something when a row has been updated
            break;

        case "rows:delete":
            // TODO: do something when a row has been deleted
            break;

        case "test":
            // TODO: do something when the user in the web app hits the test button
            break;
    }

    return Results.Text("OK");
});

// push data into Orca Scan (Webhook In)
app.MapGet("/trigger-webhook-in", async () =>
{
    // the following example adds a new row to a sheet, setting the value of Barcode, Name, Quantity and Description
    // TODO: change url to https://api.orcascan.com/sheets/{id}
    var json = JsonSerializer.Serialize(new Dictionary<string, object>
    {
        { "___orca_action", "add" },
        { "Barcode", "0123456789" },
        { "Name", "New 1" },
        { "Quantity", 12 },
        { "Description", "Add new row example" }
    });

    var response = await httpClient.PostAsync(
        "https://httpbin.org/post",
        new StringContent(json, Encoding.UTF8, "application/json")
    );

    Console.WriteLine(response.StatusCode);

    return Results.Text("OK");
});

app.Run();
