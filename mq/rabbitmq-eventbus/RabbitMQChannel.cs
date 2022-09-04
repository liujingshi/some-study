using RabbitMQ.Client;

namespace rabbitmq_eventbus
{
    public class RabbitMQChannel : IDisposable
    {
        /// <summary>
        /// RabbitMQ 信道
        /// </summary>
        public IModel Channel { get; set; }

        /// <summary>
        /// 信道是否正在使用中
        /// </summary>
        public bool IsUsing { get; set; }

        public void Dispose()
        {
            Channel.Dispose();
        }
    }
}
