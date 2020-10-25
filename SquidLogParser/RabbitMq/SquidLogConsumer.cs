using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SquidLogParser.AccessLog;
using SquidLogParser.Data;
using SquidLogParser.Services;

namespace SquidLogParser.RabbitMq
{
    public class SquiqLogConsumer
    {
        private static readonly string ExchangeName = "logs";
        private static readonly string QueueName = "squid.access.logs";

        private static readonly string RoutingKey = "squid-access";
        private readonly ILogger _logger;

        private readonly IConnection _connection;

        private readonly IModel _channel;

        private readonly IAccessLogParser _accessLogParser;

        private readonly SquidLogContext _dbContext;
        
        public SquiqLogConsumer(ILogger<SquiqLogConsumer> logger, 
                             IConnectionFactory connectionFactory, 
                             IAccessLogParser accessLogParser,
                             SquidLogContext dbContext)
        {
            _logger = logger;            
            _accessLogParser = accessLogParser;
            _dbContext = dbContext;

            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            ConfigureListener(_channel);
        }

        private async Task LogReceived(string message)
        {
            await Task.Run(() => {

                try
                {
                    _logger.LogInformation("log entry: {0}", message);

                    var logEntry = _accessLogParser.ParseLog(message);
                    var accessLogEntry = LogService.BuildAccessLogEntry(logEntry);

                    _dbContext.AccessLogs.Add(accessLogEntry);
                    _dbContext.SaveChanges();

                    _logger.LogInformation("Log: {0} processed", message);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Can't process log entry: {0}", ex.Message);
                }
            });
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
                await LogReceived(message);
            };

            channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);                
        }
    }
}