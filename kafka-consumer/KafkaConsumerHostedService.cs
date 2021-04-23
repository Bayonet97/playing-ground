using Kafka.Public;
using Kafka.Public.Loggers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace kafka_consumer
{
    public class KafkaConsumerHostedService : IHostedService
    {
        private readonly ILogger<KafkaConsumerHostedService> _logger;
        private ClusterClient _cluster;

        public KafkaConsumerHostedService(ILogger<KafkaConsumerHostedService> logger)
        {
            _logger = logger;
            _cluster = new ClusterClient(new Configuration
            {
                // Set to the message broker's domain
                Seeds = "localhost:29092"
            }, new ConsoleLogger());
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Consume messages if there are any of topic 'message'
            _cluster.ConsumeFromLatest("message");
            // Subscribe to MessageReceived from the Kafka library. Some magic here
            _cluster.MessageReceived += record =>
            {
                // Process each received record, received as a byte array
                _logger.LogInformation($"Received: {Encoding.UTF8.GetString(record.Value as byte[])}");
            };
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Dispose when done
            _cluster?.Dispose();
            return Task.CompletedTask;
        }
    }

}
