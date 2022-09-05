using RabbitMQ.Client;

namespace rabbitmq_eventbus
{
    public class RabbitMQConnection : IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private List<RabbitMQChannel> _channels;

        public RabbitMQConnection(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _connection = _connectionFactory.CreateConnection();
            _channels = new List<RabbitMQChannel>();
        }

        /// <summary>
        /// 获取信道
        /// </summary>
        /// <returns></returns>
        public RabbitMQChannel GetChannel()
        {
            // 寻找已存在的信道中未使用的
            var channel = _channels.Find(channel => channel.IsUsing == false);
            // 如果都在使用 创建新的信道
            if (channel == null)
            {
                channel = new RabbitMQChannel
                {
                    Channel = _connection.CreateModel(),
                    IsUsing = false
                };
                _channels.Add(channel);
            }
            // 如果信道已经关闭了 销毁信道 重新创建
            if (channel.Channel.IsClosed)
            {
                channel.Channel.Dispose();
                channel.Channel = _connection.CreateModel();
            }
            return channel;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
