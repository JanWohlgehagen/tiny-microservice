using System.Text;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using System.Text.Json;

namespace APIGateway.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
    [HttpPost("CreateUser")]
    public async Task<IActionResult> PostUser(UserFullDTO userDto)
    {
        var client = new HttpClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://172.18.0.1:3003/User");
        requestMessage.Content = new StringContent(JsonSerializer.Serialize(userDto), Encoding.UTF8, "application/json");
        
        Console.Write( await requestMessage.Content.ReadAsStringAsync());

        var request = await client.SendAsync(requestMessage);
        if(request.EnsureSuccessStatusCode() != null)
        {
            return Ok();
        }
        return BadRequest();
    }

    [HttpPut("EditUser")]
    public async Task<IActionResult> EditUser(UserFullDTO userDto)
    {
        var AuthClient = new HttpClient();
        var AuthRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://172.21.0.1:3001/Identity/Authenticate");

        var client = new HttpClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, "http://172.21.0.1:3003/User");
        requestMessage.Content = new StringContent(JsonSerializer.Serialize(userDto), Encoding.UTF8, "application/json");

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

    [HttpGet("Exist")]
    public string IExist()
    {
        return "Jan er så smuk <3";
    }
    
    [HttpPut("EditUserSettings")]
    public async Task<IActionResult> EditSettings(UserSettingsDTO userDto, string Id)
    {
        var AuthClient = new HttpClient();
        var AuthRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://172.21.0.1:3001/Identity/Authenticate");

        var authRequest = await AuthClient.SendAsync(AuthRequestMessage);
        var authResult = bool.Parse(await authRequest.Content.ReadAsStringAsync());
        if (authResult)
        {
            var client = new HttpClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"http://172.18.0.1:3003/UserSettings?Id={Id}");
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(userDto), Encoding.UTF8, "application/json");

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

