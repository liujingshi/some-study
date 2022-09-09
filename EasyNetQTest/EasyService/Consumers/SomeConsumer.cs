using EasyNetQ.AutoSubscribe;
using Messages;

namespace EasyService.Consumers;

public class SomeConsumer : IConsumeAsync<MessageA>, IConsumeAsync<MessageB>, IConsumeAsync<MessageC>, IConsumeAsync<MessageD>, IConsumeAsync<MessageE>
{
    [AutoSubscriberConsumer(SubscriptionId = "MessageA-SomeConsumer")]
    public async Task ConsumeAsync(MessageA message, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("[{0}] SomeConsumer 开始消费 MessageA： {1}", DateTime.Now.ToLocalTime(), message.Text);
        await Task.Delay(800);
        Console.WriteLine("[{0}] SomeConsumer  MessageA： {1}", DateTime.Now.ToLocalTime(), message.Text);
    }

    [AutoSubscriberConsumer(SubscriptionId = "MessageB-SomeConsumer")]
    public async Task ConsumeAsync(MessageB message, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("[{0}] SomeConsumer 开始消费 MessageB： {1}", DateTime.Now.ToLocalTime(), message.Text);
        await Task.Delay(1000);
        Console.WriteLine("[{0}] SomeConsumer 消费完成 MessageB： {1}", DateTime.Now.ToLocalTime(), message.Text);
    }

    [AutoSubscriberConsumer(SubscriptionId = "MessageC-SomeConsumer")]
    public async Task ConsumeAsync(MessageC message, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("[{0}] SomeConsumer 开始消费 MessageC： {1}", DateTime.Now.ToLocalTime(), message.Text);
        await Task.Delay(400);
        Console.WriteLine("[{0}] SomeConsumer 消费完成 MessageC： {1}", DateTime.Now.ToLocalTime(), message.Text);
    }

    [AutoSubscriberConsumer(SubscriptionId = "MessageD-SomeConsumer")]
    public async Task ConsumeAsync(MessageD message, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("[{0}] SomeConsumer 开始消费 MessageD： {1}", DateTime.Now.ToLocalTime(), message.Text);
        await Task.Delay(200);
        Console.WriteLine("[{0}] SomeConsumer 消费完成 MessageD： {1}", DateTime.Now.ToLocalTime(), message.Text);
    }

    [AutoSubscriberConsumer(SubscriptionId = "MessageE-SomeConsumer")]
    public async Task ConsumeAsync(MessageE message, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("[{0}] SomeConsumer 开始消费 MessageE： {1}", DateTime.Now.ToLocalTime(), message.Text);
        await Task.Delay(600);
        Console.WriteLine("[{0}] SomeConsumer 消费完成 MessageE： {1}", DateTime.Now.ToLocalTime(), message.Text);
    }
}
