using Confluent.Kafka;

namespace KafkaTest.Kafka.Consumers
{
    public class KafkaConsumer : IKafkaConsumer
    {
        private readonly IConsumer<string, string> kafkaConsumer;

        public KafkaConsumer(string bootstrapServers, string groupId)
        {
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId
            };

            kafkaConsumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        }

        public void StartConsuming(string topic)
        {
            kafkaConsumer.Subscribe(topic);

            while (true)
            {
                try
                {
                    var consumeResult = kafkaConsumer.Consume(CancellationToken.None);

                    Console.WriteLine($"Received message: {consumeResult.Offset}");
                    Console.WriteLine($"Received message: {consumeResult.Message.Key}");
                    Console.WriteLine($"Received message: {consumeResult.Message.Value}");
                }
                catch (ConsumeException ex)
                {
                    Console.WriteLine($"Error consuming message: {ex.Error.Reason}");
                }
            }
        }

        public void Dispose()
        {
            kafkaConsumer?.Dispose();
        }
    }
}
