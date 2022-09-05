using RabbitMQ.Client;
using rabbitmq_eventbus;

namespace material_service;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMQEventBus(this IServiceCollection services, IConfiguration config)
    {
        var rabbitMQSetting = config.GetSection("RabbitMQ");

        // 连接
        services.AddSingleton<RabbitMQConnection>(sp =>
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = rabbitMQSetting["HostName"],
                Port = int.Parse(rabbitMQSetting["Port"]),
                VirtualHost = rabbitMQSetting["VirtualHost"],
                UserName = rabbitMQSetting["UserName"],
                Password = rabbitMQSetting["Password"]
            };

            return new RabbitMQConnection(connectionFactory);
        });

        // RabbitMQEventBus 实例
        services.AddSingleton<RabbitMQEventBus>(sp => 
        { 
            var rabbitMQConnect = sp.GetRequiredService<RabbitMQConnection>();

            return new RabbitMQEventBus(rabbitMQConnect);
        });

        return services;
    }
}
