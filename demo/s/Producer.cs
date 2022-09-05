using RabbitMQ.Client;
using System.Text;

namespace s;

public class Producer
{
    private readonly string EXCHANGE_NAME = "exchange.direct";
    private readonly ConnectionFactory connectionFactory;
    private IConnection? connection = null;
    private IModel? channel = null;

    public Producer()
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
        this.CreateConnect();
        // 开启信道
        this.CreateChannel();
        // 创建交换机
        this.CreateExchange();
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

    private void CreateExchange()
    {
        if (channel == null) return;
        channel.ExchangeDeclare(
            exchange: EXCHANGE_NAME,
            type: "direct",
            durable: true,
            autoDelete: false,
            arguments: null
        );
    }

    public void Publish(string routingKey, string message)
    {
        byte[] body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(
            exchange: EXCHANGE_NAME,
            routingKey: routingKey,
            basicProperties: null,
            body: body
        );
        Console.WriteLine("发送消息 {0} 到交换机 {1} 的 {2} 路由键", message, EXCHANGE_NAME, routingKey);
    }

    public void Run()
    {
        while (true)
        {
            Console.Write("请输入要发送的路由Key：");
            var inputRoutingKey = Console.ReadLine();
            if (inputRoutingKey == null) continue;
            Console.Write("请输入要发送的消息：");
            var input = Console.ReadLine();
            if (input == null) continue;
            string routingKey = inputRoutingKey.ToString();
            string message = input.ToString();
            Publish(routingKey, message);
        }
    }
}
