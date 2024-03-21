using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Search.Entities;
using Search.Services;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Search.BackgroundServices
{
    public class UserUpdateConsumer : BackgroundService
    {
        private readonly IModel _rabbitMqChannel;
        private readonly RedisUserService _redisUserService;
        private const string ExchangeName = "user_updates"; // Replace with the name of your RabbitMQ exchange
        private const string QueueName = "user_updates_queue"; // Replace with the name of your RabbitMQ queue
        private string? _consumerTag = null;


        public UserUpdateConsumer(IModel rabbitMqChannel, RedisUserService redisUserService)
        {
            _rabbitMqChannel = rabbitMqChannel;
            _redisUserService = redisUserService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Declare and bind the queue to the exchange
            _rabbitMqChannel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _rabbitMqChannel.QueueBind(queue: QueueName, exchange: ExchangeName, routingKey: "");

            // Create a consumer and start consuming messages
            var consumer = new EventingBasicConsumer(_rabbitMqChannel);
            consumer.Received += async (model, ea) =>
            {
                // Deserialize the message body to a User object
                var messageBody = ea.Body.ToArray();
                var user = JsonConvert.DeserializeObject<User>(Encoding.UTF8.GetString(messageBody));

                // Update the Redis cache with the new user data
                if (user != null)
                {
                    await _redisUserService.SaveUserToRedis(user);
                }

                // Acknowledge the message
                _rabbitMqChannel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            _consumerTag = _rabbitMqChannel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

            // Wait for the consumer to start consuming messages
            while (string.IsNullOrEmpty(_consumerTag))
            {
                await Task.Delay(100, stoppingToken);
            }
        }
    }
}
