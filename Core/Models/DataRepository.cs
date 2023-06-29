using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventDrivenAppDemo.Core.Models
{
    public class DataRepository : IDataRepository
    {
        private readonly SensorDataDbContext db;

        public DataRepository(SensorDataDbContext db)
        {
            this.db = db;
        }

        public List<SensorData> GetSensorDatas() => db.SensorData.ToList();

        public SensorData PutSensorData(SensorData SensorData)
        {
            db.SensorData.Update(SensorData);
            db.SaveChanges();
            //            return db.SensorData.Where(x => x.SensorDataId == SensorData.SensorDataId).FirstOrDefault();
            //   mahima chang here
            return new SensorData();
        }

        public List<SensorData> AddSensorData(SensorData SensorData)
        {
            db.SensorData.Add(SensorData);
            db.SaveChanges();
            return db.SensorData.ToList();
        }

        public SensorData GetSensorDataById(string Id)
        {
            // return db.SensorData.Where(x => x.SensorDataId == Id).FirstOrDefault();
            //   mahima chang here
            return new SensorData();
        }

    }
}
