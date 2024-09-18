using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ProxyResource.Models;
using ProxyResource.Interfaces;
using Serilog;
using Microsoft.AspNetCore.Authorization;

namespace ProxyResource.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceService _resouceService;

        public ResourceController(IResourceService resourceService)
        {
            _resouceService = resourceService;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetResource(int id)
        {
            Log.Information($"Got request for Resource {id}");
            var resource = await _resouceService.GetResourceById(id);

            if (resource == null)
            {
                Log.Information($"Resource {id} not found.");
                return NotFound($"Resource with id {id} not found.");
            }

            Log.Information($"Returning Resource {id}");

            // Получаем значение флага из HttpContext, которое было установлено в middleware
            var headerPresent = (bool?)HttpContext.Items["FullHeaderPresent"] ?? true;

            if (headerPresent)
            {
                // Если заголовок присутствует, возвращаем полный объект
                return Ok(resource);
            }
            else
            {
                // Если заголовка нет, возвращаем часть данных
                return Ok(new
                {
                    resource.Id,
                    resource.Name,
                    resource.Color
                });
            }
        }
    }
}
