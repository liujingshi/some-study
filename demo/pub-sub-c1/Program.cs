using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace consumer1;

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

        string queueName = args.Length > 0 ? args[0] : "";

        if (queueName == "")
            queueName = channel.QueueDeclare().QueueName;
        else
            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

        channel.QueueBind(queue: queueName,
                          exchange: "exchange.fanout",
                          routingKey: "");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine("从 exchange.fanout 交换机 {0} 队列消费消息 {1}", queueName, message);
        };

        channel.BasicConsume(queue: queueName,
                             autoAck: true,
                             consumer: consumer);

        Console.WriteLine("按任意键退出");
        Console.ReadLine();
    }
}
