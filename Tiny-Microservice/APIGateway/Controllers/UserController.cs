using APIGateway.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace APIGateway.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
    [HttpPost("CreateUser")]
    public async Task<IActionResult> PostUser(PostUserDTO userDto)
    {
        var client = new HttpClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post,"http://localhost:3003/User/JANSKRIVNOGETHER");
        requestMessage.Content = new StringContent(JsonSerializer.Serialize(userDto));

        var request = await client.SendAsync(requestMessage);
        if(request.EnsureSuccessStatusCode() != null)
        {
            return Ok();
        }
        return BadRequest();
    }

    [HttpPut("EditUser")]
    public async Task<IActionResult> EditUser(EditUserDTO userDto)
    {
        var AuthClient = new HttpClient();
        var AuthRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost:3001/Identity/Authenticate");

        var client = new HttpClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, "http://localhost:3003/User/EditUser");
        requestMessage.Content = new StringContent(JsonSerializer.Serialize(userDto));

        var authRequest = await AuthClient.SendAsync(AuthRequestMessage);
        var authResult = bool.Parse(await authRequest.Content.ReadAsStringAsync());
        if (authResult)
        {
            var request = await client.SendAsync(requestMessage);

            if (request.EnsureSuccessStatusCode() != null)
            {
                return Ok();
            }
        }
        else 
        { 
            return Unauthorized();
        }
        return BadRequest();
    }

    [HttpPut("EditUserSettings")]
    public async Task<IActionResult> EditSettings(UserSettingsDTO userDto, string Id)
    {
        var AuthClient = new HttpClient();
        var AuthRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost:3001/Identity/Authenticate");

        var authRequest = await AuthClient.SendAsync(AuthRequestMessage);
        var authResult = bool.Parse(await authRequest.Content.ReadAsStringAsync());
        if (authResult)
        {
            var client = new HttpClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"http://localhost:3003/User/EditUserSettings?Id={Id}");
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(userDto));

            var request = await client.SendAsync(requestMessage);
            if (request.EnsureSuccessStatusCode() != null)
            {
                return Ok();
            }
        }
        else 
        { 
            return Unauthorized(); 
        }
        return BadRequest();
    }
}

