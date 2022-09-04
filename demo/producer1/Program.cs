using System.Text;
using RabbitMQ.Client;

namespace producer1;

public class Program
{
    public static void Main()
    {
        // 创建连接工厂
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            VirtualHost = "/",
            UserName = "guest",
            Password = "guest"
        };

        // 创建一个 TCP 连接
        var connection = factory.CreateConnection();

        // 在连接中创建一个新的信道
        var channel = connection.CreateModel();

        // 创建一个队列
        channel.QueueDeclare(queue: "queue1", // 队列名称
                             durable: true,   // 持久化
                             exclusive: false, // 只有声明它的连接能使用（连接关闭后队列会被删除）
                             autoDelete: false, // 没人订阅后自动删除
                             arguments: null); // 其它参数 key-value
        /**
         * x-expires                    Number  在自动删除队列之前，队列可以闲置多长时间(ms)
         * x-message-ttl                Number  发布到队列的消息在被丢弃之前可以存活多长时间
         * x-overflow                   String  设置队列溢出行为。这将确定当到达队列的最大长度时消息的处理情况。...
         * x-single-active-consumer     Boolean 每次只能有 1 个消费者消费
         * x-dead-letter-exchange       String  死信交换机，消息被拒绝和过期发到这里
         * x-dead-letter-routing-key    String  死信 RoutingKey，消息被拒绝和过期发到这里
         * x-max-length                 Number  设置队列最大可容纳消息数量
         * x-max-length-bytes           Number  设置队列最大可容纳消息大小
         * x-max-priority               Number  支持的最大优先级，不设置不支持优先级
         * x-queue-mode                 String  队列模式，可设置成 lazy，尽可能在磁盘保存，剩内存但速度慢了。如果不设置，内存满之前，内存磁盘都会保留，速度快
         * x-queue-version              Number  设置队列版本 1/2...
         * x-queue-master-locator       String
         */

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        while (true)
        {
            Console.Write("请输入要发送的消息：");
            var input = Console.ReadLine();
            if (input == null) continue;
            string message = input.ToString();
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: "queue1",
                                 basicProperties: properties,
                                 body: body);

            Console.WriteLine("发送消息 '{0}' 到队列 queue1", message);
        }
    }
}
