using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cafe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HttpClientFactoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HttpClientFactoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001/api/categories");
            string result = await client.GetStringAsync("/");
            return Ok(result);
        }
    }
}
