# P717 project 

 

## Project Overview

 The project includes a Blazor WebAssembly front-end application and a gRPC back-end service. The front-end application allows users to select wells and retrieve detailed information, and also supports manual input of well IDs.

## Environment Requirements

- .NET 6.0 or higher
- Visual Studio 2022 or Visual Studio Code
- gRPC tools

## Installation Steps

1. Clone the project repository to your local machine:

   ```bash
   cd p7api
   ```

2. Install the required .NET packages:

   ```bash
   dotnet restore
   ```

## Project Structure

- `GrpcGreeter`: gRPC server project providing structure and well-related data services.
- `p7api`: API gateway project that calls gRPC services and provides RESTful APIs.
- `BlazorFrontend`: Blazor WebAssembly front-end project for user interaction and data display.

## Configuration

### Protobuf File Configuration

Ensure that the Protobuf files are correctly configured in the project's `.csproj` file:

```xml
<ItemGroup>
  <Protobuf Include="Protos\wells.proto" GrpcServices="Both" />
  <Protobuf Include="Protos\structure.proto" GrpcServices="Both" />
</ItemGroup>
```

### gRPC Server Configuration

Configure the gRPC server in the `GrpcGreeter` project's `Program.cs` file:

```csharp
builder.Services.AddGrpc();
```

### API Gateway Configuration

Configure the gRPC client and CORS in the `p7api` project's `Program.cs` file:

```csharp
builder.Services.AddGrpcClient<StructureService.StructureServiceClient>(options =>
{
    options.Address = new Uri("https://localhost:7004");
});
builder.Services.AddGrpcClient<WellService.WellServiceClient>(options =>
{
    options.Address = new Uri("https://localhost:7004");
});
```

### Front-end Blazor WebAssembly Configuration

Ensure the front-end Blazor WebAssembly project is correctly configured.

 



## Running the Project

Before starting the project, you need to obtain a user token by logging in to `https://tj06.evt.slb.com/msd` using your browser. Copy the token and paste it into the `Program.cs` file of the `GrpcGreeter` project:

```csharp
builder.Services.AddHttpClient("WellFetchApiClient", client =>
{
    client.BaseAddress = new Uri("https://tj06.evt.slb.com/msd/"); 
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "//paste your token here");
});
```

### Starting the gRPC Server

1. Open the `GrpcGreeter` project in your IDE.
2. Ensure the token is correctly pasted in `Program.cs`.
3. Run the project:
    ```bash
    dotnet run
    ```

### Starting the API Gateway

1. Open the `p7api` project in your IDE.
2. Run the project:
    ```bash
    dotnet run
    ```

### Starting the Front-end

1. Open the `BlazorFronted` project in your IDE.
2. Run the project:
    ```bash
    dotnet run
    ```

Sure! Here is an English version of the README.md document for `mapping.json`:

## mapping.json Overview (Rule library)

The `mapping.json` file defines rules for mapping fields from external data sources to internal data models. It contains multiple mapping rules, each defining how to map fields from external data sources to properties in internal data models. The file currently includes two main sections: `well_definition` and `well_details`.

## Field Descriptions

Each mapping rule consists of the following fields:

- `MSD_source`: A list that defines data sources. Each data source includes the following two subfields:
  - `model`: Specifies the model name of the data source. For example, `well` indicates that the data comes from the `WellDetail` class.
  - `key`: Specifies the property path of the data source. For example, `metadata.src` indicates that the data comes from the `src` sub-property of the `metadata` property in the `WellDetail` class.
- `default_value`: The default value to use if the mapped value is not found.
- `method`: Specifies the mapping method. The following methods are supported:
  - `"Direct Mapping"`: Direct mapping, using the value from the data source.
  - `"Increase from 1"`: A field that increments from 1.
  - others ...
- `nullable`: Indicates whether the field can be null. If `false`, the field cannot be null.
- `description`: Describes the meaning of the field. If the description contains `"Fixed fields"`, the `value` of the field is used directly.
- `value`: The default or example value of the field.
- `need_review`: Indicates whether the field needs manual review.

### Example

Below is a partial example of the `mapping.json` file:

```json
{
  "well_definition": {
    "1": {
      "MSD_source": null,
      "default_value": null,
      "method": null,
      "nullable": false,
      "description": "Fixed fields",
      "value": "H7",
      "need_review": false
    },
    "2": {
      "MSD_source": null,
      "default_value": null,
      "method": null,
      "nullable": false,
      "description": "Fixed fields",
      "value": "1",
      "need_review": false
    }
    // Other fields omitted
  },
  "well_details": {
    "1": {
      "MSD_source": null,
      "default_value": null,
      "method": null,
      "nullable": false,
      "description": "Fixed fields",
      "value": "H7",
      "need_review": false
    },
    "2": {
      "MSD_source": null,
      "default_value": null,
      "method": null,
      "nullable": false,
      "description": "Fixed fields",
      "value": "1",
      "need_review": false
    }
    // Other fields omitted
  }
}
```

## Field Definitions

- `well_definition`: Defines mapping rules for fields related to well objects(GrpcGreeterClient.Wells.Well).
- `well_details`: Defines mapping rules for fields related to well details .
 
## Supplemental and To Be Improved
The latest rule library is in the formated_map.json file, but it is not complete and needs further proofreading and supplementation

When the rule library is updated, corresponding updates need to be made in the code project file \p7api\mapping.json
