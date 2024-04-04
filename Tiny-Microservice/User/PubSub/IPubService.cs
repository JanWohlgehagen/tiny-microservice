using SharedModels;

namespace User.PubSub;

public interface IPubService
{
    void newUser(UserFullDTO user);


    void updateUser(UserFullDTO user);
}