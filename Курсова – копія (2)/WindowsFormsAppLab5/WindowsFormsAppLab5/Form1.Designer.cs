using System.IO.Ports;
using System.Text;

namespace WindowsFormsApp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.comPortComboBox = new System.Windows.Forms.ComboBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.serialPort = new System.IO.Ports.SerialPort(this.components);
            this.Slave1Button = new System.Windows.Forms.Button();
            this.Slave2Button = new System.Windows.Forms.Button();
            this.receivedDataTextBox = new System.Windows.Forms.TextBox();
            this.infoLabel = new System.Windows.Forms.Label();
            this.report1Button = new System.Windows.Forms.Button();
            this.report2Button = new System.Windows.Forms.Button();
            this.mySqlCommand1 = new MySql.Data.MySqlClient.MySqlCommand();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // comPortComboBox
            // 
            this.comPortComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comPortComboBox.FormattingEnabled = true;
            this.comPortComboBox.Location = new System.Drawing.Point(755, 31);
            this.comPortComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.comPortComboBox.Name = "comPortComboBox";
            this.comPortComboBox.Size = new System.Drawing.Size(253, 28);
            this.comPortComboBox.TabIndex = 0;
            this.comPortComboBox.SelectedIndexChanged += new System.EventHandler(this.ComPortComboBox_SelectedIndexChanged);
            // 
            // connectButton
            // 
            this.connectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connectButton.Location = new System.Drawing.Point(694, 80);
            this.connectButton.Margin = new System.Windows.Forms.Padding(4);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(113, 38);
            this.connectButton.TabIndex = 1;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // disconnectButton
            // 
            this.disconnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.disconnectButton.Location = new System.Drawing.Point(936, 80);
            this.disconnectButton.Margin = new System.Windows.Forms.Padding(4);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(144, 38);
            this.disconnectButton.TabIndex = 4;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // serialPort
            // 
            this.serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.DataReceivedHandler);
            // 
            // Slave1Button
            // 
            this.Slave1Button.Enabled = false;
            this.Slave1Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Slave1Button.Location = new System.Drawing.Point(619, 137);
            this.Slave1Button.Margin = new System.Windows.Forms.Padding(4);
            this.Slave1Button.Name = "Slave1Button";
            this.Slave1Button.Size = new System.Drawing.Size(255, 37);
            this.Slave1Button.TabIndex = 5;
            this.Slave1Button.Text = "Live data 1";
            this.Slave1Button.UseVisualStyleBackColor = true;
            this.Slave1Button.Click += new System.EventHandler(this.Slave1Button_Click);
            // 
            // Slave2Button
            // 
            this.Slave2Button.Enabled = false;
            this.Slave2Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Slave2Button.Location = new System.Drawing.Point(619, 194);
            this.Slave2Button.Margin = new System.Windows.Forms.Padding(4);
            this.Slave2Button.Name = "Slave2Button";
            this.Slave2Button.Size = new System.Drawing.Size(255, 37);
            this.Slave2Button.TabIndex = 6;
            this.Slave2Button.Text = "Live data 2";
            this.Slave2Button.UseVisualStyleBackColor = true;
            this.Slave2Button.Click += new System.EventHandler(this.Slave2Button_Click);
            // 
            // receivedDataTextBox
            // 
            this.receivedDataTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.854546F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.receivedDataTextBox.Location = new System.Drawing.Point(31, 31);
            this.receivedDataTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.receivedDataTextBox.Multiline = true;
            this.receivedDataTextBox.Name = "receivedDataTextBox";
            this.receivedDataTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.receivedDataTextBox.Size = new System.Drawing.Size(555, 338);
            this.receivedDataTextBox.TabIndex = 8;
            this.receivedDataTextBox.TextChanged += new System.EventHandler(this.receivedDataTextBox_TextChanged);
            // 
            // infoLabel
            // 
            this.infoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLabel.Location = new System.Drawing.Point(619, 260);
            this.infoLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(518, 37);
            this.infoLabel.TabIndex = 3;
            // 
            // report1Button
            // 
            this.report1Button.Enabled = false;
            this.report1Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.report1Button.Location = new System.Drawing.Point(882, 137);
            this.report1Button.Margin = new System.Windows.Forms.Padding(4);
            this.report1Button.Name = "report1Button";
            this.report1Button.Size = new System.Drawing.Size(255, 37);
            this.report1Button.TabIndex = 9;
            this.report1Button.Text = "Minute report 1";
            this.report1Button.UseVisualStyleBackColor = true;
            this.report1Button.Click += new System.EventHandler(this.button1_Click);
            // 
            // report2Button
            // 
            this.report2Button.Enabled = false;
            this.report2Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.report2Button.Location = new System.Drawing.Point(882, 194);
            this.report2Button.Margin = new System.Windows.Forms.Padding(4);
            this.report2Button.Name = "report2Button";
            this.report2Button.Size = new System.Drawing.Size(255, 37);
            this.report2Button.TabIndex = 10;
            this.report2Button.Text = "Minute report 2";
            this.report2Button.UseVisualStyleBackColor = true;
            this.report2Button.Click += new System.EventHandler(this.button2_Click);
            // 
            // mySqlCommand1
            // 
            this.mySqlCommand1.CacheAge = 0;
            this.mySqlCommand1.Connection = null;
            this.mySqlCommand1.EnableCaching = false;
            this.mySqlCommand1.Transaction = null;
            // 
            // chart1
            // 
            chartArea4.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chart1.Legends.Add(legend4);
            this.chart1.Location = new System.Drawing.Point(1171, 31);
            this.chart1.Name = "chart1";
            series7.BorderWidth = 3;
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series7.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            series7.Legend = "Legend1";
            series7.Name = "Sensor1";
            series8.BorderWidth = 3;
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series8.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            series8.Legend = "Legend1";
            series8.Name = "Sensor2";
            this.chart1.Series.Add(series7);
            this.chart1.Series.Add(series8);
            this.chart1.Size = new System.Drawing.Size(507, 425);
            this.chart1.TabIndex = 11;
            this.chart1.Text = "chart";
            this.chart1.Visible = false;
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1924, 1055);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.report2Button);
            this.Controls.Add(this.report1Button);
            this.Controls.Add(this.receivedDataTextBox);
            this.Controls.Add(this.Slave2Button);
            this.Controls.Add(this.Slave1Button);
            this.Controls.Add(this.disconnectButton);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.comPortComboBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Lab5";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comPortComboBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.Timer timer1;
        private System.IO.Ports.SerialPort serialPort;
        private System.Windows.Forms.Button Slave1Button;
        private System.Windows.Forms.Button Slave2Button;
        private System.Windows.Forms.TextBox receivedDataTextBox;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Button report1Button;
        private System.Windows.Forms.Button report2Button;
        private MySql.Data.MySqlClient.MySqlCommand mySqlCommand1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    }
}

