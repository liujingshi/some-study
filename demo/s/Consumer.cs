using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace s;

public class Consumer
{
    private readonly string EXCHANGE_NAME = "exchange.direct";
    private readonly string QUEUE_NAME = "exchange.direct.queue.q1";
    private readonly string ROUTING_KEY = "info";
    private readonly ConnectionFactory connectionFactory;
    private IConnection? connection = null;
    private IModel? channel = null;

    public Consumer()
    {
        // 创建工厂
        connectionFactory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            VirtualHost = "/",
            UserName = "guest",
            Password = "guest",
            DispatchConsumersAsync = true
        };
        // 开启连接
        CreateConnect();
        // 开启信道
        CreateChannel();
        // 创建队列
        CreateQueue();
        // 创建消费者
        CreateConsumer();
    }

    private void CreateConnect()
    {
        if (connectionFactory == null) return;
        connection = connectionFactory.CreateConnection();
    }

    private void CreateChannel()
    {
        if (connection == null) return;
        channel = connection.CreateModel();
    }

    private void CreateQueue()
    {
        if (channel == null) return;
        channel.QueueDeclare(
            queue: QUEUE_NAME,
            durable: true,
            autoDelete: false,
            arguments: null
        );
        channel.QueueBind(
            queue: QUEUE_NAME,
            exchange: EXCHANGE_NAME,
            routingKey: ROUTING_KEY,
            arguments: null
        );
    }

    private void CreateConsumer()
    {
        if (channel == null) return;
        if (connection == null) return;
        var consumer1 = new AsyncEventingBasicConsumer(channel);
        consumer1.Received += async (sender, args) => await AsyncReceived(sender, args, channel);
        channel.BasicConsume(
            queue: QUEUE_NAME,
            autoAck: false,
            consumer: consumer1
        );

        var channel1 = connection.CreateModel();
        var consumer2 = new AsyncEventingBasicConsumer(channel1);
        consumer2.Received += async (sender, args) => await AsyncReceived(sender, args, channel1);
        channel1.BasicConsume(
            queue: QUEUE_NAME,
            autoAck: false,
            consumer: consumer2
        );

        var channel2 = connection.CreateModel();
        var consumer3 = new AsyncEventingBasicConsumer(channel2);
        consumer3.Received += async (sender, args) => await AsyncReceived(sender, args, channel2);
        channel2.BasicConsume(
            queue: QUEUE_NAME,
            autoAck: false,
            consumer: consumer3
        );
    }

    private async Task AsyncReceived(object? sender, BasicDeliverEventArgs args, IModel channelx)
    {
        byte[] body = args.Body.ToArray();
        string message = Encoding.UTF8.GetString(body);
        Console.WriteLine("[{0}] 从 {1} 信道 得到消息 {2}", DateTime.Now.ToLongTimeString(), channelx.ChannelNumber, message);
        await Task.Delay(3000);
        Console.WriteLine("[{0}] 消息 {1} 消费完成", DateTime.Now.ToLongTimeString(), message);
        channelx.BasicAck(args.DeliveryTag, false);
    }


    public void Run()
    {
        Console.WriteLine("订阅成功，等待消息中... 按回车键退出");
        Console.ReadLine();
    }
}
