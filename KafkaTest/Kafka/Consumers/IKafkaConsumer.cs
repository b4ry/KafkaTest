namespace KafkaTest.Kafka.Consumers
{
    public interface IKafkaConsumer
    {
        public void StartConsuming(string topic);
    }
}
