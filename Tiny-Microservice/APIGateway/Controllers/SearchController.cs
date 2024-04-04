using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        [HttpGet("Search")]
        public async Task<IActionResult> search(string searchword)
        {
            var AuthClient = new HttpClient();
            var AuthRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://172.18.0.1:3001/Identity/Authenticate");

            var client = new HttpClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"http://172.18.0.1:3002/SearchUser/FindUser?searchString={searchword}");

            var request = await client.SendAsync(requestMessage);
            var authRequest = await AuthClient.SendAsync(AuthRequestMessage);
            var authResult = bool.Parse(await authRequest.Content.ReadAsStringAsync());
            if (request.EnsureSuccessStatusCode() != null)
            {
                if (authResult)
                {
                    var result = await authRequest.Content.ReadAsStringAsync();
                    return Ok(result);
                }
                return Unauthorized();
                
            }
            return BadRequest();
        }
        
        [HttpGet("Auth")]
        public async Task<IActionResult> Auth()
        {
            var AuthClient = new HttpClient();
            var AuthRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://172.18.0.1:3001/Identity/Authenticate");
            var authRequest = await AuthClient.SendAsync(AuthRequestMessage);
            var authResult = bool.Parse(await authRequest.Content.ReadAsStringAsync());
            return Ok(authResult);
        }

    }
}
