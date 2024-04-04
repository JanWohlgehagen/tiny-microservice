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
    public class UserSettingsController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly Context _context;
        private readonly IUserSettingsConverter _userSettingsConverter;

        public UserSettingsController(ILogger<UserController> logger, Context context, IUserSettingsConverter userSettingsConverter)
        {
            _logger = logger;
            _context = context;
            _userSettingsConverter = userSettingsConverter;

        }

        [HttpPut(Name = "PutUserSettings")]
        public async Task<IActionResult> Put(UserSettingsDTO userSettings)
        {
            //convert DTO to user
            Models.UserSettings updatedUserSettings = _userSettingsConverter.ConvertToUserSettings(userSettings);
            //Update user in database
            _context.UserSettings.Update(updatedUserSettings);

            //Save changes in DB
            await _context.SaveChangesAsync();

            if (_context.ChangeTracker.HasChanges())
            {
                // Insertion was successful
                return Ok("User inserted successfully.");
            }
            _logger.LogError("ERROR | UserController | Failed to update user.");
            return BadRequest("Failed to update user. Try again later.");
        }
    }
}
