using System.Collections.Generic;
using System.Linq;

namespace EventDrivenAppDemo.Core.Models
{
    public class DataWriter
    {
        private readonly SensorDataDbContext SensorDataDbContext;
        static readonly object _object = new object();
        public DataWriter(SensorDataDbContext SensorDataDbContext)
        {
            this.SensorDataDbContext = SensorDataDbContext;
        }

        public void Write(List<SensorData> SensorDatas)
        {
            try
            {
                Monitor.Enter(_object);
                SensorDataDbContext.SensorData.AddRange(SensorDatas);
                SensorDataDbContext.SaveChanges();

            }
            catch(Exception ex)
            {
                //TODO
            }
            finally
            {
                Monitor.Exit(_object);
            }
        }
    }
}
