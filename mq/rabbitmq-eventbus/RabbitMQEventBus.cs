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

        /// <summary>
        /// 订阅消息
        /// </summary>
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

        /// <summary>
        /// 发布消息
        /// </summary>
        public void Publish()
        {
            var channel = _connection.GetChannel();
            channel.IsUsing = true;
            CreateExchangeAndQueue(channel);

            // 内容
            var body = JsonSerializer.SerializeToUtf8Bytes(new { Name = "B" });

            // 回调
            channel.Channel.BasicAcks += (sender, e) =>
            {
                // 消息已送达 信道停止使用
                channel.IsUsing = false;
            };
            channel.Channel.BasicNacks += (sender, e) =>
            {
                // 消息未送达
                // ... 重新发送消息
                channel.Channel.BasicPublish("et", "DriverAddSuccessEvent", true, null, body);
            };

            channel.Channel.BasicPublish("et", "DriverAddSuccessEvent", true, null, body);
        }
    }
}