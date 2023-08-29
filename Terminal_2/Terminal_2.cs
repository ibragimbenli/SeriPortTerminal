using System;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace Terminal_1
{
    public partial class Terminal_2 : Form
    {
        private SerialPort serialPort;
        public Terminal_2()
        {
            InitializeComponent();

            string[] availablePorts = SerialPort.GetPortNames();
            serialPortComboBox.Items.AddRange(availablePorts);
            if (availablePorts.Length <= 0) return;
            serialPortComboBox.Text = availablePorts[availablePorts.Length - 1];
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

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string receivedDatam = serialPort.ReadLine();
            var data1 = serialPort.ReadByte();
            var data2 = serialPort.ReadChar();
            var data3 = serialPort.ReadExisting();
            var data4 = serialPort.ReadTo(receivedDatam);
            byte[] asciiBytes = Encoding.ASCII.GetBytes(receivedDatam);
            var data5 = serialPort.Read(asciiBytes, 0, 11);



            bool existing = receivedDatam.Replace("-", " ").Trim().Contains("06 07 81");
            bool NoExisting = receivedDatam.Replace("-", " ").Trim().Contains("06 07 81 01");
            if (receivedDatam.Length < 3) return;
            var cont = receivedDatam[receivedDatam.Length - 4].ToString() + receivedDatam[receivedDatam.Length - 3].ToString();

            this.Invoke((MethodInvoker)delegate
            {
                if (existing == true && NoExisting == false && cont != "01")
                {
                    var date = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond;
                    var hexDataYaz = receivedDatam + " - " + date;
                    File.AppendAllText(@"C:\Users\ibrahim.benli\Desktop\RS323Test.txt", hexDataYaz + Environment.NewLine);

                    richTextBoxReceivedData.Text += receivedDatam + " " + Environment.NewLine;
            }
            });
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
                //serialPort.WriteLine(asciiBytes.ToString());
                serialPort.WriteLine(txtBoxSendData.Text);
                richTextBoxReceivedData.Text = txtBoxSendData.Text +"\n";
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