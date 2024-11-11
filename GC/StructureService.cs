using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using pseven.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Policy;

namespace pseven.Services
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
        public async Task FetchAndSaveStructureMetaAsync()
        {
            string token = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ik1UY3dNemN5TVRZd01nPT0iLCJ0eXAiOiJKV1QifQ.eyJzdWJpZCI6Ilk4OUhvcU01TXFtY2dzZTl1N0xIRFBQMzVTUVViTjlfOVpSUEtac0E5aEkiLCJlbWFpbCI6Imp6aGFuZzIwMkBzbGIuY29tIiwidW5pcXVlX25hbWUiOiJqemhhbmcyMDJAc2xiLmNvbSIsImNvbW1vbm5hbWUiOiJKaW5odWkgWkhBTkciLCJmaXJzdG5hbWUiOiJKaW5odWkiLCJsYXN0bmFtZSI6IlpIQU5HIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvY2xhaW1zL0FjY291bnRJZCI6IkRyaWxsUGxhblNoYXJlZCIsImN0aWQiOiJEcmlsbFBsYW4tU2hhcmVkIiwiZHBuIjoiRHJpbGxQbGFuLVNoYXJlZCIsImZ0ciI6WyJTdGFuZGFyZF9BY2Nlc3MiLCJNYW5hZ2VfTVNEIiwiTW5nX0NvcnBfU2V0dGluZ3MiXSwiYXV0aF90aW1lIjoiMDkvMDEvMjAyNCAwMzozMzo0MCIsImRwaWQiOiJkcmlsbHBsYW4tc2hhcmVkLWRyaWxscGxhbi1zaGEiLCJlbnRpdGxlbWVudHMiOlsibXNkIiwiY29ycG9yYXRlX3NldHRpbmdzIl0sIm5hbWVpZCI6Imp6aGFuZzIwMkBzbGIuY29tIiwibmJmIjoxNzI1MjQ4NTc5LCJleHAiOjE3MjUyNTAzNzksImlhdCI6MTcyNTI0ODU3OSwiaXNzIjoidGFpamktc3RzLWRldi5henVyZXdlYnNpdGVzLm5ldCIsImF1ZCI6IkxvY2FsaG9zdFRva2VuQ2xpZW50In0.COHXzqW5e621hZvfNKry23MyxNDVAQ5F0NW_1amoOPxzXtyXfI3sM7z-A7AnCLUMwVNxHPleu0WWOFYDuR3Dempp9tQCzMnVCWcx6urhSsKCcu8g6c6b1MrnsYLlspj8-MlgPSeQnuUNMG2rMLdH2Oc1TYVJClxr1TbCoegRq38qzvq6bLkJQ-EVWpjBGh5hORbxqTpoNh0CV7m-A-Gfrzpbh20lNlGHN5w7UcG8OX3JDGxQSRkfr5dj_pGTwifrjBWNEd8Oc1ZVu2L5mb7d5QEoGp26S5ibngy8X1bffunBthdyVcX56fFPh2M-s0Ykrjp9LIuB5LoPfppQGZ5_Sg";
            var StructureMeta = await FetchStructureMetaAsync(token);
            System.Diagnostics.Debug.Write(StructureMeta);
            var random = new Random();
            var selectedStructureMeta = StructureMeta.Take(200).ToList();
            System.Diagnostics.Debug.Write(StructureMeta);
            var result = new List<object>();
            foreach (var item in StructureMeta)
            {
                var detail = await FetchDetailAsync(item.Structures_id, token);
                var slots = await FetchSlotsAsync(item.Structures_id, token);
                var surround = await FetchsurroundAsync(item.Structures_id, token);
                result.Add(new
                {
                    Id = item.Structures_id,
                    Name = item.Structures_name,
                    FieldName = item.Structures_field_name,
                    Detail = detail,
                    Slots = slots,
                    Surround = surround
                });

            }
            var json = System.Text.Json.JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync("./static/lite_items.json", json);
        }
        private async Task<List<Structure_meta>> FetchStructureMetaAsync(string token)
        {
            using HttpClient client1 = new();
            client1.DefaultRequestHeaders.Accept.Clear();
            client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            client1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _logger.LogInformation("Fetching Lite Items with Token: {Token}", token);

            var response = await client1.GetStringAsync("https://tj06.evt.slb.com/msd/api/v1/structures/lite");
            System.Diagnostics.Debug.WriteLine(response);
            return System.Text.Json.JsonSerializer.Deserialize<List<Structure_meta>>(response);
            
        }

        private async Task<object> FetchDetailAsync(string id, string token)
        {
            using HttpClient client2 = new();
            client2.DefaultRequestHeaders.Accept.Clear();
            client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            //client2.DefaultRequestHeaders.Add("Accept", "text/plain");
            //client2.DefaultRequestHeaders.Add("Authorization", token);
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Console.WriteLine("Fetching Detail for ID: " + id + " with Token: " + token);

            var response = await client2.GetStringAsync($"https://tj06.evt.slb.com/msd/api/v1/structures/{id}");

            return System.Text.Json.JsonSerializer.Deserialize<object>(response);
        }

        private async Task<object> FetchSlotsAsync(string id, string token)
        {
            using HttpClient client3 = new();
            client3.DefaultRequestHeaders.Accept.Clear();
            client3.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            client3.DefaultRequestHeaders.Add("Accept", "text/plain");
            client3.DefaultRequestHeaders.Add("Authorization", token);
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Console.WriteLine("Fetching Slots for ID: " + id + " with Token: " + token);

            var response = await client3.GetStringAsync($"https://tj06.evt.slb.com/msd/api/v1/structures/{id}/slots");

            return System.Text.Json.JsonSerializer.Deserialize<object>(response);
        }
        private async Task<object> FetchsurroundAsync(string id, string token)
        {
            using HttpClient client4 = new();
            client4.DefaultRequestHeaders.Accept.Clear();
            client4.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            client4.DefaultRequestHeaders.Add("Accept", "text/plain");
            client4.DefaultRequestHeaders.Add("Authorization", token);
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Console.WriteLine("Fetching Detail for ID: " + id + " with Token: " + token);

            var response = await client4.GetStringAsync($"https://tj06.evt.slb.com/msd//api/v1/structures/{id}/surround");

            return System.Text.Json.JsonSerializer.Deserialize<object>(response);
        }


    }      
}
