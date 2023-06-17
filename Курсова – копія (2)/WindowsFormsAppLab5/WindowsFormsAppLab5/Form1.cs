using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.Threading;
using WindowsFormsAppLab2;
using MindFusion.Charting;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {

        private readonly Permission _permission;

        private int _countSeconds = 0;

        private readonly byte[] crcLookupTable = new byte[256]; // Lookup table for CRC calculation

        private System.Timers.Timer timer;

        private bool isTimer2Active = false;
        private bool liveData1 = false;
        private bool liveData2 = false;

        private DateTime lastData = DateTime.Now;


        private string selectedPort;
        private byte checkSumValueReceived;

        const byte addressSlave1 = 53;
        const byte addressSlave2 = 25;
        private const byte msgLengthInBytesSlave1 = 3;
        private const byte msgLengthInBytesSlave2 = 3;

        private byte currentByteReceivingCount = 0;

        bool receivingFromSlave1 = false;
        bool receivingFromSlave2 = false;
        private List<byte> receivedDataByteList = new List<byte>();

        private readonly Dictionary<int, string> hexTolettersMapping = new Dictionary<int, string>() {
            { 0x01, "1" }, { 0x02, "2" }, { 0x03, "3" }, { 0x04, "4" }, { 0x05, "5" },
            { 0x06, "6" }, { 0x07, "7" }, { 0x08, "8" }, { 0x09, "9" }, { 0x00, "0" }
        };

        public Form1(Permission permission)
        {
            this._permission = permission;
            InitializeComponent();
            InitializeTimer();
            FillCRCLookupTable();

            // Set the form's window state to maximize
            this.WindowState = FormWindowState.Maximized;
        }

        private void FillCRCLookupTable()
        {
            byte poly = 0x2F; // CRC polynomial

            for (int i = 0; i < 256; i++)
            {
                byte crc = (byte)i;
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x80) != 0)
                    {
                        crc = (byte)((crc << 1) ^ poly);
                    }
                    else
                    {
                        crc <<= 1;
                    }
                }
                crcLookupTable[i] = crc;
            }
        }

        private void InitializeTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 500; // 0.5 second
            timer.Elapsed += TimerElapsed;
            timer.AutoReset = true;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (isTimer2Active)
            {
                getDataFromSlave2(null, null);
            }
            else
            {
                getDataFromSlave1(null, null);
            }
            isTimer2Active = !isTimer2Active;
            TimeSpan duration = DateTime.Now - lastData;
            double seconds = duration.TotalSeconds;
            if (seconds > 3)
            {
                reconnect();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisY.Maximum = 40;
            chart1.ChartAreas[0].AxisY.Minimum = -5;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "H:mm:ss";
            chart1.Series[0].XValueType = ChartValueType.DateTime;
            chart1.ChartAreas[0].AxisX.Minimum = DateTime.Now.ToOADate();
            chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddMinutes(1).ToOADate();
            chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.Interval = 5;
            string[] portNames = SerialPort.GetPortNames();
            string[] ports = portNames.OrderBy(a => a.Length > 3 && int.TryParse(a.Substring(3), out int num) ? num : 0).ToArray();
            comPortComboBox.Items.AddRange(ports);
        }

        private void ComPortComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedPort = comPortComboBox.SelectedItem.ToString();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort.PortName = selectedPort;
                serialPort.BaudRate = 9600;
                serialPort.Open();
                serialPort.Encoding = Encoding.UTF8;
                infoLabel.ForeColor = Color.Green;
                infoLabel.Text = "Connection successful";
                if(_permission==Permission.admin)
                {
                    Slave1Button.Enabled = true;
                    Slave2Button.Enabled = true;
                    report1Button.Enabled = true;
                    report2Button.Enabled = true;
                    chart1.Visible = true;
                }
                if (_permission == Permission.sensor1)
                {
                    Slave1Button.Enabled = true;
                    report1Button.Enabled = true;
                }
                if (_permission == Permission.sensor2)
                {
                    Slave2Button.Enabled = true;
                    report2Button.Enabled = true;
                }
                if (_permission == Permission.report1)
                {
                    report1Button.Enabled = true;
                }
                if (_permission == Permission.report2)
                {
                    report2Button.Enabled = true;
                }
                connectButton.Enabled = false;
                disconnectButton.Enabled = true;
                comPortComboBox.Enabled = false;
                timer.Start(); // Start the timer when the connection is successful

            }
            catch (Exception ex)
            {
                infoLabel.ForeColor = Color.Red;
                infoLabel.Text = "Connection failed: " + ex.Message;
            }
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort.Close();
                infoLabel.ForeColor = Color.Green;
                infoLabel.Text = "Successfully disconnected";
                disconnectButton.Enabled = false;
                connectButton.Enabled = true;
                comPortComboBox.Enabled = true;
                Slave1Button.Enabled = false;
                Slave2Button.Enabled = false;
                report1Button.Enabled = false;
                report2Button.Enabled = false;
                timer.Stop(); // Stop the timer when disconnecting


                currentByteReceivingCount = 0;
                receivingFromSlave1 = false;
                receivingFromSlave2 = false;
            }
            catch (Exception ex)
            {
                infoLabel.ForeColor = Color.Red;
                infoLabel.Text = "Failed to disconnect: " + ex.Message;
            }
        }



        private void getDataFromSlave1(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                byte[] b1 = new byte[1] { addressSlave1 };
                serialPort.Write(b1, 0, 1);
            }
        }

        private void getDataFromSlave2(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                byte[] b2 = new byte[1] { addressSlave2 };
                serialPort.Write(b2, 0, 1);
            }
        }

        private void Slave1Button_Click(object sender, EventArgs e)
        {
            SetText("Live data for sensor 1:");
            SetText(Environment.NewLine);
            liveData2 = false;
            liveData1 = true;
        }

        private void Slave2Button_Click(object sender, EventArgs e)
        {
            SetText("Live data for sensor 2:");
            SetText(Environment.NewLine);
            liveData1 = false;
            liveData2 = true;
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            byte receivedByte = (byte)sp.ReadByte();

            ProcessReceivedByte(receivedByte, msgLengthInBytesSlave1);

        }

        private void reconnect()
        {
            serialPort.Close();
            timer.Stop(); // Stop the timer when disconnecting
            currentByteReceivingCount = 0;
            receivingFromSlave1 = false;
            receivingFromSlave2 = false;
            Thread.Sleep(100);
            serialPort.PortName = selectedPort;
            serialPort.BaudRate = 9600;
            serialPort.Open();
            serialPort.Encoding = Encoding.UTF8;
            timer.Start(); // Start the timer when the connection is successful
        }

        private void ProcessReceivedByte(byte receivedByte, int messageLength)
        {

            if (currentByteReceivingCount == 0)
            {

                if (receivedByte == addressSlave1)
                {
                    receivingFromSlave2 = false;
                    receivingFromSlave1 = true;
                }
                if (receivedByte == addressSlave2)
                {
                    receivingFromSlave1 = false;
                    receivingFromSlave2 = true;
                }
                currentByteReceivingCount++;
                return;
            }
            if (currentByteReceivingCount < messageLength - 1)
            {
                receivedDataByteList.Add(receivedByte);
            }
            else
            {
                checkSumValueReceived = receivedByte;
            }
            currentByteReceivingCount++;
            if (currentByteReceivingCount == messageLength)
            {
                byte[] bytes = receivedDataByteList.ToArray();
                ushort mycheckSumResult = CalculateCrc8(bytes);
                if (mycheckSumResult != checkSumValueReceived)
                {
                    reconnect();
                    /*
                    if ((receivingFromSlave1 && liveData1) || (receivingFromSlave2 && liveData2))
                    {
                   
                        SetText(" (Expected CRC is " + checkSumValueReceived.ToString() + ", calculated " + mycheckSumResult.ToString() + ") ");
                        SetText(Environment.NewLine);
                        reconnect();

                    }
               */
                } else
                {

                    lastData = DateTime.Now;
                    if ((receivingFromSlave1 && liveData1) || (receivingFromSlave2 && liveData2))
                    {
                        SetText(lastData + " " + bytes[0].ToString());
                        SetText(Environment.NewLine);

                    }
                    // write to db
                    if (receivingFromSlave1)
                    {
                        DbUtils.InsertData(1, bytes[0], lastData);
                    }
                    if (receivingFromSlave2)
                    {
                        DbUtils.InsertData(2, bytes[0], lastData);
                    }
                }
                receivedDataByteList = new List<byte>();
                checkSumValueReceived = 0;
                /*if ((receivingFromSlave1 && liveData1) || (receivingFromSlave2 && liveData2))
                {
                    SetText(Environment.NewLine);
                }*/
                currentByteReceivingCount = 0;
            }
        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            if (receivedDataTextBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                Invoke(d, new object[] { text });
            }
            else
            {
                if (int.TryParse(text, out int keyToConvert) && hexTolettersMapping.TryGetValue(keyToConvert, out string value))
                {
                    receivedDataTextBox.AppendText(value);
                }
                else
                {
                    receivedDataTextBox.AppendText(text);
                }
            }
        }

        byte CalculateCrc8(byte[] data)
        {
            byte crc = 0xFF;

            for (int i = 0; i < data.Length; i++)
            {
                crc = crcLookupTable[crc ^ data[i]];
            }

            return crc;
        }

        private void receivedDataTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            liveData2 = false;
            liveData1 = false;
            SetText("Report for sensor 1:");
            SetText(Environment.NewLine);
            int maxTemperature = DbUtils.GetMaxTemperature(1, 60);
            SetText($"Max temperature: {maxTemperature} °C");
            SetText(Environment.NewLine);
            int minTemperature = DbUtils.GetMinTemperature(1, 60);
            SetText($"Min temperature: {minTemperature} °C");
            SetText(Environment.NewLine);
            int avgTemperature = DbUtils.GetAvgTemperature(1, 60);
            SetText($"Average temperature: {avgTemperature} °C");
            SetText(Environment.NewLine);
            int latestTemperature = DbUtils.GetLatestTemperature(1);
            SetText($"Latest temperature: {latestTemperature} °C");
            SetText(Environment.NewLine);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            liveData2 = false;
            liveData1 = false;
            SetText("Report for sensor 2:");
            SetText(Environment.NewLine);
            int maxTemperature = DbUtils.GetMaxTemperature(2, 60);
            SetText($"Max temperature: {maxTemperature} °C");
            SetText(Environment.NewLine);
            int minTemperature = DbUtils.GetMinTemperature(2, 60);
            SetText($"Min temperature: {minTemperature} °C");
            SetText(Environment.NewLine);
            int avgTemperature = DbUtils.GetAvgTemperature(2, 60);
            SetText($"Average temperature: {avgTemperature} °C");
            SetText(Environment.NewLine);
            int latestTemperature = DbUtils.GetLatestTemperature(2);
            SetText($"Latest temperature: {latestTemperature} °C");
            SetText(Environment.NewLine);
        }

        private void lineChart1_DataItemClicked(object sender, HitResult e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            int value1 = Convert.ToInt32(DbUtils.GetLatestTemperature(1));
            int value2 = Convert.ToInt32(DbUtils.GetLatestTemperature(2));
            chart1.Series[0].Points.AddXY(dateTime,value1);
            chart1.Series[1].Points.AddXY(dateTime,value2);

            _countSeconds++;
            if(_countSeconds == 60)
            {
                _countSeconds = 0;
                chart1.ChartAreas[0].AxisX.Minimum = DateTime.Now.ToOADate();
                chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddMinutes(1).ToOADate();
                chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
                chart1.ChartAreas[0].AxisX.Interval = 5;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
