using System.Text.Json;
using RabbitMQ.Client.Events;

namespace rabbitmq_eventbus
{
    public class RabbitMQEventBus
    {
        private readonly RabbitMQConnection _connection;

        public RabbitMQEventBus(RabbitMQConnection connection)
        {
            _connection = connection;
            Subscribe();
        }

        private void CreateExchangeAndQueue(RabbitMQChannel channel)
        {
            channel.Channel.ExchangeDeclare("et", "direct", true, false, null);
            channel.Channel.QueueDeclare("qt", true, false, false, null);
            channel.Channel.QueueBind("qt", "et", "DriverAddSuccessEvent", null);
        }

        public void Subscribe()
        {
            var channel = _connection.GetChannel();
            channel.IsUsing = true;
            CreateExchangeAndQueue(channel);
            var consumer = new EventingBasicConsumer(channel.Channel);
            consumer.Received += (sender, args) =>
            {
                // ...
            };
            channel.Channel.BasicConsume("qt", true, "driver", false, false, null, consumer);
        }

        public void Publish()
        {
            var channel = _connection.GetChannel();
            channel.IsUsing = true;
            CreateExchangeAndQueue(channel);

            // 回调
            channel.Channel.BasicAcks += (sender, e) =>
            {
                // 消息已送达 信道停止使用
                channel.IsUsing = false;
            };
            channel.Channel.BasicAcks += (sender, e) =>
            {
                // 消息未送达
                // ... 重新发送消息
            };

            // 内容
            var body = JsonSerializer.SerializeToUtf8Bytes(new { Name="B" });

            channel.Channel.BasicPublish("et", "DriverAddSuccessEvent", true, null, body);


        }
    }
}