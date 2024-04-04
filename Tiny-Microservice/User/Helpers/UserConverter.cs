using SharedModels;

namespace User.Helpers
{
    public class UserConverter :IUserConverter
    {
        public Models.User ConvertToNewUser(UserFullDTO user)
        {
            var id = Guid.NewGuid().ToString();
            return new Models.User()
            {
                id = id, name = user.name, imageURL = user.imageURL, email = user.email, phoneNumber = user.phoneNumber,
                address = user.address, city = user.city, country = user.country, postalCode = user.postalCode,
                about = user.about
            };
        }
        
        public UserFullDTO ConvertToUserFullDTO(Models.User user)
        {
            return new UserFullDTO()
            {
                id = user.id, name = user.name, imageURL = user.imageURL, email = user.email, phoneNumber = user.phoneNumber,
                address = user.address, city = user.city, country = user.country, postalCode = user.postalCode,
                about = user.about
            };
        }
        
        public Models.User ConvertToUser(UserFullDTO user)
        {
            return new Models.User()
            {
                id = user.id, name = user.name, imageURL = user.imageURL, email = user.email, phoneNumber = user.phoneNumber,
                address = user.address, city = user.city, country = user.country, postalCode = user.postalCode,
                about = user.about
            };
        }
    }
}
