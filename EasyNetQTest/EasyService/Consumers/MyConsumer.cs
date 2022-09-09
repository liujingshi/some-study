using EasyNetQ.AutoSubscribe;
using Messages;

namespace EasyService.Consumers;

public class MyConsumer : IConsumeAsync<MessageA>, IConsumeAsync<MessageB>, IConsumeAsync<MessageC>, IConsumeAsync<MessageD>, IConsumeAsync<MessageE>
{
    [AutoSubscriberConsumer(SubscriptionId = "MessageA-Consumer")]
    public async Task ConsumeAsync(MessageA message, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("[{0}] 开始消费 MessageA： {1}", DateTime.Now.ToLocalTime(), message.Text);
        await Task.Delay(800);
        Console.WriteLine("[{0}] 消费完成 MessageA： {1}", DateTime.Now.ToLocalTime(), message.Text);
    }

    [AutoSubscriberConsumer(SubscriptionId = "MessageB-Consumer")]
    public async Task ConsumeAsync(MessageB message, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("[{0}] 开始消费 MessageB： {1}", DateTime.Now.ToLocalTime(), message.Text);
        await Task.Delay(1200);
        Console.WriteLine("[{0}] 消费完成 MessageB： {1}", DateTime.Now.ToLocalTime(), message.Text);
    }

    [AutoSubscriberConsumer(SubscriptionId = "MessageC-Consumer")]
    public async Task ConsumeAsync(MessageC message, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("[{0}] 开始消费 MessageC： {1}", DateTime.Now.ToLocalTime(), message.Text);
        await Task.Delay(400);
        Console.WriteLine("[{0}] 消费完成 MessageC： {1}", DateTime.Now.ToLocalTime(), message.Text);
    }

    [AutoSubscriberConsumer(SubscriptionId = "MessageD-Consumer")]
    public async Task ConsumeAsync(MessageD message, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("[{0}] 开始消费 MessageD： {1}", DateTime.Now.ToLocalTime(), message.Text);
        await Task.Delay(200);
        Console.WriteLine("[{0}] 消费完成 MessageD： {1}", DateTime.Now.ToLocalTime(), message.Text);
    }

    [AutoSubscriberConsumer(SubscriptionId = "MessageE-Consumer")]
    public async Task ConsumeAsync(MessageE message, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("[{0}] 开始消费 MessageE： {1}", DateTime.Now.ToLocalTime(), message.Text);
        await Task.Delay(5000);
        Console.WriteLine("[{0}] 消费完成 MessageE： {1}", DateTime.Now.ToLocalTime(), message.Text);
    }
}
