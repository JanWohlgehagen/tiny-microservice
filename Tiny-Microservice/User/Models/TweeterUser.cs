namespace User.Models
{
    public class TweeterUser
    {
        public string Id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string? profilePictureUrl { get; set;}
        public string? address { get; set; }
        public string? phoneNumber { get; set; }
        public string? bio { get; set; }
        public string? city { get; set; }

        public UserSettings userSettings { get; set; }
        public TweeterUser(string id, string username, string email, string profilePictureUrl, string address, string phoneNumber, string bio, string city)
        {
            this.Id = id;
            this.username = username;
            this.email = email;
            this.profilePictureUrl = profilePictureUrl;
            this.address = address;
            this.phoneNumber = phoneNumber;
            this.bio = bio;
            this.city = city;
        }

        public TweeterUser()
        {
        }
    }
}
