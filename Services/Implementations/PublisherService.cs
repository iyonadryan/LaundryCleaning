using Confluent.Kafka;
using LaundryCleaning.Common.Exceptions;
using LaundryCleaning.Common.Models.Entities;
using LaundryCleaning.Data;
using LaundryCleaning.Services.Interfaces;
using System.Text.Json;

namespace LaundryCleaning.Services.Implementations
{
    public class PublisherService : IPublisherService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<PublisherService> _logger;
        private readonly IConfiguration _configuration;

        public PublisherService(
            ApplicationDbContext dbContext
            ,ILogger<PublisherService> logger
            ,IConfiguration configuration)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                AllowAutoCreateTopics = true,
                Acks = Acks.All,
                MessageTimeoutMs = 10000
            };

            var topic = typeof(T).Name;
            var payload = JsonSerializer.Serialize(message);

            var entity = new SystemPublisher
            {
                Topic = topic,
                Payload = payload
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            try
            {
                _logger.LogInformation("Sending to topic {Topic}: {Payload}", topic, payload);
                var deliveryResult = await producer.ProduceAsync(topic, new Message<Null, string> { Value = payload }, cancellationToken);
                _logger.LogInformation($"Delivered message to, topic: {topic}, message: {deliveryResult.Value}");

                entity.IsPublished = true;
                entity.PublishedAt = DateTime.Now;
            }
            catch (ProduceException<Null, string> e)
            {
                entity.ErrorMessage = e.Error.Reason;
                throw new BusinessLogicException($"Delivery failed: {e.Error.Reason}");
            }
            catch (Exception ex)
            {
                entity.ErrorMessage = "Unexpected Kafka produce error";
                _logger.LogError(ex, "Unexpected Kafka produce error");
                throw new BusinessLogicException("Unexpected Kafka produce error");
            }

            producer.Flush(cancellationToken);

            await _dbContext._publisher.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync();
        }
    }
}
