using System.Text;
using RabbitMQ.Client;
using System.Threading;


namespace producer1;

public class Program
{
    public static void Main()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5173,
            VirtualHost = "/",
            UserName = "guest",
            Password = "guest"
        };

        var connection = factory.CreateConnection();

        var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "exchange.ack.fanout",
                                type: "fanout",
                                durable: true,
                                autoDelete: false);

        while (true)
        {
            // Console.Write("请输入要发送的消息：");
            // var input = Console.ReadLine();
            // if (input == null) continue;
            // string message = input.ToString();
            string message = Guid.NewGuid().ToString();
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "exchange.ack.fanout",
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine("发送消息 '{0}' 到交换机 exchange.ack.fanout", message);

            Thread.Sleep(500);
        }
    }
}
