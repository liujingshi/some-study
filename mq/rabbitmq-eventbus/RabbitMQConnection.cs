using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace rabbitmq_eventbus
{
    public class RabbitMQConnection : IDisposable
    {
        // 连接工厂
        private readonly ConnectionFactory _connectionFactory;
        // 连接实例
        private IConnection _connection;
        // 关闭连接控制器
        private System.Timers.Timer _closeConnectionController;
        // 连接空闲开始时间
        private DateTime? _connnectionFreeStartTime;

        // 信道池
        private List<RabbitMQChannel> _channels;

        // 连接是否打开（手动关闭才算关闭，否则连接会自动重新连接不算关闭）
        public bool IsConnected { get; private set; }


        public RabbitMQConnection(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            OpenConnect();
            _channels = new List<RabbitMQChannel>();

            _closeConnectionController = new System.Timers.Timer(1000 * 10);
            _closeConnectionController.Elapsed += CloseConnectionTimer;
            _closeConnectionController.Start();
        }

        /// <summary>
        /// 使用连接工厂创建连接实例
        /// </summary>
        /// <returns></returns>
        private IConnection CreateConnection()
        {
            try
            {
                return _connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException e)
            {
                Thread.Sleep(2000);
                return CreateConnection();
            }
        }

        /// <summary>
        /// 连接关闭定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CloseConnectionTimer(object? sender, System.Timers.ElapsedEventArgs args)
        {
            if (_channels.FindAll(channel => channel.IsUsing == true).Count > 0)
            {
                _connnectionFreeStartTime = null;
                return;
            }
            if (!_connnectionFreeStartTime.HasValue)
            {
                _connnectionFreeStartTime = DateTime.Now;
                return;
            }
            if ((DateTime.Now - _connnectionFreeStartTime).Value.TotalSeconds >= 60 * 60 * 24 * 3)
            {
                CloseConnect();
            }
        }

        /// <summary>
        /// 开启连接
        /// </summary>
        public void OpenConnect()
        {
            if (_connection != null) _connection.Dispose();
            _connection = CreateConnection();
            IsConnected = true;
            _closeConnectionController.Start();
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseConnect()
        {
            _closeConnectionController.Close();
            _connection.Close();
            IsConnected = false;
        }
                
        /// <summary>
        /// 获取信道
        /// </summary>
        /// <returns></returns>
        public RabbitMQChannel GetChannel()
        {
            // 如果连接关闭
            if (IsConnected == false) OpenConnect();
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
            // 如果信道已经关闭了 开启信道
            if (channel.Channel.IsClosed) channel.OpenChannel(_connection);
            return channel;
        }

        public void Dispose()
        {
            CloseConnect();
            _connection.Dispose();
        }
    }
}
