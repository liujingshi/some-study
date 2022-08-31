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

        channel.ExchangeDeclare(exchange: "exchange.topic",
                                type: "topic");

        string queueName = args.Length > 0 ? args[0] : "";

        if (queueName == "")
            queueName = channel.QueueDeclare().QueueName;
        else
            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

        if (args.Length > 1)
            for (int i = 1; i < args.Length; i++)
                channel.QueueBind(queue: queueName,
                              exchange: "exchange.topic",
                              routingKey: args[i]);
        else
            channel.QueueBind(queue: queueName,
                              exchange: "exchange.topic",
                              routingKey: "default");


        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var routingKey = args.RoutingKey;
            Console.WriteLine("从 exchange.topic 交换机 {0} 队列 {1} 路由键消费消息 {2}", queueName, routingKey, message);
        };

        channel.BasicConsume(queue: queueName,
                             autoAck: true,
                             consumer: consumer);

        Console.WriteLine("按任意键退出");
        Console.ReadLine();
    }
}
