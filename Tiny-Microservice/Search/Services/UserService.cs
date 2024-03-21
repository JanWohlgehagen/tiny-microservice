using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Search.Entities;

namespace UserService.Services
{
    public class UserService
    {
        private readonly IModel _rabbitMqChannel;
        private const string ExchangeName = "user_updates"; // Replace with the name of your RabbitMQ exchange

        public UserService(IModel rabbitMqChannel)
        {
            _rabbitMqChannel = rabbitMqChannel;
        }

        public void CreateUser(User user)
        {
            // Save the user to the database
            // ...

            // Publish a message to the RabbitMQ exchange
            var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(user));
            _rabbitMqChannel.BasicPublish(exchange: ExchangeName, routingKey: "", body: messageBody);
        }

        public void UpdateUser(User user)
        {
            // Update the user in the database
            // ...

            // Publish a message to the RabbitMQ exchange
            var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(user));
            _rabbitMqChannel.BasicPublish(exchange: ExchangeName, routingKey: "", body: messageBody);
        }
    }
}
