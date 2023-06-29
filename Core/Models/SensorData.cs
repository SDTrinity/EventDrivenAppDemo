using System;
using System.Collections.Generic;
using System.Text;

namespace EventDrivenAppDemo.Core.Models
{
    public class SensorData
    {
        public string Gate { get; set; }
        public DateTime TimeStamp { get; set; }
        public int NumberOfPeople { get; set; }
        public string Type { get; set; }

    }
}
