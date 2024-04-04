using SharedModels;
using Services;
using EasyNetQ;
using System.Reflection.PortableExecutable;

namespace CalculatorService.Communications
{
    public static class Subscriptions
    {
        private static IBus? _bus;
        private static SearchService? _searchService;

        public static void StartCreateUserSubscription(SearchService searchService)
        {
            _searchService = searchService;
            _bus = RabbitHutch.CreateBus("host=rmq;port=5672;virtualHost=/;username=guest;password=guest");

            var topic = "createUserResult";

            var subscription = _bus.PubSub.SubscribeAsync<UserFullDTO>("SearchService-" + Environment.MachineName, async (e, cancellationToken) =>
            {
                if (e != null)
                {
                    _searchService.HandleCreateUser(e);
                }
                else
                {
                    Console.WriteLine("Received null response from the message bus (createUserResult).");
                }
            }, configure => configure.WithTopic(topic));
            Console.WriteLine("createUserResult setup");
        }

        public static void StartUpdateUserSubscription(SearchService searchService)
        {
            _searchService = searchService;
            _bus = RabbitHutch.CreateBus("host=rmq;username=guest;password=guest");

            var topic = "updateUserResult";

            var subscription = _bus.PubSub.SubscribeAsync<UserFullDTO>("SearchService-" + Environment.MachineName, async (e, cancellationToken) =>
            {
                if (e != null)
                {
                    _searchService.HandleUpdateUser(e);
                }
                else
                {
                    Console.WriteLine("Received null response from the message bus (updateUserResult).");
                }
            }, configure => configure.WithTopic(topic));
            Console.WriteLine("updateUserResult setup");
        }
    }
}
