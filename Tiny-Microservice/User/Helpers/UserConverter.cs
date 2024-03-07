using User.Models;

namespace User.Helpers
{
    public class UserConverter
    {
        public TweeterUser ConvertToUser(EditUserDTO user)
        {
            return new TweeterUser(user.Id, user.username, user.email, user.profilePictureUrl, user.address, user.phoneNumber, user.bio, user.city);
            
        }

        public TweeterUser ConvertToNewUser(PostUserDTO user)
        {
            TweeterUser tweeterUser = new TweeterUser();
            tweeterUser.Id = Guid.NewGuid().ToString();
            tweeterUser.username = user.username;
            tweeterUser.email = user.email;
            return tweeterUser;

        }
    }
}
