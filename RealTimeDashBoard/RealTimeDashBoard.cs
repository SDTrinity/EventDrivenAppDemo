using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" , DispatchConsumersAsync = true};
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

Console.WriteLine("[*]              GREEN STADIUM REAL TIME (Async consumer) DASHBOARD               [*]");
Console.WriteLine("[*]                                                                               [*]");
Console.WriteLine("[*]                                                                               [*]");
channel.ExchangeDeclare(exchange: "SensorDataExchange", type: ExchangeType.Fanout);

// declare a server-named queue
var queueName = channel.QueueDeclare().QueueName;
channel.QueueBind(queue: queueName,
                  exchange: "SensorDataExchange",
                  routingKey: string.Empty);

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.Received += async (model, ea) =>
{
    byte[] body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] {message}");
    await Task.Yield();
};
channel.BasicConsume(queue: queueName,
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();