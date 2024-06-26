using SharedModels;

namespace User.Helpers;

public class UserSettingsConverter : IUserSettingsConverter
{
    public Models.UserSettings ConvertToNewUserSettings(string userId)
    {
        var id = Guid.NewGuid().ToString();
        return new Models.UserSettings()
        {
            id = id, userId = userId, theme = "light", language = "en"
        };
    }
    
    public Models.UserSettings ConvertToUserSettings(UserSettingsDTO userSettings)
    {
        return new Models.UserSettings()
        {
            id = userSettings.id, userId = userSettings.userId, theme = userSettings.theme, language = userSettings.language
        };
    }
    
}