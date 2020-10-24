using RabbitMQ.Client.Events;

namespace SquidLogParser.RabbitMq
{
    public class QueueConsumer
    {
        public QueueConsumer()
        {
            var consumer = new AsyncEventingBasicConsumer(channel);
            
        }
    }
}