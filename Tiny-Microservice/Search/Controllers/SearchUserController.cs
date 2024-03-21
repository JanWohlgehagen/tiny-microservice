using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Search.Entities;
using Search.DTOs;
using Search.Services;

namespace Search.Controllers
{
    public class SearchUserController : Controller
    {
        private readonly RedisUserService _redisUserService;

        public SearchUserController(RedisUserService redisUserService)
        {
            _redisUserService = redisUserService;
        }

        // GET: SearchUserController/Get
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Get(string searchString)
        {
            try
            {
                // Call the RedisUserService to get a list of users that match the search string
                List<User> Redisusers = await _redisUserService.SearchUsers(searchString);

                // or just call a list since this service will be running and it shouldnt be too hard on the ram for our limited database.
                List<User> users = new List<User>();

                if (users.Any())
                {
                    List<UserDto> userDtos = users.Select(user => new UserDto
                    {
                        username = user.username,
                        email = user.email,
                        profilePictureUrl = user.profilePictureUrl,
                        address = user.address,
                        phoneNumber = user.phoneNumber,
                        bio = user.bio,
                        city = user.city
                    }).ToList();

                    return Ok(userDtos);
                }
                else
                {
                    // No users found, return a 404 Not Found response
                    return NotFound();
                }
            }
            catch
            {
                // An error occurred while processing the request, return a 500 Internal Server Error response
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
