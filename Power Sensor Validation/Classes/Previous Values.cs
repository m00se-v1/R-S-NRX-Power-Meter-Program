using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Power_Sensor_Validation.Classes
{
    public class Previous_Values
    {
        public string Previous_Value { get; set; }
        public Previous_Values(string previous_Value)
        {
            Previous_Value = previous_Value;
        }
    }
}
