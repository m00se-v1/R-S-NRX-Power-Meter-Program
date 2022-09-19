using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;

namespace Power_Sensor_Validation.Classes
{
    internal class GoodToGo
    {
        public bool weAreGood(TextBox sensorModel, TextBox sensorSn, TextBox directoryField, ComboBox comboBox)
        {
            bool yesOrNo;
            yesOrNo = (sensorModel.Text.ToUpper().Contains("NRP") && (Convert.ToInt32(sensorSn.Text) > 100000) && directoryField.Text != "" && (Directory.Exists(directoryField.Text) || File.Exists(directoryField.Text))&& comboBox.Text != "");
            return yesOrNo;
        }
    }
}
