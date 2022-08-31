using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace consumer1;

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

        channel.QueueDeclare(queue: "q1",
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine("从 q1 队列消费消息 {0}", message);
        };

        channel.BasicConsume(queue: "q1",
                             autoAck: true,
                             consumer: consumer);

        Console.WriteLine("按任意键退出");
        Console.ReadLine();
    }
}
