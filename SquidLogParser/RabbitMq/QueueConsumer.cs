using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SquidLogParser.RabbitMq
{
    public class QueueConsumer
    {
        private static readonly string ExchangeName = "logs";
        private static readonly string QueueName = "squid.access.logs";

        private static readonly string RoutingKey = "squid-access";
        private readonly ILogger _logger;

        private readonly IConnection _connection;

        private readonly IModel _channel;

        
        public QueueConsumer(ILogger<QueueConsumer> logger, IConnectionFactory connectionFactory)
        {
            _logger = logger;            

            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            ConfigureListener(_channel);
        }

        private async Task MessageReceived(string message)
        {
            _logger.LogInformation("message: {0}", message);
        }

        private void ConfigureListener(IModel channel)
        {

            channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true, false);
            _logger.LogInformation("Declared exchange: {0}", ExchangeName);

            var queue = channel.QueueDeclare(QueueName, true, false, false);
            _logger.LogInformation("Declared queue: {0}", QueueName);

            channel.QueueBind(QueueName, ExchangeName, RoutingKey);
            _logger.LogInformation("Bind Queue: {0} with Exchange: {1} using RoutingKey: {2}", 
                QueueName, ExchangeName, RoutingKey);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await MessageReceived(message);
            };

            channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);                
        }
    }
}