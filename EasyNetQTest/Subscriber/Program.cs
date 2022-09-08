using EasyNetQ;
using Messages;

namespace Subscriber;

public class Program
{
    public static void Main(string[] args)
    {
        using (var bus = RabbitHutch.CreateBus("" +
            "host=localhost:5672;" +
            "virtualHost=/;" +
            "username=guest;" +
            "password=guest;" +
            "requestedHeartbeat=60;" +
            "prefetchcount=50;" +
            "publisherConfirms=false;" +
            "persistentMessages=true;" +
            "product=SubscriberService;" +
            "platform=Subscriber;" +
            "timeout=10"))
        {
            bus.PubSub.Subscribe<TextMessage>("test", HandleTextMessage);
            Console.WriteLine("监听消息. Enter 退出.");
            Console.ReadLine();
        }
    }

    public static void HandleTextMessage(TextMessage textMessage)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("得到消息: {0}", textMessage.Text);
        Console.ResetColor();
    }

}