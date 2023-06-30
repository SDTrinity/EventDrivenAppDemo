﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
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

        public List<SensorData> AddSensor(SensorData SensorData)
        {
            db.SensorData.Add(SensorData);
            db.SaveChanges();
            return db.SensorData.Where(x => x.SensorDataId == SensorData.SensorDataId).ToList();
        }
        public dynamic GetSensorsByType(string type)
        {
            return db.SensorData.Where(x => x.Type == type).ToList();
        }

        public dynamic GetSensors()
        {
            return db.SensorData.ToList().Select(x => new { x.Gate, x.Type, x.NumberOfPeople });
        }

        public dynamic GetSensorsByDateRange(DateTime fromTimeStamp, DateTime toTimeStamp)
        {
            return db.SensorData.Where(x => x.TimeStamp >= fromTimeStamp && x.TimeStamp <= toTimeStamp)
                .ToList();
        }

    }
}
