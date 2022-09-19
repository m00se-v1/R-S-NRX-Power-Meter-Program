using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.Windows.Controls;
using System.IO;
using System.Data;
using System.Globalization;

namespace Power_Sensor_Validation.Classes
{
    internal class CsvModify
    {
        CreateFileName create = new CreateFileName();
        public void CsvCreate(TextBox sensorModel, TextBox sensorSn, ComboBox comboBox, DataTable dataTable, TextBox directoryField)
        {
            
            if (File.Exists(directoryField.Text + @"\" + create.FileName(sensorModel, sensorSn, comboBox)))
            {
                File.Delete(directoryField.Text + @"\" + create.FileName(sensorModel, sensorSn, comboBox));
            }
            var dataWriter = File.AppendText(directoryField.Text + @"\" + create.FileName(sensorModel, sensorSn, comboBox));
            CsvWriter csv = new CsvWriter(dataWriter, CultureInfo.InvariantCulture);
            foreach (DataColumn column in dataTable.Columns)
            {
                csv.WriteField(column.ColumnName);
            }
            csv.NextRecord();
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    csv.WriteField(row[i]);
                }
                csv.NextRecord();
            }
            dataWriter.Close();

        }
        public void CsvAppendBelow(TextBox sensorModel, TextBox sensorSn, ComboBox comboBox, DataTable dataTable, TextBox directoryField)
        {
            var dataWriter = File.AppendText(directoryField.Text);
            CsvWriter csv = new CsvWriter(dataWriter, CultureInfo.InvariantCulture);
            foreach (DataColumn column in dataTable.Columns)
            {
                csv.WriteField(column.ColumnName);
            }
            csv.NextRecord();
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    csv.WriteField(row[i]);
                }
                csv.NextRecord();
            }
            dataWriter.Close();
        }
        public void CsvAppendAdjacent(TextBox sensorModel, TextBox sensorSn, ComboBox comboBox, DataTable dataTable, TextBox directoryField)
        {
            IEnumerable<string> input;
            string[] output;
            string[] newValues;
            string concatData;
            string[] columnNames;
            string columnNamess;
            int i;
            int multi;
            int next = -1;
            while ((input = File.ReadLines(directoryField.Text)) != null)
            {
                output = input.ToArray();
                multi = (output.Length / 19) - 1;
                i = multi * 19;
                if (next==-1)
                {
                    next++;
                    columnNames = dataTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName.ToString()).ToArray();
                    columnNamess = string.Join(",", columnNames);
                    output[i] = output[i] + "," + columnNamess;
                    File.WriteAllLines(directoryField.Text, output);
                    next--;
                    
                }
                else if (next<18)
                {
                    output = input.ToArray();
                    
                    newValues = dataTable.Rows[next].ItemArray.Select(x => x.ToString()).ToArray();
                    concatData = string.Join(",", newValues);
                    
                    output[i + next + 1] = output[i + next + 1] + "," + concatData;
                    File.WriteAllLines(directoryField.Text, output);

                }
                if (next != 18)
                {
                    next++;
                }
                else if (next == 18)
                {
                    return;
                }
            }
        }
    }
}
