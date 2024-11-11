using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrpcGreeterClient.Wells;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using pseven.Models.well;
using System.Collections.Generic;
using System.Linq;
using Azure.Core;
using System.Threading.Channels;
//using pseven.Models;
using p7api.Map;
using p7api.Models.Map;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
namespace p7api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WELL_liteController : ControllerBase
    {
        //private readonly WELL_liteContext _context;
        private readonly WellService.WellServiceClient _WellLiteClient;
        private readonly ILogger<WELL_liteController> _logger;
        public WELL_liteController(WellService.WellServiceClient WellLite,ILogger<WELL_liteController> logger)
        {
            //_context = context;
            _WellLiteClient = WellLite;
            _logger = logger;
        }

        // GET: api/WELL_lite
        [HttpGet]
        public async Task<IEnumerable<GrpcGreeterClient.Wells.Well>> GetWell_lites()
            
        {
            _logger.LogInformation("Received GetWell_lites request");
            var WellsRequest = new WellRequest { Count = 10 };
            var Response = await _WellLiteClient.GetWellsAsync(WellsRequest);
            _logger.LogInformation("Returning {Count} wells", Response.Wells.Count);
            //return await _context.Well_lites.ToListAsync();
            return Response.Wells;
        }
        //[HttpGet("{id}")]
        //public async Task<ActionResult<WELL_lite>> GetWellDetails(string id)
        //{
        //    var request = new GetWellDetailsRequest { Id = id };
        //    var response = await _wellClient.GetWellDetailsAsync(request);
        //    if (response.Well == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(response.Well);
        //}
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmSelection([FromBody] JsonElement jsonElement)
        {
            if (jsonElement.TryGetProperty("well_ids", out var wellIdsElement) && wellIdsElement.ValueKind == JsonValueKind.Array)
            {
                var wellIds = new List<string>();
                foreach (var idElement in wellIdsElement.EnumerateArray())
                {
                    wellIds.Add(idElement.GetString());
                }

                _logger.LogInformation("Received well IDs: {WellIds}", string.Join(", ", wellIds));

                //var client = new WellService.WellServiceClient(_channel);
                var wellDetailsRequest = new WellDetailsRequest
                {
                    WellIds = { wellIds }
                };
                var response = await _WellLiteClient.GetWellDetailsAsync(wellDetailsRequest);
        
            if (response==null||response.WellDetails == null || response.WellDetails.Count == 0)
            {
                _logger.LogInformation("NULLS The Request: {Request}", wellDetailsRequest);
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

                if (mapping.WellDetails != null)
                {
                    foreach (var item in mapping.WellDetails)
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

				if (mapping.WellDefinition == null)
				{
					_logger.LogError("WellDefinition is null in the deserialized mapping.");
				}
				if (mapping.WellDetails == null)
				{
					_logger.LogError("WellDetails is null in the deserialized mapping.");
				}

				foreach (var wellDetails in response.WellDetails)
                {
					_logger.LogInformation("Processing well detail: {WellDetailId}", wellDetails.Id);
					var wellDetailJson = System.Text.Json.JsonSerializer.SerializeToElement(wellDetails);

					records.AddRange(JsonToTextConverter.ConvertJsonToText(wellDetailJson, mapping.WellDefinition, "well_definition",wellDetails));
					records.AddRange(JsonToTextConverter.ConvertJsonToText(wellDetailJson, mapping.WellDetails, "well_details",wellDetails));

					//records.AddRange(JsonToTextConverter.ConvertJsonToText(wellDetailJson, mapping));

                    var structid = wellDetails.StructureId;
                    var slotid = wellDetails.SlotId;
                    var time = wellDetails.Metadata.Time;
                    var PRIMNAME = wellDetails.Metadata.Src;
                }

				//var records = JsonToTextConverter.ConvertJsonToText(response , mapping);

				var fileContent = string.Join("\n", records);
				var fileBytes = Encoding.UTF8.GetBytes(fileContent);

				_logger.LogInformation("Generated file content.");

				
				var fileName = "well_details.p717";
				_logger.LogInformation("Returning file to client: {FileName}", fileName);
				return File(fileBytes, "text/plain", fileName);
				
               
                //return File(fileBytes, "text/plain", fileName);
                ////return Ok(response.WellDetails);
                
        }
            return BadRequest("Invalid request format.");
        }
        [HttpGet("test")]
        public  IActionResult TestWellDetailsRequest()
        {
            var wellDetailsRequest = new WellDetailsRequest();
            wellDetailsRequest.WellIds.AddRange(new[] { "5ca5714cafab4e0a94ed941f", "5caad4e8cf144d6014bf415b" });

            _logger.LogInformation("Initialized WellDetailsRequest with well IDs: {Ids}", string.Join(", ", wellDetailsRequest.WellIds));
            
            var response =  _WellLiteClient.GetWellDetails(wellDetailsRequest);

            if (response.WellDetails == null || response.WellDetails.Count == 0)
            {
                _logger.LogWarning("No well details found for the provided well IDs");
                _logger.LogInformation("NULLS The Request: {Request}", wellDetailsRequest);
                return NotFound();
            }
            _logger.LogInformation("Returning {Count} well details", response.WellDetails.Count);
            //return Ok(response.WellDetails);
            return Ok(response.WellDetails);
        }
        // GET: api/WELL_lite/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<WELL_lite>> GetWELL_lite(string id)
        //{
        //    var wELL_lite = await _context.Well_lites.FindAsync(id);

        //    if (wELL_lite == null)
        //    {
        //        return NotFound();
        //    }

        //    return wELL_lite;
        //}

        //// PUT: api/WELL_lite/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutWELL_lite(string id, WELL_lite wELL_lite)
        //{
        //    if (id != wELL_lite.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(wELL_lite).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!WELL_liteExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/WELL_lite
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<WELL_lite>> PostWELL_lite(WELL_lite wELL_lite)
        //{
        //    _context.Well_lites.Add(wELL_lite);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (WELL_liteExists(wELL_lite.Id))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetWELL_lite", new { id = wELL_lite.Id }, wELL_lite);
        //}

        //// DELETE: api/WELL_lite/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteWELL_lite(string id)
        //{
        //    var wELL_lite = await _context.Well_lites.FindAsync(id);
        //    if (wELL_lite == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Well_lites.Remove(wELL_lite);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool WELL_liteExists(string id)
        //{
        //    return _context.Well_lites.Any(e => e.Id == id);
        //}
    }
}
