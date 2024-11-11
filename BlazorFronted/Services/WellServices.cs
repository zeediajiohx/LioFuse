using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;

using System.Collections.Generic;
namespace BlazorFronted.Services;
public class WellService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WellService> _logger;

    public WellService(HttpClient httpClient,ILogger<WellService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<Well>> GetWellsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Well>>("api/WELL_lite");
    }
    public async Task<HttpResponseMessage> PostWellSelectedAsync(List<string> selectedWells)
    {
        var jsonContent = new StringContent(JsonSerializer.Serialize(new { well_ids = selectedWells }), Encoding.UTF8, "application/json");
        Console.WriteLine($"JSON Content: {await jsonContent.ReadAsStringAsync()}");

        var response = await _httpClient.PostAsync("api/WELL_lite/confirm", jsonContent);
        Console.WriteLine($"Response: {response}");
        return response;
    }
}
//public class ConfirmSelectionService
//{
//    private readonly HttpClient _httpClient;
//    public ConfirmSelectionService(HttpClient httpClient)
//    {
//        _httpClient= httpClient;
//    }
    
//}
public class Well
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SlotId { get; set; }
    public string SlotName { get; set; }
    public string StructureId { get; set; }
    public string Src { get; set; }
    public string DrillplanId { get; set; }
}