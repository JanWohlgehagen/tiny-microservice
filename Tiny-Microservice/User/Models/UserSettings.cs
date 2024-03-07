namespace User.Models
{
    public class UserSettings
    {
        public string theme { get; set; } = "light";
        public string language { get; set; } = "en";
        public bool showEmail { get; set; } = true;
        public bool showPhoneNumber { get; set; } = true;
        public bool showAddress { get; set; } = true;
        public bool showBio { get; set; } = true;
        public bool showCity { get; set; } = true;

        public string userId { get; set; }
        public UserSettings(string userId, string theme, string language, bool showEmail, bool showPhoneNumber, bool showAddress, bool showBio, bool showCity)
        {
            this.theme = theme;
            this.language = language;
            this.showEmail = showEmail;
            this.showPhoneNumber = showPhoneNumber;
            this.showAddress = showAddress;
            this.showBio = showBio;
            this.showCity = showCity;
        }

        public UserSettings()
        {
        }
    }
}
