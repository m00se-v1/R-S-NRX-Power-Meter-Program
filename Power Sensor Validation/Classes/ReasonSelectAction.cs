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
    internal class ReasonSelectAction
    {
        UI_Reset ui = new UI_Reset();
        public void execute(Label directoryLabel, Button fileBrowse, Button folderBrowse, CheckBox fileCreated, ComboBox comboBox, DataTable mainTable, TextBox sensorModel)
        {
            ui.Reset(directoryLabel,fileBrowse,folderBrowse,fileCreated, mainTable, sensorModel);
            if (comboBox.Text == "After Annual Calibration" || comboBox.Text == "After Repair")
            {
                mainTable.Columns.Add("Delta from Previous Values");
                directoryLabel.Content = "File Name:";
                MessageBox.Show("Please click on the File Browse button and select your file.", "Select a File");
                folderBrowse.Visibility = Visibility.Hidden;
                fileBrowse.Visibility = Visibility.Visible;
            }
            else if (comboBox.Text == "Before Annual Calibration")
            {
                if(mainTable.Columns.Contains("Delta from Previous Values"))
                {
                    mainTable.Columns.Remove("Delta from Previous Values");
                }
                fileCreated.Visibility = Visibility.Visible;
            }
            else
            {
                if (folderBrowse.Visibility != Visibility.Visible)
                {
                    directoryLabel.Content = "File Save Directory:";
                    MessageBox.Show("Please click on the Folder Browse button and select the folder to save your file in.", "Select a Folder");
                    folderBrowse.Visibility = Visibility.Visible;
                    fileBrowse.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
