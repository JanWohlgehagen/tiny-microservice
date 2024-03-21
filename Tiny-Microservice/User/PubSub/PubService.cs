using SharedModels;

namespace User.PubSub;

public class PubService
{
    public void newUser(UserFullDTO user)
    {
        var connectionEstablished = false;


        try
        {
            var bus = Client.GetRMQConnection();

            bus.PubSub.PublishAsync(user, x => x.WithTopic("createUserResult"));

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    
    public void updateUser(UserFullDTO user)
    {
        var connectionEstablished = false;

        try
        {
            var bus = Client.GetRMQConnection();

            bus.PubSub.PublishAsync(user, x => x.WithTopic("updateUserResult"));

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}