using System.Collections.Generic;

namespace EventDrivenAppDemo.Core.Models
{
    public interface IDataRepository
    {
        List<SensorData> AddSensorData(SensorData SensorData);
        List<SensorData> GetSensorDatas();
        SensorData PutSensorData(SensorData SensorData);
        SensorData GetSensorDataById(string id);
    }
}