
using NServiceBus;

namespace Sales;

public class Program
{
    public static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    public static async Task AsyncMain()
    {
        Console.Title = "Sales";

        var endpointConfiguration = new EndpointConfiguration("Sales");

        var transport = endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press Enter to exit.");  
        Console.ReadLine();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
