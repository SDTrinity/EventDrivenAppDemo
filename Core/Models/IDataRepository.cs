using System.Collections.Generic;

namespace EventDrivenAppDemo.Core.Models
{
    public interface IDataRepository
    {
        List<SensorData> AddSensor(SensorData SensorData);
        dynamic GetSensors();
        dynamic GetSensorsByType(string type);

        dynamic GetSensorsByDateRange(DateTime from, DateTime to);
    }
}