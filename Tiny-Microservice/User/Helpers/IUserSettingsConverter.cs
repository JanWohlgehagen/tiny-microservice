using SharedModels;

namespace User.Helpers;

public interface IUserSettingsConverter
{
    Models.UserSettings ConvertToUserSettings(UserSettingsDTO userSettings);
    Models.UserSettings ConvertToNewUserSettings(string userId);
}