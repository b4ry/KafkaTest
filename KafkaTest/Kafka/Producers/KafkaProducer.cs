using Confluent.Kafka;
using Newtonsoft.Json;

namespace KafkaTest.Kafka.Producers
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<string, string> kafkaProducer;

        public KafkaProducer(string bootstrapServers)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
            };

            kafkaProducer = new ProducerBuilder<string, string>(producerConfig).Build();
        }

        public void SendMessage<T>(string topic, string key, T message)
        {
            var serializedMessage = JsonConvert.SerializeObject(message);

            kafkaProducer.Produce(topic, new Message<string, string> { Key = key, Value = serializedMessage });
        }

        public void Dispose()
        {
            kafkaProducer?.Dispose();
        }
    }
}
