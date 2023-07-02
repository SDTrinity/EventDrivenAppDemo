using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using EventDrivenAppDemo.Core.Models;
using Microsoft.Extensions.Logging;

namespace SqlService
{
    public class AsyncWriter
    {
        private void InsertData(IHost app, SensorData sensorData)
        {
            var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
            if(scopedFactory != null)
            {
                using (var scope = scopedFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetService<DataRepository>();
                    if (service != null)
                    {
                        service.AddSensor(sensorData);
                    }
                }
            }

        }
        public void Consume(IModel channel, IHost app)
        {
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
                if (sensorData != null)
                {
                    sensorData.SensorDataId = Guid.NewGuid().ToString();
                    InsertData(app,sensorData);
                }

            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
