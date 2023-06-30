using System.Text;
using EventDrivenAppDemo.Core.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;


Console.WriteLine("[*]              SENSOR DATA GENERATION SIMULATION               [*]");
Console.WriteLine("[*]                                                              [*]");
Console.WriteLine("[*]                                                              [*]");
var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "SensorDataExchange", type: ExchangeType.Fanout);

bool entryType = true;
while (true)
{
    var message = GetMessage(entryType);
    var body = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(exchange: "SensorDataExchange",
                         routingKey: string.Empty,
                         basicProperties: null,
                         body: body);
    Console.WriteLine($"Sent {message}");
    entryType = !entryType;
    Thread.Sleep(10000);
}

static string GetMessage(bool entryType)
{
    var numberOfPeople = new Random();
    int count = numberOfPeople.Next(0,100);
    var gate = new Random();
    int gateNumber = numberOfPeople.Next(1, 6);

    var sensorData = new SensorData()
    {
        Gate = gateNumber.ToString(),
        TimeStamp = DateTime.UtcNow,
        NumberOfPeople = count,
        Type = (entryType == true ? "enter": "leave")
    };
    return JsonConvert.SerializeObject(sensorData);
}