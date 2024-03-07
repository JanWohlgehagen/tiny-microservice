using EasyNetQ;
using System.Diagnostics;

namespace Identity.Messaging
{
    public class IdendityMessager
    {
        public async Task InitiateMessaging()
        {
            var connectionEstablished = false;
            using var bus = RabbitHutch.CreateBus("host=localhost;username=application;password=pepsi");
            while (!connectionEstablished)
            {
                var subscriptionResult = bus.PubSub
                    .SubscribeAsync<string>("RPS", e =>
                    {
                        bus.PubSub.PublishAsync(true);
                    })
                    .AsTask();

                await subscriptionResult.WaitAsync(CancellationToken.None);
                connectionEstablished = subscriptionResult.Status == TaskStatus.RanToCompletion;
                if (!connectionEstablished) Thread.Sleep(1000);
                
            }
            
        }
    }
}
