using EasyNetQ;

namespace User.PubSub;

public static class Client
{
    public static IBus GetRMQConnection()
    {
        return RabbitHutch.CreateBus("host=rmq;username=guest;password=guest");
    }
}