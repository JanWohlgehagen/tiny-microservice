using Microsoft.AspNetCore.Mvc;
using SharedModels;
using User.Data;
using User.Helpers;
using System.Linq;
using User.PubSub;

namespace User.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly Context _context;
        private readonly IUserConverter _userConverter;
        private readonly IPubService _pubService;
        private readonly IUserSettingsConverter _userSettingsConverter;

        public UserController(ILogger<UserController> logger, Context context, IUserConverter userConverter, IPubService pubService, IUserSettingsConverter userSettingsConverter)
        {
            _logger = logger;
            _context = context;
            _userConverter = userConverter;
            _userSettingsConverter = userSettingsConverter;
            _pubService = pubService;
        }

        [HttpPost(Name = "PostUser")]
        public async Task<IActionResult> Post(UserFullDTO user)
        {   Console.WriteLine("I was hit, help!");
            //convert DTO to user
            Models.User newUser = _userConverter.ConvertToNewUser(user);
            Models.UserSettings newUserSettings = _userSettingsConverter.ConvertToNewUserSettings(newUser.id);
            //Add user to database
            _context.Users.Add(newUser);
            _context.UserSettings.Add(newUserSettings);
               
            //Save Changes in DB
            await _context.SaveChangesAsync();

           
            UserFullDTO newUserDTO = _userConverter.ConvertToUserFullDTO(newUser);
            _pubService.newUser(newUserDTO);
            // Insertion was successful
            return Ok("User inserted successfully.");
            
            // else
            // {
            //     _logger.LogError("ERROR | UserController | Failed to insert user.");
            //     return BadRequest("Failed to insert user. Try again later.");
            //
            // }
        }

        [HttpPut(Name = "PutUser")]
        public async Task<IActionResult> Put(UserFullDTO user)
        {
            //convert DTO to user
            Models.User updatedUser = _userConverter.ConvertToUser(user);
            //Update user in database
            _context.Users.Update(updatedUser);

            //Save changes in DB
            await _context.SaveChangesAsync();

            if (_context.ChangeTracker.HasChanges())
            {
                _pubService.updateUser(user);
                // Insertion was successful
                return Ok("User inserted successfully.");
            }
            else
            {
                _logger.LogError("ERROR | UserController | Failed to update user.");
                return BadRequest("Failed to update user. Try again later.");
            }
        }
        
        [HttpGet(Name = "GetUser")]
        public async Task<IActionResult> GetUser(string userName)
        {
            IEnumerable<Models.User> users = _context.Users.Where(user => user.name.Contains(userName));
    
            if (!users.Any())
            {
                _logger.LogError("INFO | UserController | No users found with name containing: {userName}", userName);
                return BadRequest("No users found.");
            }
    
            return Ok(users);
        }
    }
}
