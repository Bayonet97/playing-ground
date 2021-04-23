using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace kafka_producer
{
    public class KafkaProducerHostedService : IHostedService
    {
        private readonly ILogger<KafkaProducerHostedService> _logger;
        private IProducer<Null, string> _producer;

        public KafkaProducerHostedService(ILogger<KafkaProducerHostedService> logger)
        {
            _logger = logger;
            var config = new ProducerConfig()
            {
                // Set to the message broker's domain
                BootstrapServers = "localhost:29092"
            };
            // Create a producer with given types and config. 1st type param is key, 2nd is value.
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // This is test code that runs on start
            for (int i = 0; i < 10; i++)
            {
                var value = $"This is input from a client, input number: {i}";
                _logger.LogInformation(value);
                // Creates a message and pushes to kafka with topic 'message'
                await _producer.ProduceAsync("message", new Message<Null, string>()
                {
                    Value = value
                }, cancellationToken);
            }
            // Make sure the queue is empty, time out at 10 seconds.
            _producer.Flush(TimeSpan.FromSeconds(10));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Destroy the producer when done.
            _producer?.Dispose();
            return Task.CompletedTask;
        }
    }

}
