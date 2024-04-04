using System.Net;
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

            Console.WriteLine("trying auth");
            var AuthRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://172.19.0.1:3001/Identity/Authenticate");
            Console.WriteLine("auth succesful");

            var client = new HttpClient();

            Console.WriteLine("trying to get FindUser");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"http://172.19.0.1:3002/SearchUser/FindUser?searchString={searchword}");
            Console.WriteLine("Ran through to get FindUser");

            var request = await client.SendAsync(requestMessage);
            var authRequest = await AuthClient.SendAsync(AuthRequestMessage);
            var authResult = bool.Parse(await authRequest.Content.ReadAsStringAsync());
            var responseContent = await request.Content.ReadAsStringAsync();
            if (request.IsSuccessStatusCode)
            {
                if (authResult)
                {
                    Console.WriteLine("auth result = " + authResult);
                    var result = await request.Content.ReadAsStringAsync();
                    return Ok(result);
                }
                return Unauthorized();
                
            }
            if (request.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine("No user found.");
                return NotFound(responseContent);
                
            }
            return BadRequest(responseContent);
        }
        
        [HttpGet("Auth")]
        public async Task<IActionResult> Auth()
        {
            var AuthClient = new HttpClient();
            var AuthRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://172.19.0.1:3001/Identity/Authenticate");
            var authRequest = await AuthClient.SendAsync(AuthRequestMessage);
            var authResult = bool.Parse(await authRequest.Content.ReadAsStringAsync());
            return Ok(authResult);
        }

    }
}
