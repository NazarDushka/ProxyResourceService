using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ProxyResource.Models;
using ProxyResource.Interfaces;
using Serilog;

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
        public async Task<IActionResult> GetUser(int id)
        {
            Log.Information($"Got request for Resource{id}");
            var user = await _resouceService.GetResourceById(id);
            if (user == null)
            {
                Log.Information($"Resource{id} didn't find");
                return NotFound($"Resource with id {id} not found.");
            }
            Log.Information($"Returning Resource{id}");
            return Ok(user);
        }
    }
}
