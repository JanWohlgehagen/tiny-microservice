namespace APIGateway.Models
{
    public class UserSettingsDTO
    {
        public string theme { get; set; }
        public string language { get; set; }
        public bool showEmail { get; set; }
        public bool showPhoneNumber { get; set; }
        public bool showAddress { get; set; }
        public bool showBio { get; set; }
        public bool showCity { get; set; }
        public UserSettingsDTO(string theme, string language, bool showEmail, bool showPhoneNumber, bool showAddress, bool showBio, bool showCity)
        {
            this.theme = theme;
            this.language = language;
            this.showEmail = showEmail;
            this.showPhoneNumber = showPhoneNumber;
            this.showAddress = showAddress;
            this.showBio = showBio;
            this.showCity = showCity;
        }
    }
}
