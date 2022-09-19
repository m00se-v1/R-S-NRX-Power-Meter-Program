using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Power_Sensor_Validation.Classes
{
    internal class ControlControls
    {
        public void Visibility(bool cool,Button mainButton, Button fileBrowse, Button folderBrowse, Button fileSave, Button checkConnection, TextBox sensorModel, TextBox sensorSn,ComboBox comboBox,CheckBox fileCreated, TextBox directoryField, Button FillData)
        {
            switch(cool)
            {
                case false:
                    if (fileSave.IsEnabled)
                    {
                        FillData.IsEnabled = false;
                        mainButton.IsEnabled = false;
                        fileSave.IsEnabled = false;
                        sensorSn.IsEnabled = false;
                        sensorModel.IsEnabled = false;
                        directoryField.IsEnabled = false;
                        checkConnection.IsEnabled = false;
                        comboBox.IsEnabled = false;
                        fileCreated.IsEnabled = false;
                        fileBrowse.IsEnabled = false;
                        folderBrowse.IsEnabled = false;
                    }
                    else
                    {
                        FillData.IsEnabled = false;
                        mainButton.IsEnabled = false;
                        sensorSn.IsEnabled = false;
                        sensorModel.IsEnabled = false;
                        directoryField.IsEnabled = false;
                        checkConnection.IsEnabled = false;
                        comboBox.IsEnabled = false;
                        fileCreated.IsEnabled = false;
                        fileBrowse.IsEnabled = false;
                        folderBrowse.IsEnabled = false;
                    }
                    break;
                case true:
                    FillData.IsEnabled = true;
                    mainButton.IsEnabled = true;
                    fileSave.IsEnabled = true;
                    sensorSn.IsEnabled = true;
                    sensorModel.IsEnabled = true;
                    directoryField.IsEnabled = true;
                    checkConnection.IsEnabled = true;
                    comboBox.IsEnabled = true;
                    fileCreated.IsEnabled = true;
                    fileBrowse.IsEnabled = true;
                    folderBrowse.IsEnabled = true;
                    break;
            }

        }
    }
}
