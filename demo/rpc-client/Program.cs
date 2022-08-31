using System.Text;
using RabbitMQ.Client;

namespace rpc_client;

public class Program
{
    public static void Main(string[] args)
    {
        var rpcClient = new RpcClient("r_queue1");

        var response = rpcClient.Call(args.Length > 0 ? args[0] : "30");
        Console.WriteLine(response);

        rpcClient.Close();
    }
}
