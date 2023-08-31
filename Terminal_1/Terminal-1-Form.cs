﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Windows.Forms;

namespace Terminal_1
{
    public partial class Terminal_1 : Form
    {
        private SerialPort serialPort;
        public Terminal_1()
        {
            InitializeComponent();

            string[] availablePorts = SerialPort.GetPortNames();
            serialPortComboBox.Items.AddRange(availablePorts);
            if (availablePorts.Length <= 0) return;
            serialPortComboBox.Text = availablePorts[0];
            serialPort = new SerialPort();
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), comboParity.Text);
            serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), comboStopBit.Text); ;
            serialPort.DataBits = Convert.ToInt32(comboDataBit.Text);
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            btnClose.Enabled = false;
            btnOpen.Enabled = true;
            btnSend.Enabled = false;
        }

        private void txtBoxSendData_TextChanged(object sender, EventArgs e)
        {
            txtBoxSendData.TextChanged -= txtBoxSendData_TextChanged;

            string textWithoutSeparators = txtBoxSendData.Text.Replace("-", "");

            string newText = InsertSeparators(textWithoutSeparators, 2, "-");

            txtBoxSendData.Text = newText;

            txtBoxSendData.SelectionStart = textWithoutSeparators.Length + (textWithoutSeparators.Length / 2);

            txtBoxSendData.TextChanged += txtBoxSendData_TextChanged;
        }
        private void txtExistSearch_TextChanged(object sender, EventArgs e)
        {
            txtBoxSendData.TextChanged -= txtExistSearch_TextChanged;

            string textWithoutSeparators = txtExistSearch0.Text.Replace("-", "");

            //string newText = InsertSeparators(textWithoutSeparators, 2, "-");

            //txtExistSearch.Text = newText;

            txtExistSearch0.SelectionStart = textWithoutSeparators.Length + (textWithoutSeparators.Length / 2);

            txtExistSearch0.TextChanged += txtExistSearch_TextChanged;
        }
        private void txtNoExistSearch_TextChanged(object sender, EventArgs e)
        {
            txtNoExistSearch.TextChanged -= txtNoExistSearch_TextChanged;

            string textWithoutSeparators = txtNoExistSearch.Text.Replace("-", "");

            string newText = InsertSeparators(textWithoutSeparators, 2, "-");

            txtNoExistSearch.Text = newText;

            txtNoExistSearch.SelectionStart = textWithoutSeparators.Length + (textWithoutSeparators.Length / 2);

            txtNoExistSearch.TextChanged += txtNoExistSearch_TextChanged;
        }

        private string InsertSeparators(string input, int interval, string separator)
        {
            if (input.Length >= 22)
                MessageBox.Show("En çok 11 karakter");
            for (int i = interval; i < input.Length; i += interval + separator.Length)
            {
                input = input.Insert(i, separator);
            }
            return input;
        }

        static byte[] HexStringToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];

            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int DataCount = Convert.ToInt32(txtDataCount.Text);
            
            int bytesToRead = serialPort.BytesToRead;
            while (bytesToRead < DataCount)
            {
                bytesToRead = serialPort.BytesToRead;
            }

            byte[] buffer = new byte[bytesToRead];
            serialPort.Read(buffer, 0, bytesToRead);

            var val1 = Convert.ToInt32(txtExistSearch0.Text);
            var val2 = Convert.ToInt32(txtExistSearch1.Text);
            var val3 = Convert.ToInt32(txtExistSearch2.Text);
            var val4 = Convert.ToInt32(txtNoExistSearch.Text);
            string Hexdata = "";

            if (buffer[0] == val1 && buffer[1] == val2 && buffer[2] == val3 && buffer[3] != val4)
            {
                for (int i = 0; i < bytesToRead; i++)
                {
                    Hexdata = Hexdata + String.Format("{0:X}", Convert.ToInt32(buffer[i])) + " ";

                }
                Invoke(new Action(() => richTextBoxReceivedData.Text += Hexdata + Environment.NewLine));
                var date = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond;
                var hexDataYaz = Hexdata + " - " + date;

                File.AppendAllText(@"C:\Users\ibrahim.benli\Desktop\RS323Test.txt", hexDataYaz);
            }
            File.AppendAllText(@"C:\Users\ibrahim.benli\Desktop\RS323Test.txt", Environment.NewLine);
        }
        private void btnConvert_Click(object sender, EventArgs e)
        {
            //richTextBoxReceivedData.Text = "";
            //char[] texxten = txtSearch.Text.ToCharArray();
            var data = txtBoxSendData.Text;
            data = data.Trim().Replace("-","");
            string hex = String.Format("{0:X}", Convert.ToInt32(data));

            richTextBoxReceivedData.Text += $"Klavyeden girilen sayı: '{data}' - HexaDecimal: '{hex}' \n";
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {

            if (serialPort == null)
            {
                MessageBox.Show("Bağlanacak Cihaz Bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!serialPort.IsOpen)
            {
                serialPort.PortName = serialPortComboBox.SelectedItem.ToString();
                try
                {
                    if (Convert.ToInt32(comboDataBit.Text).GetType() == typeof(string) || comboDataBit.Text == "")
                    {
                        MessageBox.Show("Lütfen bir sayısal Baud Değeri Giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        serialPort.BaudRate = int.Parse(comboBaudRate.Text); //Set your desired baud rate
                        serialPort.Open();
                        btnOpen.Enabled = false;
                        btnClose.Enabled = true;
                        btnSend.Enabled = true;
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message} Com Erişimi Reddetti!");
                    return;
                }
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
                btnOpen.Enabled = true;
                btnClose.Enabled = false;
                btnSend.Enabled = false;
            }
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                //byte[] asciiBytes = Encoding.ASCII.GetBytes(txtBoxSendData.Text);
                //serialPort.WriteLine(asciiBytes[0].ToString());

                serialPort.WriteLine(txtBoxSendData.Text);
            }
        }
        private void txtBoxSendData_Click(object sender, EventArgs e)
        {
            txtBoxSendData.Text = string.Empty;
        }
        private void btnClean_Click(object sender, EventArgs e)
        {
            richTextBoxReceivedData.Text = string.Empty;
        }

        private void txtBoxSendData_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Backspace ve hexadecimal characters (0-9, A-F) izin verme durumu
            if (!char.IsControl(e.KeyChar) && !Uri.IsHexDigit(e.KeyChar))
            {
                e.Handled = true; //hexdecimal değilse tuşa basamasın
            }
        }

    }
}