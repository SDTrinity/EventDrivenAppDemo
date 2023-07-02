using EventDrivenAppDemo.Core.Models;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using SqlService;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDb");

builder.Services.AddTransient<DataRepository>();
//Add Repository Pattern
builder.Services.AddScoped<IDataRepository, DataRepository>();
builder.Services.AddDbContext<SensorDataDbContext>(x => x.UseSqlServer(connectionString));
var app = builder.Build();

app.MapGet("/", () => "Sql Service!");
var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
var factory = new ConnectionFactory { HostName = "localhost" };
var connection = factory.CreateConnection();
var channel = connection.CreateModel();
AsyncWriter asyncWriter = new AsyncWriter();
asyncWriter.Consume(channel, app);


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
        return db.GetSensorsByDateRange(from, to);
    
});
app.Run();
