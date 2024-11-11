using GrpcGreeterClient.Structures;
using Microsoft.AspNetCore.Mvc;
using p7api.Map;
using p7api.Models.Map;
using System.Text.Json;
using System.Text;
using GrpcGreeterClient.Wells;

namespace p7api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class STRUCTURE_liteController : ControllerBase
	{
		//private readonly STRUCTURE_liteContext _context;
		private readonly StructureService.StructureServiceClient _StructureLiteClient;
		private readonly ILogger<STRUCTURE_liteController> _logger;
		public STRUCTURE_liteController(StructureService.StructureServiceClient StructureLite, ILogger<STRUCTURE_liteController> logger)
		{
			//_context = context;
			_StructureLiteClient = StructureLite;
			_logger = logger;
		}

		// GET: api/STRUCTURE_lite
		[HttpGet]
		public async Task<IEnumerable<GrpcGreeterClient.Structures.Structure>> GetStructure_lites()
		{
			_logger.LogInformation("Received GetStructure_lites request");
			var StructuresRequest = new StructureRequest { Count = 10 };
			var Response = await _StructureLiteClient.GetStructuresAsync(StructuresRequest);
			_logger.LogInformation("Returning {Count} structures", Response.Structures.Count);
			//return await _context.Structure_lites.ToListAsync();
			return Response.Structures;
		}
		//[HttpGet("{id}")]
		//public async Task<ActionResult<STRUCTURE_lite>> GetStructureDetails(string id)
		//{
		//    var request = new GetStructureDetailsRequest { Id = id };
		//    var response = await _structureClient.GetStructureDetailsAsync(request);
		//    if (response.Structure == null)
		//    {
		//        return NotFound();
		//    }
		//    return Ok(response.Structure);
		//}
		[HttpPost("confirm")]
		public async Task<IActionResult> ConfirmSelection([FromBody] JsonElement jsonElement)
		{
			if (jsonElement.TryGetProperty("Structure_ids", out var structureIdsElement) && structureIdsElement.ValueKind == JsonValueKind.Array)
			{
				var structureIds = new List<string>();
				foreach (var idElement in structureIdsElement.EnumerateArray())
				{
					structureIds.Add(idElement.GetString());
				}

				_logger.LogInformation("Received structure IDs: {StructureIds}", string.Join(", ", structureIds));

				//var client = new StructureService.StructureServiceClient(_channel);
				var structureDetailsRequest = new StructureDetailsRequest
				{
					StructureIds = { structureIds }
				};
				var response = await _StructureLiteClient.GetStructureDetailsAsync(structureDetailsRequest);

				if (response == null || response.StructureDetails == null || response.StructureDetails.Count == 0)
				{
					_logger.LogInformation("NULLS The Request: {Request}", structureDetailsRequest);
					return NotFound();
				}
				var records = new List<string>();
                records.Add("IOGP,User Guide Example A,7,1.0,1,2018:08:30,14:42:26, P717 User Guide example A.p717, P7 task force");
                records.Add("CC,0,0,0,** IOGP P7 User Guide Example A");
                records.Add("CC,0,0,0,** Created by User Guide group on 2018-10-19");
                records.Add("CC,0,0,0,** Based on test dataset Input file: \"Alpha 01 BIG RIG WIRELINE GYRO CONTINUOUS.PDF\"");
                records.Add("CC,0,0,0,** The DF is used as ZDP, 26 ft above GL, which is 2600 ftUS above the VRS (total vertical to ZDP 2626)");
                records.Add("CC,0,0,0,=================================================");
                records.Add("CC,0,0,0,  Implicit CRS/CT Identification\r\n"); records.Add("CC,0,0,0,=================================================");
                records.Add("HC,1,3,0,CRS Number/EPSG Code/Name/Source                  ,1, 4267, NAD27,                       9.5,2018:09:06,EPSG, \r\n");
                records.Add("HC,1,3,0,CRS Number/EPSG Code/Name/Source                  ,2,32039, NAD27 / Texas South Central, 9.5,2018:09:06,EPSG,\r\n");
                records.Add("HC,1,3,0,CRS Number/EPSG Code/Name/Source                  ,3, 6358, NAVD88 depth (ftUS),         9.5,2018:09:06,EPSG, \r\n");
                records.Add("HC,1,3,0,CRS Number/EPSG Code/Name/Source                  ,4, 4326, WGS 84,                      9.5,2018:09:06,EPSG, \r\n");
                records.Add("HC,1,6,1,Coordinate System Axis 1                          ,2,1,, Northing, north, N, 12,ftUS\r\n");
                records.Add("HC,1,6,1,Coordinate System Axis 2                          ,2,2,, Easting,  east,  E, 12,ftUS\r\n");
                records.Add("HC,1,6,1,Coordinate System Axis 1                          ,3,1,, Depth,    down,  D, 12,ftUS\r\n");
                records.Add("HC,1,6,2,Coordinate Axis Conversion Applied                ,2,15498, axis order change (2D),9843,Axis Order Reversal (2D)\r\n");
                records.Add("HC,1,7,0,Transformation Number/EPSG Code/Name/Source       ,1,15851, NAD27 to WGS 84 (79),        9.5,2018:09:06,EPSG,\r\n");
                records.Add("CC,0,0,0,  ----------------------------------------------\r\n");
                records.Add("CC,0,0,0,  Mandatory Entities");
                records.Add("CC,0,0,0,  ----------------------------------------------\r\n");

                _logger.LogInformation("Loading mapping.json file.");
				//var mappingContent = ResourceHelper.GetEmbeddedResource("p7api.mapping.json");
				//_logger.LogInformation("Mapping content: {MappingContent}", mappingContent);
				string filePath = "C:\\Users\\JZhang202\\source\\repos\\p7Api\\p7api\\mapping.json";
				var mappingstring = System.IO.File.ReadAllText(filePath);
				Console.WriteLine("File content:");
				//Console.WriteLine(mappingstring);
				Mapping mapping = Newtonsoft.Json.JsonConvert.DeserializeObject<Mapping>(mappingstring);

				if (mapping.Structure!= null)
				{
					foreach (var item in mapping.Structure)
					{
						Console.WriteLine($"ID: {item.Key}, Description: {item.Value.MSD_source}");
					}
				}
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};

				//var mapping = JsonSerializer.Deserialize<Mapping>(mappingContent, options);

				//_logger.LogInformation("Deserialized mapping: {Mapping}", System.Text.Json.JsonSerializer.Serialize(mapping, options));
				if (mapping == null)
				{
					_logger.LogError("Failed to deserialize mapping.json file.");
					return StatusCode(500, "Failed to deserialize mapping.json file.");
				}

				if (mapping.Structure == null)
				{
					_logger.LogError("StructureDefinition is null in the deserialized mapping.");
				}
				//if (mapping.StructureDetails == null)
				//{
				//	_logger.LogError("StructureDetails is null in the deserialized mapping.");
				//}

				foreach (var structureDetails in response.StructureDetails)
				{

					_logger.LogInformation("Processing structure detail: {StructureDetailId}", structureDetails.Id);
					var structureDetailJson = System.Text.Json.JsonSerializer.SerializeToElement(structureDetails);
					var wellDetilslist = structureDetails.WellSet;
					var wells = structureDetails;
					records.AddRange(StrJsonToTextConverter.ConvertJsonToText(structureDetailJson, mapping.Structure, "structure", structureDetails));
					foreach (var wellDetails in wellDetilslist) {
                        _logger.LogInformation("Processing well detail: {WellDetailId}", wellDetails.Id);
                        var wellDetailJson = System.Text.Json.JsonSerializer.SerializeToElement(wellDetails);

                        records.AddRange(JsonToTextConverter.ConvertJsonToText(wellDetailJson, mapping.WellDefinition, "well_definition", wellDetails));
                        records.AddRange(JsonToTextConverter.ConvertJsonToText(wellDetailJson, mapping.WellDetails, "well_details", wellDetails));
                    }
					//records.AddRange(JsonToTextConverter.ConvertJsonToText(structureDetailJson, mapping.StructureDetails, "structure_details", structureDetails));

					//records.AddRange(JsonToTextConverter.ConvertJsonToText(structureDetailJson, mapping));

					//var structid = structureDetails.StructureId;
					//var slotid = structureDetails.SlotId;
					var time = structureDetails.Metadata.Time;
					var PRIMNAME = structureDetails.Metadata.Src;
				}

				//var records = JsonToTextConverter.ConvertJsonToText(response , mapping);

				var fileContent = string.Join("\n", records);
				var fileBytes = Encoding.UTF8.GetBytes(fileContent);

				_logger.LogInformation("Generated file content.");


				var fileName = "structure_details.p717";
				_logger.LogInformation("Returning file to client: {FileName}", fileName);
				return File(fileBytes, "text/plain", fileName);


				//return File(fileBytes, "text/plain", fileName);
				////return Ok(response.StructureDetails);

			}
			return BadRequest("Invalid request format.");
		}
		[HttpGet("test")]
		public IActionResult TestStructureDetailsRequest()
		{
			var structureDetailsRequest = new StructureDetailsRequest();
			structureDetailsRequest.StructureIds.AddRange(new[] { "66013e3453764296e000d12b", "641406994b8443ce393cd6b1", "65c1fd5c26d5fa8edf621fa7" });

			_logger.LogInformation("Initialized StructureDetailsRequest with structure IDs: {Ids}", string.Join(", ", structureDetailsRequest.StructureIds));

			var response = _StructureLiteClient.GetStructureDetails(structureDetailsRequest);

			if (response.StructureDetails == null || response.StructureDetails.Count == 0)
			{
				_logger.LogWarning("No structure details found for the provided structure IDs");
				_logger.LogInformation("NULLS The Request: {Request}", structureDetailsRequest);
				return NotFound();
			}
			_logger.LogInformation("Returning {Count} structure details", response.StructureDetails.Count);
			//return Ok(response.StructureDetails);
			return Ok(response.StructureDetails);
		}
	}
	}
