using System.Text;
using RabbitMQ.Client;

namespace producer1;

public class Program
{
    public static void Main(string[] args)
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

        channel.ExchangeDeclare(exchange: "exchange.fanout",
                                type: "fanout");

        while (true)
        {
            Console.Write("请输入要发送的消息：");
            var input = Console.ReadLine();
            if (input == null) continue;
            string message = input.ToString();
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "exchange.fanout",
                                 routingKey: args.Length > 0 ? args[0] : "",
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine("发送消息 '{0}' 到交换机 exchange.fanout", message);
        }
    }
}
