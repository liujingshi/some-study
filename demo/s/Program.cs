namespace s;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0) return;
        if (args[0] == "p" || args[0] == "P")
        {
            new Producer().Run();
        }
        else
        {
            new Consumer().Run();
        }
    }
}