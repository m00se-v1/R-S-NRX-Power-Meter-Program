using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Power_Sensor_Validation.Classes
{
    internal class CreateFileName
    {
        //Naming convention for the .csv files
        public string FileName(TextBox sensorModel, TextBox sensorSn, ComboBox comboBox)
        {
            string modifier;
            if(comboBox.Text == "Before Annual Calibration" || comboBox.Text == "After Annual Calibration")
            {
                modifier = "Annual_Calibration_Data";
            }
            else if(comboBox.Text == "Before Repair" || comboBox.Text == "After Repair")
            {
                modifier = "Repair-Data";
            }
            else
            {
                modifier = "Troubleshooting-Data";
            }
            string fileName = $"{sensorModel.Text.ToUpper()}_{sensorSn.Text}_{modifier}.csv";
            return fileName;
        }


    }
}
