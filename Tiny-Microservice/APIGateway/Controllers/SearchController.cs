using APIGateway.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        [HttpPost("Search")]
        public async Task<IActionResult> search(string searchword, PostUserDTO userDto)
        {
            var AuthClient = new HttpClient();
            var AuthRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost:3001/Identity/Authenticate");

            var client = new HttpClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:3003/SearchUserController/get?searchstring{searchword}");

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

    }
}
