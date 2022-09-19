using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Data;
using System.IO;

namespace Power_Sensor_Validation.Classes
{
    internal class FileSaveAction
    {
        CsvModify csvModify =new CsvModify();
        public void execute(TextBox sensorModel, TextBox sensorSn, TextBox directoryField, ComboBox comboBox, Label status, DataTable dt, CheckBox fileExists)
        {
            switch (comboBox.Text)
            {
                case "Before Annual Calibration":
                    if(fileExists.IsChecked==true&& File.Exists(directoryField.Text))
                    {
                        csvModify.CsvAppendBelow(sensorModel, sensorSn, comboBox, dt, directoryField);
                        status.Content = "File has been appended!";
                    }
                    else
                    {
                        csvModify.CsvCreate(sensorModel, sensorSn, comboBox, dt, directoryField);
                        status.Content = "File has been saved!";
                    }
                    break;

                case "After Annual Calibration":
                    if (File.Exists(directoryField.Text))
                    {
                        csvModify.CsvAppendAdjacent(sensorModel, sensorSn, comboBox, dt, directoryField);
                        status.Content = "File has been appended!";
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("File not found.", "Error");
                    }
                    break;

                case "Before Repair":
                    if (fileExists.IsChecked == true && File.Exists(directoryField.Text))
                    {
                        csvModify.CsvAppendBelow(sensorModel, sensorSn, comboBox, dt, directoryField);
                        status.Content = "File has been appended!";
                    }
                    else
                    {
                        csvModify.CsvCreate(sensorModel, sensorSn, comboBox, dt, directoryField);
                        status.Content = "File has been saved!";
                    }
                    break;
                case "After Repair":
                    if (File.Exists(directoryField.Text))
                    {
                        csvModify.CsvAppendAdjacent(sensorModel, sensorSn, comboBox, dt,directoryField);
                        status.Content = "File has been appended!";
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("File not found.", "Error");
                    }
                    break;
                default:
                    csvModify.CsvCreate(sensorModel, sensorSn, comboBox, dt, directoryField);
                    status.Content = "File has been saved!";
                    break;
            }
        }
    }
}
