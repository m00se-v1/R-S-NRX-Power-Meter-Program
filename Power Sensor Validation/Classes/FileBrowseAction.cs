using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Globalization;
using CsvHelper;
using System.Data;
using Microsoft.Win32;

namespace Power_Sensor_Validation.Classes
{
    internal class FileBrowseAction
    {

        List2DT converter = new List2DT();

        public string[] Execute(TextBox sensorModel, TextBox sensorSn, TextBox directoryField, DataGrid dataGrid, DataTable oldTable, string[] array, ComboBox comboBox)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Comma-Separated Values files (*.csv)|*.csv|All files (*.*)|*.*";
            openFile.FilterIndex = 2;
            openFile.InitialDirectory = @"\\fs1\Data\Engineering Operations\Cellular Components Characterization\CCC\SOP & Training\Equipment\MPC Equipment\POWER SENSOR VALIDATION\" + Environment.MachineName.ToString();
            openFile.ShowDialog();
            directoryField.Text = openFile.FileName;
            string tempTitle;
            switch (comboBox.Text)
            {
                case "After Annual Calibration":
                    if (directoryField.Text.Contains("NRP-Z21"))
                    {
                        sensorModel.Text = "NRP-Z21";
                        sensorModel.IsEnabled = false;
                    }
                    else if (directoryField.Text.Contains("NRP-Z11"))
                    {
                        sensorModel.Text = "NRP-Z11";
                        sensorModel.IsEnabled = false;
                    }
                    else if (directoryField.Text.Contains("NRP18S"))
                    {
                        sensorModel.Text = "NRP18S";
                        sensorModel.IsEnabled = false;
                    }
                    else if (directoryField.Text.Contains("NRP33S"))
                    {
                        sensorModel.Text = "NRP33S";
                        sensorModel.IsEnabled = false;
                    }
                    else if (directoryField.Text.Contains("NRP50S"))
                    {
                        sensorModel.Text = "NRP50S";
                        sensorModel.IsEnabled = false;
                    }
                    if (directoryField.Text.Contains("NRP-Z21") || directoryField.Text.Contains("NRP-Z11"))
                    {
                        tempTitle = openFile.SafeFileName;
                        tempTitle = tempTitle.Remove(0, 8);
                        tempTitle = tempTitle.Remove(6, 36);
                        sensorSn.Text = tempTitle;
                        sensorSn.IsEnabled = false;
                    }
                    else
                    {
                        tempTitle = openFile.SafeFileName;
                        tempTitle = tempTitle.Remove(0, 7);
                        tempTitle = tempTitle.Remove(6, 36);
                        sensorSn.Text = tempTitle;
                        sensorSn.IsEnabled = false;
                    }
                    break;
                case "After Repair":
                    if (directoryField.Text.Contains("NRP-Z21"))
                    {
                        sensorModel.Text = "NRP-Z21";
                        sensorModel.IsEnabled = false;
                    }
                    else if (directoryField.Text.Contains("NRP-Z11"))
                    {
                        sensorModel.Text = "NRP-Z11";
                        sensorModel.IsEnabled = false;
                    }
                    else if (directoryField.Text.Contains("NRP18S"))
                    {
                        sensorModel.Text = "NRP18S";
                        sensorModel.IsEnabled = false;
                    }
                    else if (directoryField.Text.Contains("NRP33S"))
                    {
                        sensorModel.Text = "NRP33S";
                        sensorModel.IsEnabled = false;
                    }
                    else if (directoryField.Text.Contains("NRP50S"))
                    {
                        sensorModel.Text = "NRP50S";
                        sensorModel.IsEnabled = false;
                    }
                    if (directoryField.Text.Contains("NRP-Z21") || directoryField.Text.Contains("NRP-Z11"))
                    {
                        tempTitle = openFile.SafeFileName;
                        tempTitle = tempTitle.Remove(0, 8);
                        tempTitle = tempTitle.Remove(6, 36);
                        sensorSn.Text = tempTitle;
                        sensorSn.IsEnabled = false;
                    }
                    else
                    {
                        tempTitle = openFile.SafeFileName;
                        tempTitle = tempTitle.Remove(0, 7);
                        tempTitle = tempTitle.Remove(6, 36);
                        sensorSn.Text = tempTitle;
                        sensorSn.IsEnabled = false;
                    }
                    break;
                case "Troubleshooting/Comparison":
                    if (directoryField.Text.Contains("NRP-Z21"))
                    {
                        sensorModel.Text = "NRP-Z21";
                    }
                    else if (directoryField.Text.Contains("NRP-Z11"))
                    {
                        sensorModel.Text = "NRP-Z11";
                    }
                    else if (directoryField.Text.Contains("NRP18S"))
                    {
                        sensorModel.Text = "NRP18S";
                    }
                    else if (directoryField.Text.Contains("NRP33S"))
                    {
                        sensorModel.Text = "NRP33S";
                    }
                    else if (directoryField.Text.Contains("NRP50S"))
                    {
                        sensorModel.Text = "NRP50S";
                    }
                    break;
                default:
                    if (directoryField.Text.Contains("NRP-Z21"))
                    {
                        sensorModel.Text = "NRP-Z21";
                    }
                    else if (directoryField.Text.Contains("NRP-Z11"))
                    {
                        sensorModel.Text = "NRP-Z11";
                    }
                    else if (directoryField.Text.Contains("NRP18S"))
                    {
                        sensorModel.Text = "NRP18S";
                    }
                    else if (directoryField.Text.Contains("NRP33S"))
                    {
                        sensorModel.Text = "NRP33S";
                    }
                    else if (directoryField.Text.Contains("NRP50S"))
                    {
                        sensorModel.Text = "NRP50S";
                    }
                    if (directoryField.Text.Contains("NRP-Z21") || directoryField.Text.Contains("NRP-Z11"))
                    {
                        tempTitle = openFile.SafeFileName;
                        tempTitle = tempTitle.Remove(0, 8);
                        tempTitle = tempTitle.Remove(6, 36);
                        sensorSn.Text = tempTitle;
                    }
                    else
                    {
                        tempTitle = openFile.SafeFileName;
                        tempTitle = tempTitle.Remove(0, 7);
                        tempTitle = tempTitle.Remove(6, 36);
                        sensorSn.Text = tempTitle;
                    }
                    break;
            }
            IEnumerable<string> input = File.ReadLines(directoryField.Text);
            List<string> output = new List<string>();
            output = input.ToList();
            output.RemoveRange(0, output.Count - 18);

            List<Previous_Values> pValues = new List<Previous_Values>();
            foreach (var line in output)
            {
                var delimitedLine = line.Split(',');
                pValues.Add(new Previous_Values(delimitedLine[3]));


                if (pValues.Count == 18)
                {
                    oldTable = converter.ToDataTable(pValues);
                    dataGrid.ItemsSource = oldTable.DefaultView;
                }
            }
            array = oldTable.AsEnumerable().Select(r => r.Field<string>("Previous Values (dB)")).ToArray();
            return array;
        }
        public void SoftExecute(TextBox sensorModel, TextBox sensorSn, TextBox directoryField, DataGrid dataGrid, DataTable oldTable, string[] array, ComboBox comboBox)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Comma-Separated Values files (*.csv)|*.csv|All files (*.*)|*.*";
            openFile.FilterIndex = 2; 
            openFile.InitialDirectory = @"\\fs1\Data\Engineering Operations\Cellular Components Characterization\CCC\SOP & Training\Equipment\MPC Equipment\POWER SENSOR VALIDATION\" + Environment.MachineName.ToString();
            openFile.ShowDialog();
            directoryField.Text = openFile.FileName;
            string tempTitle;
            switch (comboBox.Text)
            {
                case "After Annual Calibration":
                    if (directoryField.Text.Contains("NRP-Z21"))
                    {
                        sensorModel.Text = "NRP-Z21";
                        sensorModel.IsEnabled = false;
                    }
                    else if (directoryField.Text.Contains("NRP-Z11"))
                    {
                        sensorModel.Text = "NRP-Z11";
                        sensorModel.IsEnabled = false;
                    }
                    else if (directoryField.Text.Contains("NRP18S"))
                    {
                        sensorModel.Text = "NRP18S";
                        sensorModel.IsEnabled = false;
                    }
                    else if (directoryField.Text.Contains("NRP33S"))
                    {
                        sensorModel.Text = "NRP33S";
                        sensorModel.IsEnabled = false;
                    }
                    else if (directoryField.Text.Contains("NRP50S"))
                    {
                        sensorModel.Text = "NRP50S";
                        sensorModel.IsEnabled = false;
                    }
                    if (directoryField.Text.Contains("NRP-Z21") || directoryField.Text.Contains("NRP-Z11"))
                    {
                        tempTitle = openFile.SafeFileName;
                        tempTitle = tempTitle.Remove(0, 8);
                        tempTitle = tempTitle.Remove(6, 36);
                        sensorSn.Text = tempTitle;
                        sensorSn.IsEnabled = false;
                    }
                    else
                    {
                        tempTitle = openFile.SafeFileName;
                        tempTitle = tempTitle.Remove(0, 7);
                        tempTitle = tempTitle.Remove(6, 36);
                        sensorSn.Text = tempTitle;
                        sensorSn.IsEnabled = false;
                    }
                    break;
                case "After Repair":
                    if (directoryField.Text.Contains("NRP-Z21"))
                    {
                        sensorModel.Text = "NRP-Z21";
                        sensorModel.IsEnabled = false;
                    }
                    else if (directoryField.Text.Contains("NRP-Z11"))
                    {
                        sensorModel.Text = "NRP-Z11";
                        sensorModel.IsEnabled = false;
                    }
                    else if (directoryField.Text.Contains("NRP18S"))
                    {
                        sensorModel.Text = "NRP18S";
                        sensorModel.IsEnabled = false;
                    }
                    else if (directoryField.Text.Contains("NRP33S"))
                    {
                        sensorModel.Text = "NRP33S";
                        sensorModel.IsEnabled = false;
                    }
                    else if (directoryField.Text.Contains("NRP50S"))
                    {
                        sensorModel.Text = "NRP50S";
                        sensorModel.IsEnabled = false;
                    }
                    if (directoryField.Text.Contains("NRP-Z21") || directoryField.Text.Contains("NRP-Z11"))
                    {
                        tempTitle = openFile.SafeFileName;
                        tempTitle = tempTitle.Remove(0, 8);
                        tempTitle = tempTitle.Remove(6, 36);
                        sensorSn.Text = tempTitle;
                        sensorSn.IsEnabled = false;
                    }
                    else
                    {
                        tempTitle = openFile.SafeFileName;
                        tempTitle = tempTitle.Remove(0, 7);
                        tempTitle = tempTitle.Remove(6, 36);
                        sensorSn.Text = tempTitle;
                        sensorSn.IsEnabled = false;
                    }
                    break;
                case "Troubleshooting/Comparison":
                    if (directoryField.Text.Contains("NRP-Z21"))
                    {
                        sensorModel.Text = "NRP-Z21";
                    }
                    else if (directoryField.Text.Contains("NRP-Z11"))
                    {
                        sensorModel.Text = "NRP-Z11";
                    }
                    else if (directoryField.Text.Contains("NRP18S"))
                    {
                        sensorModel.Text = "NRP18S";
                    }
                    else if (directoryField.Text.Contains("NRP33S"))
                    {
                        sensorModel.Text = "NRP33S";
                    }
                    else if (directoryField.Text.Contains("NRP50S"))
                    {
                        sensorModel.Text = "NRP50S";
                    }
                    break;
                default:
                    if (directoryField.Text.Contains("NRP-Z21"))
                    {
                        sensorModel.Text = "NRP-Z21";
                    }
                    else if (directoryField.Text.Contains("NRP-Z11"))
                    {
                        sensorModel.Text = "NRP-Z11";
                    }
                    else if (directoryField.Text.Contains("NRP18S"))
                    {
                        sensorModel.Text = "NRP18S";
                    }
                    else if (directoryField.Text.Contains("NRP33S"))
                    {
                        sensorModel.Text = "NRP33S";
                    }
                    else if (directoryField.Text.Contains("NRP50S"))
                    {
                        sensorModel.Text = "NRP50S";
                    }
                    if (directoryField.Text.Contains("NRP-Z21") || directoryField.Text.Contains("NRP-Z11"))
                    {
                        tempTitle = openFile.SafeFileName;
                        tempTitle = tempTitle.Remove(0, 8);
                        tempTitle = tempTitle.Remove(6, 36);
                        sensorSn.Text = tempTitle;
                    }
                    else
                    {
                        tempTitle = openFile.SafeFileName;
                        tempTitle = tempTitle.Remove(0, 7);
                        tempTitle = tempTitle.Remove(6, 36);
                        sensorSn.Text = tempTitle;
                    }
                    break;
            }

        }
    }
}
