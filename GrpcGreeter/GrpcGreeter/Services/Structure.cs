using Grpc.Core;
using GrpcGreeter.Structures;
using System.Text.Json;

namespace GrpcGreeter.Services
{
    public class StructureService : Structures.StructureService.StructureServiceBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<StructureService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;
        //public 
        //private readonly ILogger<GreeterService> _logger;
        public StructureService(IHttpClientFactory httpClientFactory, ILogger<StructureService> logger, JsonSerializerOptions jsonOptions)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _jsonOptions = jsonOptions;
        }

        public override async Task<StructureResponse> GetStructures(StructureRequest request, ServerCallContext context)
        {
            var httpClient = _httpClientFactory.CreateClient("WellFetchApiClient");
            try
            {
                _logger.LogInformation("Sending request to API: {Url}", httpClient.BaseAddress + "api/v1/structures/lite");
                var response = await httpClient.GetAsync("api/v1/structures/lite");
                response.EnsureSuccessStatusCode();
                _logger.LogInformation("Received HTTP response: {StatusCode}", response.StatusCode);
                var responseContent = await response.Content.ReadAsStringAsync();
                //_logger.LogInformation("Response content: {Content}", responseContent);
                var structures = new List<Structure>();
                using (JsonDocument doc = JsonDocument.Parse(responseContent))
                {
                    foreach (var element in doc.RootElement.EnumerateArray())
                    {
                        var structure = new Structure
                        {
                            Id = element.TryGetProperty("id", out var idProp) ? idProp.GetString() ?? string.Empty : string.Empty,
                            Name = element.TryGetProperty("name", out var nameProp) ? nameProp.GetString() ?? string.Empty : string.Empty,
                            FieldName = element.TryGetProperty("field_Name", out var slotIdProp) ? slotIdProp.GetString() ?? string.Empty : string.Empty
                        };
                        //    SlotName = element.TryGetProperty("slot_name", out var slotNameProp) ? slotNameProp.GetString() ?? string.Empty : string.Empty,
                        //    StructureId = element.TryGetProperty("structure_id", out var structureIdProp) ? structureIdProp.GetString() ?? string.Empty : string.Empty,
                        //    Src = element.TryGetProperty("src", out var srcProp) ? srcProp.GetString() ?? string.Empty : string.Empty,
                        //    DrillplanId = element.TryGetProperty("drillplan_id", out var drillplanIdProp) ? drillplanIdProp.GetString() ?? string.Empty : string.Empty
                        //};
                        structures.Add(structure);
                    }
                }
                //var structures = await response.Content.ReadFromJsonAsync<List<Structure>>();

                if (structures != null)
                {
                    _logger.LogInformation("Received {Count} structures from API", structures.Count);
                    //foreach (var structure in structures)
                    //{
                    //    _logger.LogInformation("Structure: id, {Name}, {SlotName},  {Src}, {Drillplan_id}",
                    //         structure.Name, structure.SlotName,  structure.Src, structure.DrillplanId);
                    //}
                    var structuresWithNullCounts = structures.Select(structure => new
                    {
                        Structure = structure,
                        NullCount = new[]
                        {
                        structure.Id,
                        structure.Name,
                        structure.FieldName,
                        
                    }.Count(value => string.IsNullOrEmpty(value))
                    });

                    var sortedStructures = structuresWithNullCounts.OrderBy(structure => structure.NullCount).ToList();

                    Random random = new Random();
                    var randomTopStructures = sortedStructures.Take(request.Count * 50).OrderBy(_ => random.Next()).Take(request.Count).Select(structure => structure.Structure).ToList();

                    var responseMessage = new StructureResponse();
                    responseMessage.Structures.AddRange(randomTopStructures);

                    return responseMessage;



                }
                else
                {
                    _logger.LogWarning("no structures from API");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "An error occurred while sending the request to the API");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred");
            }


            return new StructureResponse();
        }
        public override async Task<StructureDetailsResponse> GetStructureDetails(StructureDetailsRequest request, ServerCallContext context)
        {
            var httpClient = _httpClientFactory.CreateClient("WellFetchApiClient");
            var structures = new List<StructureDetail>();
            _logger.LogInformation(" ^_^ structure details for ID");
            foreach (var id in request.StructureIds)
            {
                try
                {
                    _logger.LogInformation("Fetching structure details for ID: {Id}", id);
                    var structureResponse = await httpClient.GetAsync($"api/v1/structures/{id}");
                    structureResponse.EnsureSuccessStatusCode();
                    if (structureResponse.IsSuccessStatusCode)
                    {
                        var responseContent = await structureResponse.Content.ReadAsStringAsync();
                        var structuredetresp = new StructureDetailsResponse();
                        using (JsonDocument doc = JsonDocument.Parse(responseContent))
                        {
                            _logger.LogInformation("Processing structure data for ID: {Id}", id);

                            var thisstructured = new StructureDetail();

                            foreach (var element in doc.RootElement.EnumerateObject())
                            {
                                //_logger.LogInformation("Key: {Key}, Value: {Value}", element.Name, element.Value);
                            }
                        }
                        var structure = JsonHelper.DeserializeWithMapping<StructureDetail>(responseContent);

                        //var structureDetailss = new List<StructureDetail>();
                        //var structure = await structureResponse.Content.ReadFromJsonAsync<StructureDetail>(_jsonOptions);

                        if (structure != null)
                        {
                            structures.Add(structure);
                            _logger.LogInformation("Received HTTP Name: {Name}", structure.Name);
                        }
                        else
                        {
                            _logger.LogWarning("No structure found for ID: {Id}", id);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Failed to fetch structure for ID: {Id}", id);
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "An error occurred while sending the request to the API");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An unexpected error occurred");
                }

                //var structure = await httpClient.GetFromJsonAsync<StructureDetail>($"api/v1/structures/{id}");


                ////_logger.LogInformation("Response content: {Content}", responseContent);
                //if (structure != null)
                //{
                //    if (structure.Area == null)
                //    {
                //        structure.Area = string.Empty; //  
                //    }

                //    structures.Add(structure);
                //    _logger.LogInformation("Received HTTP Name: {Name}", structure.Name);
                //}
                //else
                //{
                //    _logger.LogWarning("No structure found for ID: {Id}", id);
                //}
            }

            return new StructureDetailsResponse
            {
                StructureDetails = { structures }
            };
        }

    }
}
