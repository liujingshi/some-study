using EasyNetQ;
using Messages;

namespace Publisher;

public class Program
{
    public static void Main(string[] args)
    {
        using (var bus = RabbitHutch.CreateBus("" +
            "host=localhost:5672;" +
            "virtualHost=/;" +
            "username=guest;" +
            "password=guest;" +
            "requestedHeartbeat=60;" + // 心跳
            "prefetchcount=50;" + // 消费者可以同时取的消息数量
            "publisherConfirms=false;" + // 发布方确认
            "persistentMessages=true;" + // MQ 是否持久化消息
            "product=PublisherService;" + // 产品名称（显示在 RabbitMQ 管理界面中的）
            "platform=Publisher;" + // 平台名称（现在在 RabbitMQ 中的）
            "timeout=10")) // 超时时间 s
        {
            var input = String.Empty;
            Console.WriteLine("输入一个消息. 输入 'Quit' 退出.");
            while ((input = Console.ReadLine()) != "Quit")
            {
                if (input == null) continue;
                bus.PubSub.Publish(new TextMessage(input));
                Console.WriteLine("消息发布成功!");
            }
        }
    }
}