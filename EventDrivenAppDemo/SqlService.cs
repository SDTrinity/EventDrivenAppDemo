using EventDrivenAppDemo.Core.Models;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDb");

builder.Services.AddTransient<DataWriter>();
//Add Repository Pattern
builder.Services.AddScoped<IDataRepository, DataRepository>();
builder.Services.AddDbContext<SensorDataDbContext>(x => x.UseSqlServer(connectionString));
var app = builder.Build();

app.MapGet("/", () => "Sql Service!");
var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "SensorDataExchange", type: ExchangeType.Fanout);

// declare a server-named queue
var queueName = channel.QueueDeclare().QueueName;
channel.QueueBind(queue: queueName,
                  exchange: "SensorDataExchange",
                  routingKey: string.Empty);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    byte[] body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    SensorData? sensorData = JsonSerializer.Deserialize<SensorData>(message);
    if(sensorData!=null)
    {
        sensorData.SensorDataId = Guid.NewGuid().ToString();
        var sensorDatas = new List<SensorData>();
        sensorDatas.Add(sensorData);
        InsertData(scopedFactory, sensorDatas);
    }

};
channel.BasicConsume(queue: queueName,
                     autoAck: true,
                     consumer: consumer);

void InsertData(IServiceScopeFactory scopedFactory, List<SensorData> sensorDatas)
{
    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<DataWriter>();
        service.Write(sensorDatas);
    }
}


app.MapPost("/Sensor/list", ([FromServices] IDataRepository db) =>
{
     return db.GetSensors();
});

app.MapPost("/Sensor", ([FromServices] IDataRepository db, SensorData sensor) =>
{
    sensor.SensorDataId = Guid.NewGuid().ToString();
    return db.AddSensor(sensor);
});

app.MapGet("/Sensor/type", ([FromServices] IDataRepository db, string type) =>
{
    if (type.Equals("enter") || type.Equals("leave"))
        return db.GetSensorsByType(type);
    else
        return HttpStatusCode.BadRequest;
});

app.MapGet("/Sensor/from/to", ([FromServices] IDataRepository db, DateTime from, DateTime to) =>
{
        return db.GetSensorsByDateRange(from.ToUniversalTime(), to.ToUniversalTime());
    
});
app.Run();
