using Microsoft.AspNetCore.Mvc;
using User.Data;
using User.Helpers;
using User.Models;

namespace User.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly Context _context;
        private readonly UserConverter _userConverter;

        public UserController(ILogger<UserController> logger, Context context, UserConverter userConverter)
        {
            _logger = logger;
            _context = context;
            _userConverter = userConverter;
        }

        [HttpPost(Name = "PostUser")]
        public async Task<IActionResult> Post(PostUserDTO user)
        {   
            //convert DTO to user
            TweeterUser tweeterUser = _userConverter.ConvertToNewUser(user);
            //Add user to database
            _context.Users.Add(tweeterUser);

            //Create user settings with user ID
            UserSettings userSettings = new UserSettings();
            userSettings.userId = tweeterUser.Id;
               
            //Save Changes in DB
            await _context.SaveChangesAsync();

            if (_context.ChangeTracker.HasChanges())
            {
                // Insertion was successful
                return Ok("User inserted successfully.");
            }
            else
            {
                _logger.LogError("ERROR | UserController | Failed to insert user.");
                return BadRequest("Failed to insert user. Try again later.");

            }
        }

        [HttpPut(Name = "PutUser")]
        public async Task<IActionResult> Put(EditUserDTO user)
        {
            //convert DTO to user
            TweeterUser tweeterUser = _userConverter.ConvertToUser(user);
            //Update user in database
            _context.Users.Update(tweeterUser);

            //Save changes in DB
            await _context.SaveChangesAsync();

            if (_context.ChangeTracker.HasChanges())
            {
                // Insertion was successful
                return Ok("User inserted successfully.");
            }
            else
            {
                _logger.LogError("ERROR | UserController | Failed to update user.");
                return BadRequest("Failed to update user. Try again later.");

            }
        }
    }
}
