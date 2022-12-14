using EasyNetQ;
using Messages;
using Microsoft.AspNetCore.Mvc;

namespace EasyService.Controllers;

[ApiController]
[Route("[controller]")]
public class AppController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private IBus _rabbitBus;

    public AppController(IBus rabbitBus)
    {
        _rabbitBus = rabbitBus;
    }

    [HttpPost]
    public ActionResult Post([FromForm] string msg)
    {
        _rabbitBus.PubSub.Publish(new TextMessage(msg));

        return Ok(new
        {
            success = true,
        });
    }

    [HttpPost]
    [Route("a")]
    public ActionResult PostA([FromForm] string msg)
    {
        _rabbitBus.PubSub.PublishAsync(new MessageA(msg));

        return Ok(new
        {
            success = true,
        });
    }

    [HttpPost]
    [Route("b")]
    public ActionResult PostB([FromForm] string msg)
    {
        _rabbitBus.PubSub.PublishAsync(new MessageB(msg));

        return Ok(new
        {
            success = true,
        });
    }

    [HttpPost]
    [Route("c")]
    public ActionResult PostC([FromForm] string msg)
    {
        _rabbitBus.PubSub.PublishAsync(new MessageC(msg));

        return Ok(new
        {
            success = true,
        });
    }

    [HttpPost]
    [Route("d")]
    public ActionResult PostD([FromForm] string msg)
    {
        _rabbitBus.PubSub.PublishAsync(new MessageD(msg));

        return Ok(new
        {
            success = true,
        });
    }

    [HttpPost]
    [Route("e")]
    public ActionResult PostE([FromForm] string msg)
    {
        _rabbitBus.PubSub.PublishAsync(new MessageE(msg));

        return Ok(new
        {
            success = true,
        });
    }

    [HttpGet]
    public IEnumerable<object> Get()
    {
        _rabbitBus.PubSub.Subscribe<TextMessage>("test", async (textMessage) =>
        {
            await Task.Delay(800);
            Console.WriteLine(textMessage.Text);
        });
        return Enumerable.Range(1, 5).Select(index => new
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
