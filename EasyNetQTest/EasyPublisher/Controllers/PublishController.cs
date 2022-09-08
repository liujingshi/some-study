using EasyNetQ;
using Messages;
using Microsoft.AspNetCore.Mvc;

namespace EasyPublisher.Controllers;

[ApiController]
[Route("[controller]")]
public class PublishController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private IBus _rabbitBus;

    public PublishController(IBus rabbitBus)
    {
        _rabbitBus = rabbitBus;
    }

    [HttpPost]
    public ActionResult Post([FromForm] string msg) 
    {
        //_rabbitBus.PubSub.Publish(new TextMessage(msg));

        Pb(msg);

        return Ok(new
        {
            success = true,
        });
    }


    [HttpGet]
    public IEnumerable<object> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    private void Pb(string msg)
    {
        _rabbitBus.PubSub.PublishAsync(new TextMessage(msg)).ContinueWith(task => 
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                Console.WriteLine("{0} Successed", msg);
            }
            else
            {
                Console.WriteLine("{0} Error...Retry...", msg);
                Pb(msg);
            }
        });
    }
}