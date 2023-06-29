using System.Collections.Generic;
using System.Linq;

namespace EventDrivenAppDemo.Core.Models
{
    public class DataWriter
    {
        private readonly SensorDataDbContext SensorDataDbContext;

        public DataWriter(SensorDataDbContext SensorDataDbContext)
        {
            this.SensorDataDbContext = SensorDataDbContext;
        }

        public void Write()
        {
            if(!SensorDataDbContext.SensorData.Any())
            {
                var SensorDatas = new List<SensorData>()
                {
                        new SensorData()
                        {
                            Gate = "1",
                            TimeStamp = DateTime.UtcNow,
                            NumberOfPeople = 10,
                            Type = "leave"
                        },
                        new SensorData()
                        {
                           Gate = "2",
                            TimeStamp = DateTime.UtcNow,
                            NumberOfPeople = 10,
                            Type = "entry"
                        }
                };

                SensorDataDbContext.SensorData.AddRange(SensorDatas);
                SensorDataDbContext.SaveChanges();
            }
        }
    }
}
