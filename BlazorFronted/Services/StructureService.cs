using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Net.Http.Json;

namespace BlazorFronted.Services
{
	public class StructureService
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<StructureService> _logger;
		public StructureService(HttpClient httpClient, ILogger<StructureService> logger)
		{
			_httpClient = httpClient;
			_logger = logger;
		}
		public async Task<List<Structure>> GetStructuresAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Structure>>("api/Structure_lite");
        }
        public async Task<HttpResponseMessage> PostStructureSelectedAsync(List<string> selectedStructures)
        {
            Console.WriteLine("starts:");
            var jsonContent = new StringContent(JsonSerializer.Serialize(new { Structure_ids = selectedStructures }), Encoding.UTF8, "application/json");
            Console.WriteLine($"JSON Content: {await jsonContent.ReadAsStringAsync()}");

            var response = await _httpClient.PostAsync("api/Structure_lite/confirm", jsonContent);
            Console.WriteLine($"Response str: {response}");
            return response;
        }
    }
	public class Structure
	{
        public string Id { get; set; }
        public string? Name { get; set; }
		public string? FieldName {  get; set; }
    }
}
