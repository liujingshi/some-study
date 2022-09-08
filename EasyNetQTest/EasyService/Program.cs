using EasyNetQ;

namespace EasyService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // EasyNetQ
        builder.Services.RegisterEasyNetQ("" +
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
            "timeout=10");

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
