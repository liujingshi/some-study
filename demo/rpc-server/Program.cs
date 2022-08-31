using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace rpc_server;

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

        channel.QueueDeclare(queue: "rpc_queue",
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        channel.BasicQos(0, 1, false);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender, args) =>
        {
            string response = "";

            var body = args.Body.ToArray();
            var props = args.BasicProperties;
            var replyProps = channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            try
            {
                var message = Encoding.UTF8.GetString(body);
                int n = int.Parse(message);
                Console.WriteLine("Fib({0})", message);
                response = Fib(n).ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response = "";
            }
            finally
            {
                var responseBytes = Encoding.UTF8.GetBytes(response);

                channel.BasicPublish(exchange: "", 
                                     routingKey: props.ReplyTo,
                                     basicProperties: replyProps, 
                                     body: responseBytes);

                channel.BasicAck(deliveryTag: args.DeliveryTag,
                                 multiple: false);
            }
        };

        channel.BasicConsume(queue: "rpc_queue",
                             autoAck: false,
                             consumer: consumer);

        Console.WriteLine("按任意键退出");
        Console.ReadLine();
    }

    private static int Fib(int n)
    {
        if (n == 0 || n == 1)
        {
            return n;
        }

        return Fib(n - 1) + Fib(n - 2);
    }
}
