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
                List<User> users = await _redisUserService.SearchUsers(searchString);

                if (users.Any())
                {
                    // Map the list of users to a list of UserDto objects
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

                    // Return a 200 OK response with the list of UserDto objects in the response body
                    return Ok(userDtos);
                }
                else
                {
                    // Call the 'User' microservice to get a list of all users that match the search string
                    List<User> usersFromUserService = await _userService.GetUsersBySearchString(searchString);
                    // using RabbitMQ we need to call the 'User' microservice and get the users that arent found in the Redis cache.
                    // Then store them below as seen.
                    // We also need to return them to the Client hmm along with the ones there WERE found in Redis.

                    if (usersFromUserService.Any())
                    {
                        // Save the users to Redis
                        foreach (User user in usersFromUserService)
                        {
                            await _redisUserService.SaveUserToRedis(user);
                        }

                        // Map the list of users to a list of UserDto objects
                        List<UserDto> userDtos = usersFromUserService.Select(user => new UserDto
                        {
                            username = user.username,
                            email = user.email,
                            profilePictureUrl = user.profilePictureUrl,
                            address = user.address,
                            phoneNumber = user.phoneNumber,
                            bio = user.bio,
                            city = user.city
                        }).ToList();

                        // Return a 200 OK response with the list of UserDto objects in the response body
                        return Ok(userDtos);
                    }
                    else
                    {
                        // No users found, return a 404 Not Found response
                        return NotFound();
                    }
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
