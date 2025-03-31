using Microsoft.AspNetCore.Mvc;

namespace Publisher.Presentation.Controllers
{
    [ApiController]
    [Route("api/azure-functions")]
    public class AzureFunctionsController(IHttpClientFactory httpClientFactory) : ControllerBase
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient();

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _httpClient.GetStringAsync("http://localhost:7071/api/HelloWorld");
            return Ok(response); // Returns "Hello World from Azure Functions!"
        }
    }
}

