# orca-webhook-dotnet

Example of how to build an [Orca Scan WebHook](https://orcascan.com/docs/api/webhooks) endpoint in C# using .Net Core.

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

This will now be running on port 3000.

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

## History

For change-log, check [releases](https://github.com/orca-scan/orca-webhook-dotnet/releases).
