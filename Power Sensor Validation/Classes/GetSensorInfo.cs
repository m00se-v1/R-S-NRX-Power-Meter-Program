using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Xml.Linq;
using RohdeSchwarz.RsPwrMeter;

namespace Power_Sensor_Validation.Classes
{
    internal class GetSensorInfo
    {

        public string GetSerial(RsPwrMeter pwrMeter, String sensorNum, DispatcherTimer timer)
        {
            timer.Stop();
            pwrMeter = new RsPwrMeter("TCPIP::10.0.0.25::INSTR", true, true, "Simulate=False");
            string[] info = pwrMeter.Channel["CH1"].Sensor.Info.Split(',');
            sensorNum = info[20].Remove(0,8);
            sensorNum = sensorNum.Remove(6, 1);

            pwrMeter.Close();
            timer.Start();
            return sensorNum;
        }
        public string GetModel(RsPwrMeter pwrMeter, String sensorMod, DispatcherTimer timer)
        {
            timer.Stop();
            pwrMeter = new RsPwrMeter("TCPIP::10.0.0.25::INSTR", true, true, "Simulate=False");
            string[] info = pwrMeter.Channel["CH1"].Sensor.Info.Split(',');
            sensorMod = info[25].Remove(0, 6);
            if(sensorMod.Contains("NRP-"))
            {
                sensorMod = sensorMod.Remove(7,1);
            }
            sensorMod = sensorMod.Remove(6,1);
            pwrMeter.Close();
            timer.Start();
            return sensorMod;
        }
    }
}
