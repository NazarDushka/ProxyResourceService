using System.ComponentModel.Design;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ProxyResource.Models;
using ProxyResource.Interfaces;

namespace ProxyResource.Services
{
    public class ResourceService : Interfaces.IResourceService
    {
        private readonly HttpClient _httpClient;
        // Кеш для збереження даних про ресурси у пам'яті
        private static readonly Dictionary<int, Resource> _resourceCache = new Dictionary<int, Resource>();

        public ResourceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{id}")]
        public async Task<Resource> GetResourceById(int id)
        {
            // Перевіряємо, чи є ресурс в кеші
            if (_resourceCache.ContainsKey(id))
            {
                return _resourceCache[id];
            }


            var response = await _httpClient.GetAsync($"https://reqres.in/api/unknown/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var Response = JsonSerializer.Deserialize<ReqresResourceResponse>(responseContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            // Додаємо ресурс в кеш
            _resourceCache[id] = Response.Data;

            return Response.Data;
        }
    }
}
