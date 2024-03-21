using SharedModels;

namespace User.Helpers;

public class UserSettingsConverter
{
    public Models.UserSettings ConvertToNewUserSettings(string userId)
    {
        var id = new Guid().ToString();
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