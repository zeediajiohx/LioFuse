using Grpc.Core;
using GrpcGreeter.Wells;
using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GrpcGreeter.Services
{
    public class WellService : Wells.WellService.WellServiceBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<WellService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;
        //public 
        //private readonly ILogger<GreeterService> _logger;
        public WellService(IHttpClientFactory httpClientFactory, ILogger<WellService> logger, JsonSerializerOptions jsonOptions)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _jsonOptions = jsonOptions;
        }

        public override async Task<WellResponse> GetWells(WellRequest request, ServerCallContext context)
        {
            var httpClient = _httpClientFactory.CreateClient("WellFetchApiClient");
            try
            {
                _logger.LogInformation("Sending request to API: {Url}", httpClient.BaseAddress + "api/v1/wells/lite");
                var response = await httpClient.GetAsync("api/v1/wells/lite");
                response.EnsureSuccessStatusCode();
                _logger.LogInformation("Received HTTP response: {StatusCode}", response.StatusCode);
                var responseContent = await response.Content.ReadAsStringAsync();
                //_logger.LogInformation("Response content: {Content}", responseContent);
                var wells = new List<Well>();
                using (JsonDocument doc = JsonDocument.Parse(responseContent))
                {
                    foreach (var element in doc.RootElement.EnumerateArray())
                    {
                        var well = new Well
                        {
                            Id = element.TryGetProperty("id", out var idProp) ? idProp.GetString() ?? string.Empty : string.Empty,
                            Name = element.TryGetProperty("name", out var nameProp) ? nameProp.GetString() ?? string.Empty : string.Empty,
                            SlotId = element.TryGetProperty("slot_id", out var slotIdProp) ? slotIdProp.GetString() ?? string.Empty : string.Empty,
                            SlotName = element.TryGetProperty("slot_name", out var slotNameProp) ? slotNameProp.GetString() ?? string.Empty : string.Empty,
                            StructureId = element.TryGetProperty("structure_id", out var structureIdProp) ? structureIdProp.GetString() ?? string.Empty : string.Empty,
                            Src = element.TryGetProperty("src", out var srcProp) ? srcProp.GetString() ?? string.Empty : string.Empty,
                            DrillplanId = element.TryGetProperty("drillplan_id", out var drillplanIdProp) ? drillplanIdProp.GetString() ?? string.Empty : string.Empty
                        };
                        wells.Add(well);
                    }
                }
                //var wells = await response.Content.ReadFromJsonAsync<List<Well>>();

                if (wells != null)
                {
                    _logger.LogInformation("Received {Count} wells from API", wells.Count);
                    //foreach (var well in wells)
                    //{
                    //    _logger.LogInformation("Well: id, {Name}, {SlotName},  {Src}, {Drillplan_id}",
                    //         well.Name, well.SlotName,  well.Src, well.DrillplanId);
                    //}
                    var wellsWithNullCounts = wells.Select(well => new
                    {
                        Well = well,
                        NullCount = new[]
                        {
                        well.Id,
                        well.Name,
                        well.SlotId,
                        well.SlotName,
                        well.StructureId,
                        well.Src,
                        well.DrillplanId,
                    }.Count(value => string.IsNullOrEmpty(value))
                    });

                    var sortedWells = wellsWithNullCounts.OrderBy(well => well.NullCount).ToList();

                    Random random = new Random();
                    var randomTopWells = sortedWells.Take(request.Count*50).OrderBy(_ => random.Next()).Take(request.Count).Select(well => well.Well).ToList();

                    var responseMessage = new WellResponse();
                    responseMessage.Wells.AddRange(randomTopWells);

                    return responseMessage;



                }
                else
                {
                    _logger.LogWarning("no wells from API");
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


            return new WellResponse();
        }
        public override async Task<WellDetailsResponse> GetWellDetails(WellDetailsRequest request, ServerCallContext context)
        {
            var httpClient = _httpClientFactory.CreateClient("WellFetchApiClient");
            var wells = new List<WellDetail>();
            _logger.LogInformation(" ^_^ well details for ID");
            foreach (var id in request.WellIds)
            {
                try
                {
                    _logger.LogInformation("Fetching well details for ID: {Id}", id);
                    var wellResponse = await httpClient.GetAsync($"api/v1/wells/{id}");
                    wellResponse.EnsureSuccessStatusCode();
                    if (wellResponse.IsSuccessStatusCode)
                    {
                        var responseContent = await wellResponse.Content.ReadAsStringAsync();
                        var welldetresp = new WellDetailsResponse();
                        using (JsonDocument doc = JsonDocument.Parse(responseContent))
                        {
                            _logger.LogInformation("Processing well data for ID: {Id}", id);
                            
                            var thiswelld = new WellDetail();
                            
                            foreach (var element in doc.RootElement.EnumerateObject())
                            {
                                _logger.LogInformation("Key: {Key}, Value: {Value}", element.Name, element.Value);
                            }
                        }
                        var well = JsonHelper.DeserializeWithMapping <WellDetail> (responseContent);

                        //var wellDetailss = new List<WellDetail>();
                        //var well = await wellResponse.Content.ReadFromJsonAsync<WellDetail>(_jsonOptions);

                        if (well != null)
                        {
                            wells.Add(well);
                            _logger.LogInformation("Received HTTP Name: {Name}", well.Name);
                        }
                        else
                        {
                            _logger.LogWarning("No well found for ID: {Id}", id);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Failed to fetch well for ID: {Id}", id);
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

                //var well = await httpClient.GetFromJsonAsync<WellDetail>($"api/v1/wells/{id}");


                ////_logger.LogInformation("Response content: {Content}", responseContent);
                //if (well != null)
                //{
                //    if (well.Area == null)
                //    {
                //        well.Area = string.Empty; //  
                //    }

                //    wells.Add(well);
                //    _logger.LogInformation("Received HTTP Name: {Name}", well.Name);
                //}
                //else
                //{
                //    _logger.LogWarning("No well found for ID: {Id}", id);
                //}
            }

            return new WellDetailsResponse
            {
                WellDetails = { wells }
            };
        }

    }
}
