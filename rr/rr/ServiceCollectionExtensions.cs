using RawRabbit.Configuration;
using RawRabbit.Configuration.Exchange;
using RawRabbit.Instantiation;
using RawRabbit.Instantiation.Disposable;

namespace rr;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRawRabbit(this IServiceCollection services)
    {

        services.AddSingleton<BusClient>(sp =>
        {
			return RawRabbitFactory.CreateSingleton(new RawRabbitOptions()
			{
				ClientConfiguration = new RawRabbitConfiguration
				{
					Username = "guest",
					Password = "guest",
					VirtualHost = "/",
					Port = 5672,
					Hostnames = { "localhost" },
					RequestTimeout = TimeSpan.FromSeconds(10),
					PublishConfirmTimeout = TimeSpan.FromSeconds(10),
					PersistentDeliveryMode = true,
					TopologyRecovery = true,
					AutoCloseConnection = false,
					AutomaticRecovery = true,
					Exchange = new GeneralExchangeConfiguration
					{
						AutoDelete = false,
						Durable = true,
						Type = ExchangeType.Topic
					},
					Queue = new GeneralQueueConfiguration
					{
						AutoDelete = false,
						Durable = true,
						Exclusive = false
					},
					RecoveryInterval = TimeSpan.FromMinutes(1),
					GracefulShutdown = TimeSpan.FromMinutes(1),
					RouteWithGlobalId = true
				}
			});
        });

        return services;
    }
}
