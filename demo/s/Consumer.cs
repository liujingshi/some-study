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
    private EventingBasicConsumer? consumer = null;

    public Consumer()
    {
        // 创建工厂
        connectionFactory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 25671,
            VirtualHost = "/",
            UserName = "guest",
            Password = "guest"
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
        consumer = new EventingBasicConsumer(channel);
        consumer.Received += Received;
        channel.BasicConsume(
            queue: QUEUE_NAME,
            autoAck: true,
            consumer: consumer
        );
    }

    private void Received(object? sender, BasicDeliverEventArgs args)
    {
        byte[] body = args.Body.ToArray();
        string message = Encoding.UTF8.GetString(body);
        string routingKey = args.RoutingKey;
        Console.WriteLine("0从 {0} 交换机 {1} 队列 {2} 路由键消费消息 {3}", EXCHANGE_NAME, QUEUE_NAME, routingKey, message);
    }

    public void Run()
    {
        Console.WriteLine("订阅成功，等待消息中... 按回车键退出");
        Console.ReadLine();
    }
}
