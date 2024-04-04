using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Search.Entities;
using SharedModels;
using Services;
using System.Text.Json;

namespace Search.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchUserController : Controller
{
    private readonly SearchService _searchService;

    public SearchUserController()
    {
        _searchService = new SearchService();
    }

    // GET: SearchUserController/
    [HttpGet("FindUser")]
    public async Task<IActionResult> FindUser(string searchString)
    {
        try
        {
            List<UserSimpleDTO> usersFromSearch = new List<UserSimpleDTO>();

            usersFromSearch = await _searchService.FindUser(searchString);
            Console.WriteLine("Found " + usersFromSearch.Count + " users in cache:");
            foreach (var user in usersFromSearch)
            {
                Console.WriteLine("name: " + user.name + " - id: " + user.id); 
            }

            if (usersFromSearch == null || usersFromSearch.Count == 0)
            {
                Console.WriteLine("Could not find user in cache, contacting user service...");
                using (var httpClient = new HttpClient())
                {
                    string apiUrl = "http://172.19.0.1:3003/User";
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl + "?userName=" + searchString);

                    if (response.IsSuccessStatusCode)
                    {
                        // Deserialize the response content to get the users
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        List<UserSimpleDTO> newUsers = JsonSerializer.Deserialize<List<UserSimpleDTO>>(jsonResponse);

                        if (newUsers != null && newUsers.Count > 0)
                        {
                            Task addUserTask = Task.Run(async () =>
                            {
                                foreach (var user in newUsers)
                                {
                                    _searchService.AddUserAsync(user);
                                }
                            });
                            addUserTask.Start();

                            return Ok(newUsers);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Response does not have a successful status code: " + response.StatusCode);
                        return NotFound("No such user found from UserService");
                    }
                }
            }

            return Ok(usersFromSearch);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

