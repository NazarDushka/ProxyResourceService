using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ProxyResource.Models;

namespace ProxyResource.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
            private readonly HttpClient _httpClient;
        // Кеш для збереження даних про ресурси у пам'яті
        private static readonly Dictionary<int, Resource> _resourceCache = new Dictionary<int, Resource>();

            public ResourceController(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetUser(int id)
            {
                // Перевіряємо, чи є ресурс в кеші
                if (_resourceCache.ContainsKey(id))
                {
                    return Ok(_resourceCache[id]);
                }

                // Якщо немає в кеші, робимо запит до зовнішнього API
                var response = await _httpClient.GetAsync($"https://reqres.in/api/unknown/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound($"Resource with id {id} not found.");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var Response = JsonSerializer.Deserialize<ReqresResourceResponse>(responseContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                // Додаємо користувача в кеш
                _resourceCache[id] = Response.Data;

                return Ok(Response.Data);
            }
        }
}
