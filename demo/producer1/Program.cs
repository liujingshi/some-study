﻿using System.Text;
using RabbitMQ.Client;

namespace producer1;

public class Program
{
    public static void Main()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            VirtualHost = "/",
            UserName = "guest",
            Password = "guest"
        };

        var connection = factory.CreateConnection();

        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "q1",
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        while (true)
        {
            Console.Write("请输入要发送的消息：");
            var input = Console.ReadLine();
            if (input == null) continue;
            string message = input.ToString();
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: "q1",
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine("发送消息 '{0}' 到队列 q1", message);
        }
    }
}