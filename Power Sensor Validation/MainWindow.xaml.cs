using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Windows.Media;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Net.NetworkInformation;
using System.IO;
using System.Globalization;
using RohdeSchwarz.RsPwrMeter;
using System.Threading;
using Power_Sensor_Validation.Classes;



namespace Power_Sensor_Validation
{
    public partial class MainWindow : Window
    {
        #region Declarations
        GoodToGo checkIf = new GoodToGo();
        DispatcherTimer pingTime = new DispatcherTimer();
        Ping ping = new Ping();
        DataTable dtMain = new DataTable();
        DataTable dtPrev = new DataTable();
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        FileBrowseAction aFileBrowse = new FileBrowseAction();
        FileSaveAction aFileSave = new FileSaveAction();
        ReasonSelectAction aReasonSelect = new ReasonSelectAction();
        CultureInfo cultEN_US = new CultureInfo("en-US");
        ControlControls controls = new ControlControls();
        GetSensorInfo lets = new GetSensorInfo();
        public PingReply reply;
        public int errorMessageCount = 0;
        public int next=0;
        public static string validationStatus;
        public string delta;
        public string deltaFromPrev;
        public string currentRack = Environment.MachineName.ToString();
        public string sensorSerial;
        public string snsrModel;
        RsPwrMeter NRX;
        double[] testResult;
        string[] testResultFromPrev;
        public int[] powerLevels = {
            -20,
            -10,
            0
        };
        public long[] freqREF = {
            1000000000,
            2000000000,
            3000000000,
            4000000000,
            5000000000,
            6000000000
        };
        public string[] freqSTR =
        {
            "1.000 GHz",
            "2.000 GHz",
            "3.000 GHz",
            "4.000 GHz",
            "5.000 GHz",
            "6.000 GHz"
        };
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            
        }
        #region Handlers
        public void TheWindow_Initialized(object sender, EventArgs e)
        {
            pingTime.Interval = new TimeSpan(0, 0, 0, 1, 500);
            pingTime.Start();
            pingTime.Tick += new EventHandler(pwrMeterTime);

        }

        private void dataGrid_Initialized(object sender, EventArgs e)
        {
            dtMain.Columns.Add("Validation Reason");
            dtMain.Columns.Add("Power (dBm)");
            dtMain.Columns.Add("Frequency");
            dtMain.Columns.Add("Value (dB)");
            dtMain.Columns.Add("Delta from Power");

        }

        private void fileSave_Click(object sender, RoutedEventArgs e)
        {
            aFileSave.execute(sensorModelBox, sensorSNBox, fileDir, cbReason, actualStatus, dtMain, isCreated);

        }

        private void checkConnection_Click(object sender, RoutedEventArgs e)
        {
            pingTime.Start();
        }

        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            aReasonSelect.execute(saveDir, fileBrowse, folderBrowse, isCreated, cbReason, dtMain,sensorModelBox);
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fileSave.IsEnabled)
            {
                fileSave.IsEnabled = false;
            }
        }

        private void sensorModelBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fileSave.IsEnabled)
            {
                fileSave.IsEnabled = false;
            }
        }

        private void sensorSNBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fileSave.IsEnabled)
            {
                fileSave.IsEnabled = false;
            }
        }

        private void fileBrowse_Click(object sender, RoutedEventArgs e)
        {
            if(cbReason.Text=="After Annual Calibration"||cbReason.Text=="After Repair")
            {
                testResultFromPrev = aFileBrowse.Execute(sensorModelBox, sensorSNBox, fileDir, dataGrid, dtPrev, testResultFromPrev, cbReason);
            }
            else
            {
                aFileBrowse.SoftExecute(sensorModelBox, sensorSNBox, fileDir, dataGrid, dtPrev, testResultFromPrev, cbReason);

            }
        }

        public void folderBrowse_Click(object sender, RoutedEventArgs e)
        {
            folderBrowserDialog.ShowDialog();
            fileDir.Text = folderBrowserDialog.SelectedPath;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if(indicator.Fill==Brushes.Red)
            {
                System.Windows.MessageBox.Show("Make sure the power meter is synced up with the program by clicking the Sync button and making sure the circle is green.", "Error");

            }
            else
            {
                actualStatus.Content = "Starting...";
                MainBody();
            }
        }

        private void fileDir_Initialized(object sender, EventArgs e)
        {
            fileDir.Text = @"\\fs1\Data\Engineering Operations\Cellular Components Characterization\CCC\SOP & Training\Equipment\MPC Equipment\POWER SENSOR VALIDATION\" + currentRack;
        }

        public void pwrMeterTime(object sender, EventArgs e)
        {
            pwrMeterConnection();
        }

        private void isCreated_Checked(object sender, RoutedEventArgs e)
        {
            saveDir.Content = "File Name:";
            System.Windows.MessageBox.Show("Please click on the File Browse button and select your file.", "Select a File");
            folderBrowse.Visibility = Visibility.Hidden;
            fileBrowse.Visibility = Visibility.Visible;
        }

        private void isCreated_Unchecked(object sender, RoutedEventArgs e)
        {
            saveDir.Content = "File Save Directory:";
            System.Windows.MessageBox.Show("Please click on the Folder Browse button and select the folder to save your file in.", "Select a Folder");
            fileBrowse.Visibility = Visibility.Hidden;
            folderBrowse.Visibility = Visibility.Visible;
        }

        private async void autoFill_Click(object sender, RoutedEventArgs e)
        {
            if(indicator.Fill!=Brushes.Red)
            {
                controls.Visibility(false, Play, fileBrowse, folderBrowse, fileSave, checkConnection, sensorModelBox, sensorSNBox, cbReason, isCreated, fileDir, autoFill);
                Dispatcher.Invoke(() => actualStatus.Content = "Getting Serial Number...");
                await Task.Run(() => sensorSerial = lets.GetSerial(NRX, sensorSerial, pingTime));
                Dispatcher.Invoke(() => actualStatus.Content = "Getting Model...");
                await Task.Run(() => snsrModel = lets.GetModel(NRX, snsrModel, pingTime));
                sensorModelBox.Text = snsrModel;
                sensorSNBox.Text = sensorSerial;
                controls.Visibility(true, Play, fileBrowse, folderBrowse, fileSave, checkConnection, sensorModelBox, sensorSNBox, cbReason, isCreated, fileDir, autoFill);
                actualStatus.Content = "Not Testing";
            }
            else
            {
                return;
            }
        }

        #endregion

        #region Prelude
        public async void MainBody()
        {
            controls.Visibility(false, Play, fileBrowse, folderBrowse, fileSave, checkConnection, sensorModelBox, sensorSNBox, cbReason, isCreated, fileDir, autoFill);
            pingTime.Stop();
            if (checkIf.weAreGood(sensorModelBox, sensorSNBox, fileDir, cbReason))
            {
                try
                {


                    validationStatus = cbReason.Text;
                    await Task.Run(() => Run());
                }
                catch (Ivi.Driver.IOException e)
                {
                    System.Windows.MessageBox.Show("Something went wrong.\n\n" + e.Message, "Oops", MessageBoxButton.OK);
                    if (Convert.ToBoolean(MessageBoxResult.OK))
                    {
                        Close();
                    }
                }

            }
            else
            {
                System.Windows.MessageBox.Show("Please make sure that:\n\nThe power sensor model is in the format of NRP-XXX or NRPXXX\nThe power sensor's serial number is correct\nYou've specified a location for the data to be saved\nYou've selected a reason for validation.", "Error");
                actualStatus.Content = "Not Testing";
            }
            actualStatus.Content = "All Done";
            fileSave.IsEnabled = true;
            pingTime.Start();
            controls.Visibility(true, Play, fileBrowse, folderBrowse, fileSave, checkConnection, sensorModelBox, sensorSNBox, cbReason, isCreated, fileDir, autoFill);

        }
        #endregion

        #region Power Meter Ping
        public void pwrMeterConnection()
        {
            pingTime.Stop();
            bool isPresent = false;
            reply = ping.Send("10.0.0.25");
            isPresent = reply.Status == IPStatus.Success;
            if (isPresent)
            {
                errorMessageCount = 0;
                indicator.Fill = Brushes.Green;
                pingTime.Start();
            }
            else
            {
                pingTime.Stop();
                indicator.Fill = Brushes.Red;
                if (errorMessageCount == 0)
                {
                    errorMessageCount = 1;
                    System.Windows.MessageBox.Show("Please make sure the NRX power meter is connected to the rack's network switch via ethernet cable.\n\nOnce you are sure the power meter is connected, press the Sync button.", "Error");
                }
            }

        }
        #endregion

        #region Main Process
        public void Run()
        {
            if (dtMain.Rows.Count > 0)
            {
                dtMain.Rows.Clear();
            }
            Dispatcher.Invoke(() =>
            {
                if(dtMain.Columns.Contains($"{DateTime.Today.ToString("d")}"))
                {
                    dtMain.Columns.Remove($"{DateTime.Today.ToString("d")}");
                }
                dtMain.Columns.Add($"{DateTime.Today.ToString("d")}");

            });
            try
            {
                NRX = new RsPwrMeter("TCPIP::10.0.0.25::INSTR", true, true, "Simulate=False");
                try
                {
                    Dispatcher.Invoke(() => actualStatus.Content = "Resetting Power Meter...");
                    NRX.Utility.Reset();
                    Dispatcher.Invoke(() => actualStatus.Content = "Done");
                    Thread.Sleep(500);
                    NRX.UtilityFunctions.OPCTimeout = 10000;
                    NRX.UtilityFunctions.SelfTest();
                    Dispatcher.Invoke(() => dataGrid.ItemsSource = dtMain.DefaultView);
                    Dispatcher.Invoke(() => actualStatus.Content = "Power Sensor Selftest...");
                    NRX.Channel["CH1"].Sensor.RepCapKey = "CH1";
                    NRX.Channel["CH1"].Sensor.RepCapName = "Channel";
                    string selftestresult = NRX.Channel["CH1"].Sensor.Test();
                    if (selftestresult.Contains("PASS"))
                    {
                        Dispatcher.Invoke(() => actualStatus.Content = "Passed");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Dispatcher.Invoke(() => actualStatus.Content = "Failed");
                        System.Windows.MessageBox.Show("The power sensor has failed the self test.\n\nThe tool will now close.","Fatal Error");
                        Dispatcher.Invoke(() => this.Close());
                    }
                    
                    Dispatcher.Invoke(() => actualStatus.Content = "Zeroing Power Sensor...");
                    NRX.Channel["CH1"].Zero();
                    Dispatcher.Invoke(() => actualStatus.Content = "Done");
                    Thread.Sleep(500);
                    NRX.Measurement.Channel["CH1"].Continuous = false;
                    NRX.Channel["CH1"].Mode = MeasurementMode.ContAv;
                    NRX.Channel["CH1"].Averaging.Enabled = false;
                    NRX.System.WriteString("SOURce:RF:FREQuency:VALue 1.0e9\n");
                    Dispatcher.Invoke(() => actualStatus.Content = "Testing...");
                    for (int i = 0; i < powerLevels.Length; i++)
                    {
                        NRX.System.WriteString("SOURce:POWer:VALue " + powerLevels[i] + "\n");


                        for (int j = 0; j < freqSTR.Length; j++)
                        {
                            NRX.Channel["CH1"].Frequency = freqREF[j];
                            NRX.System.WriteString("OUTPut:SOURce:STATe 1\n");
                            NRX.Channel["CH1"].Trigger.Source = TriggerSource.Bus;
                            NRX.Measurement.Channel["CH1"].InitiateWait();
                            testResult = NRX.Measurement.Fetch();
                            delta = (testResult[0] - powerLevels[i]).ToString("0.000", cultEN_US);
                            deltaFromPrev = (testResult[0] - 2).ToString("0.000", cultEN_US);
                            if(dtMain.Columns.Contains("Delta from Previous Values"))
                            {
                                Dispatcher.Invoke(() => dtMain.Rows.Add(validationStatus, powerLevels[i], freqSTR[j], testResult[0].ToString("0.000", cultEN_US), delta, (testResult[0] - Convert.ToDouble(testResultFromPrev[next])).ToString("0.000",cultEN_US)));
                                next++;
                            }
                            else
                            {
                                Dispatcher.Invoke(() => dtMain.Rows.Add(validationStatus, powerLevels[i], freqSTR[j], testResult[0].ToString("0.000", cultEN_US), delta));
                            }
                            NRX.System.WriteString("OUTPut:SOURce:STATe 0\n");
                        }
                    }
                    Dispatcher.Invoke(() => actualStatus.Content = "Resetting/Disposing...");
                    NRX.Utility.Reset();
                    NRX.Dispose();

                }
                catch (Ivi.Driver.IOException e)
                {
                    System.Windows.MessageBox.Show(e.Message, "Error");
                }
            }
            catch (Ivi.Driver.IOException e)
            {
                System.Windows.MessageBox.Show(e.Message, "Communication Error");
            }
        }
        #endregion
    }
}


