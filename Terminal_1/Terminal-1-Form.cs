using System;
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
        string glbReceivedData = "";
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

            string textWithoutSeparators = txtExistSearch.Text.Replace("-", "");

            //string newText = InsertSeparators(textWithoutSeparators, 2, "-");

            //txtExistSearch.Text = newText;

            txtExistSearch.SelectionStart = textWithoutSeparators.Length + (textWithoutSeparators.Length / 2);

            txtExistSearch.TextChanged += txtExistSearch_TextChanged;
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
            int bytesToRead = serialPort.BytesToRead;
            //int bytesToReadd = serialPort.ReadByte();
            byte[] buffer = new byte[bytesToRead];
            serialPort.Read(buffer, 0, bytesToRead);

            string receivedData = Encoding.ASCII.GetString(buffer);

            if (glbReceivedData.Length < 32)
            {
                glbReceivedData += receivedData;
            }
            else
            {
                if (glbReceivedData.Contains("06-07-81") && glbReceivedData.Contains("06-07-81-01") == false)
                {
                    Invoke(new Action(() => richTextBoxReceivedData.Text += glbReceivedData));
                    var date = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond;
                    var hexDataYaz = receivedData + " - " + date;

                    File.AppendAllText(@"C:\Users\ibrahim.benli\Desktop\RS323Test.txt", hexDataYaz + Environment.NewLine);
                }
                glbReceivedData = "";
            }
           
            //bool existing = receivedData.Replace("-", " ").Trim().Contains("06 07 81");
            //bool NoExisting = receivedData.Replace("-", " ").Trim().Contains(txtNoExistSearch.Text);
            //if (receivedData.Length < 4) return;
            ////var cont = receivedData[receivedData.Length - 4].ToString() + receivedData[receivedData.Length - 3].ToString();
            //existing = true;
            //if (existing)
            //{

            //}

        

            //}
            #region Comments Codes
            //string data0 = serialPort.ReadLine();

            //var data1 = serialPort.ReadExisting();
            //MessageBox.Show(data1);

            //int data1 = serialPort.ReadByte();

            ////byte[] asciiBytes = Encoding.ASCII.GetBytes(data1.ToString());
            //string hex = String.Format("{0:X}", Convert.ToInt32(data1));
            //this.Invoke((MethodInvoker)delegate
            //{
            //    richTextBoxReceivedData.Text += hex;
            //});


            //this.Invoke((MethodInvoker)delegate
            //{
            //    //byte[] asciiBytess = { Convert.ToByte(data1) }; //Yönet 1 
            //    richTextBoxReceivedData.Text += asciiBytes[0]; //Yöntem 2
            //});

            //int[] dizi = new int[asciiBytes.Length + 1];

            //for (int i = 0; i < asciiBytes.Length; i++)
            //{
            //    dizi[dizi.Length - 1] = hex[i];
            //}

            //this.Invoke((MethodInvoker)delegate
            //{
            //    foreach (int i in dizi)
            //    {
            //        richTextBoxReceivedData.Text = i.ToString();
            //    }
            //});


            //var data4 = serialPort.ReadTo(data1);
            //byte[] asciiBytes = Encoding.ASCII.GetBytes(data1);
            //var data5 = serialPort.Read(asciiBytes, 0, 11);

            //char[] data = receivedDatam.ToCharArray();

            //byte[] binaryData = HexStringToByteArray(receivedDatam);

            //byte[] gelenAscii = Encoding.ASCII.GetBytes(data);

            //this.Invoke((MethodInvoker)delegate
            //{
            //    //foreach (var d in data1)
            //{
            //string hexx = String.Format("{0:X}", Convert.ToInt32(d));
            ////var hex1 = Convert.ToInt32(d);
            //string hex = Convert.ToString(hex1);
            //richTextBoxReceivedData.Text += hex + " - ";
            //richTextBoxReceivedData.Text += hexx + " - ";
            //richTextBoxReceivedData.Text += d + " " + Environment.NewLine;
            //}

            //for (int i = 0; i < gelenAscii.Length; i++)
            //{
            //    richTextBoxReceivedData.Text += hex + " ";
            //}

            //});
            //string hex = String.Format("{0:X}", Convert.ToInt32(receivedDatam));

            //byte[] asciiBytes = Encoding.ASCII.GetBytes(asciiText);
            //string hexString = BitConverter.ToString(asciiBytes).Replace("-", "");
            //data = ser.readline().decode('utf-8').strip();
            //bool existing = receivedDatam.Replace("-", " ").Trim().Contains("06 07 81");
            //bool NoExisting = receivedDatam.Replace("-", " ").Trim().Contains("06 07 81 01");
            //var cont = receivedDatam[receivedDatam.Length-4].ToString() + receivedDatam[receivedDatam.Length - 3].ToString();

            //if (existing == true && NoExisting == false && cont != "01")
            //{
            //var date = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond;
            //var hexDataYaz = receivedDatam + " - " + date;
            //File.AppendAllText(@"C:\Users\ibrahim.benli\Desktop\RS323Test.txt", hexDataYaz + Environment.NewLine); 
            #endregion
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            //char[] texxten = txtSearch.Text.ToCharArray();
            var data = txtExistSearch.Text;

            string hex = String.Format("{0:X}", Convert.ToInt32(data));

            richTextBoxReceivedData.Text = hex;
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