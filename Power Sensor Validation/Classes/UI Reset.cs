using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Data;

namespace Power_Sensor_Validation.Classes
{
    internal class UI_Reset
    {
        public void Reset(Label directoryLabel, Button fileBrowse, Button folderBrowse, CheckBox fileCreated, DataTable mainTable, TextBox sensorModel)
        {
            if(mainTable.Columns.Contains("Delta from Previous Values"))
            {
                mainTable.Columns.Remove("Delta from Previous Values");
            }
            if(sensorModel.IsEnabled==false)
            {
                sensorModel.IsEnabled = true;
            }
            directoryLabel.Content = "File Save Directory:";
            fileBrowse.Visibility = Visibility.Hidden;
            folderBrowse.Visibility = Visibility.Visible;
            fileCreated.Visibility = Visibility.Hidden;
        }
    }
}
