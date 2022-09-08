
using NServiceBus;

namespace ClientUI;

public class Program
{
    public static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    public static async Task AsyncMain()
    {
        Console.Title = "ClientUI";

        var endpointConfiguration = new EndpointConfiguration("ClientUI");

        var transport = endpointConfiguration.UseTransport<LearningTransport>();

        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(object), "Sales");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();

        await endpointInstance.Stop()
            .ConfigureAwait(false);

    }
}
