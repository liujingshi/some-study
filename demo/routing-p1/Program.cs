using System.Text;
using RabbitMQ.Client;

namespace producer1;

public class Program
{
    public static void Main()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            VirtualHost = "/",
            UserName = "guest",
            Password = "guest"
        };

        var connection = factory.CreateConnection();

        var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "exchange.direct",
                                type: "direct");

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
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "exchange.direct",
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine("发送消息 {0} 到交换机 exchange.direct 的 {1} 路由键", message, routingKey);
        }
    }
}
