using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Well_blazor.Services
{

public class WellService
    {
        private readonly HttpClient _httpClient;

        public WellService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Well>> GetWellsAsync()
        {
            return await _httpClient.GetFromJsonAsync <List<Well> >("api/WELL_lite");
        }
    }

    // 定义 Well 类
    public class Well
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Slot_id { get; set; }
        public string? Slot_name { get; set; }
        public string? Structure_id { get; set; }
        public string? Src { get; set; }
        public string? Drillplan_id { get; set; }
    }
}
