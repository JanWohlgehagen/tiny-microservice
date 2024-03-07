namespace User.Models
{
    public class EditUserDTO
    {
        public string Id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string profilePictureUrl { get; set; }
        public string address { get; set; }
        public string phoneNumber { get; set; }
        public string bio { get; set; }
        public string city { get; set; }

        public EditUserDTO(string Id, string username, string email, string profilePictureUrl, string address, string phoneNumber, string bio, string city)
        {
            this.Id = Id;
            this.username = username;
            this.email = email;
            this.profilePictureUrl = profilePictureUrl;
            this.address = address;
            this.phoneNumber = phoneNumber;
            this.bio = bio;
            this.city = city;
        }

    }
}
