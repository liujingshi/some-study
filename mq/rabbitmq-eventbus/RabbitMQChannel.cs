using RabbitMQ.Client;

namespace rabbitmq_eventbus
{
    public class RabbitMQChannel : IDisposable
    {
        private bool _isUsing;
        // 关闭信道控制器
        private System.Timers.Timer _closeChannelController;

        /// <summary>
        /// RabbitMQ 信道
        /// </summary>
        public IModel Channel { get; set; }

        /// <summary>
        /// 信道是否正在使用中
        /// </summary>
        public bool IsUsing 
        { 
            get { return _isUsing; }
            set 
            { 
                _isUsing = value;
                // 在通道开始使用时关闭定时器，在通道停止使用时打开定时器
                if (_isUsing == false)
                {
                    _closeChannelController.Start();
                }
                else
                {
                    _closeChannelController.Close();
                }
            }
        }

        public RabbitMQChannel()
        {
            _closeChannelController = new System.Timers.Timer(1000 * 60 * 60 * 1);
            _closeChannelController.Elapsed += (sender, e) => CloseChannel();
        }

        /// <summary>
        /// 开启通道
        /// </summary>
        public void OpenChannel(IConnection connection)
        {
            if (Channel.IsClosed)
            {
                Channel.Dispose();
                Channel = connection.CreateModel();
            }
        }

        /// <summary>
        /// 关闭通道
        /// </summary>
        public void CloseChannel()
        {
            _closeChannelController.Close();
            IsUsing = false;
            Channel.Close();
        }

        public void Dispose()
        {
            CloseChannel();
            Channel.Dispose();
        }
    }
}
