using SharedModels;

namespace User.Helpers;

public interface IUserConverter
{
    Models.User ConvertToNewUser(UserFullDTO user);
    UserFullDTO ConvertToUserFullDTO(Models.User user);
    Models.User ConvertToUser(UserFullDTO user);
}