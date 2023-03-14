using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace sPlotter {
    public partial class Form1 : Form {


        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        public Thread readThread;
        public String ReadSerialVal;

        static bool _continue = false;
        public static SerialPort _serialPort = new SerialPort();

        public String touched_temp = "0";
        public int pos_x = 0, pos_y = 0, m_x, m_y;
        public int a, b;

        public int total_x, total_y;


        public Form1() {
            InitializeComponent();
        }


        private void comboBox1_MouseClick(object sender, MouseEventArgs e) {
            comboBox1.Items.Clear();
            foreach (string s in SerialPort.GetPortNames()) {
                comboBox1.Items.Add(s);
            }
        }

        private void Form1_Load(object sender, EventArgs e) {
          

            double[] x = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            double[] y = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            formsPlot1.Plot.AddScatter(x, y);
            formsPlot1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e) {

            if (!_continue) {

                StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;



                // Allow the user to set the appropriate properties.
                _serialPort.PortName = comboBox1.Text;
                _serialPort.BaudRate = int.Parse(textBox1.Text);

                // Set the read/write timeouts
                _serialPort.ReadTimeout = 500;
                _serialPort.WriteTimeout = 500;

                try {
                    _serialPort.Open();
                    button1.BackColor = Color.Red;
                    _continue = true;
                    button1.Text = "Close";
                    readThread = new Thread(Read);
                    readThread.Start();

                } catch (Exception eee) {
                    MessageBox.Show(eee.Message);
                }

            } else {
                button1.BackColor = Color.OliveDrab;
                button1.Text = "Begin";
                //readThread.Abort();
                _serialPort.Close();
                _continue = false;

            }

        }

        private void UpdateLabel(string newText) {
          
            richTextBox1.AppendText(newText+"\n");
            richTextBox1.ScrollToCaret();
        }


        public delegate void ShowSerialData(String r);
        private void Read() {
            while (_continue) {
                try {

                    ReadSerialVal = _serialPort.ReadLine();
                    if (richTextBox1.InvokeRequired) {
                        ShowSerialData ssd = UpdateLabel;
                        Invoke(ssd, ReadSerialVal);
                    } else {
                        richTextBox1.AppendText(ReadSerialVal);
                        richTextBox1.ScrollToCaret();
                    }

                } catch (TimeoutException) {

                } catch (Exception ex) {
                    break;
                }
            }
        }


        private void richTextBox1_TextChanged(object sender, EventArgs e) {

        }

        private void pictureBox3_Click(object sender, EventArgs e) {
            Process.Start("https://www.youtube.com/tnowroz");
        }

        private void pictureBox2_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/TNeutron");
        }

        private void pictureBox1_Click(object sender, EventArgs e) {
            Process.Start("https://www.linkedin.com/in/tnowroz/");
        }

        private void pictureBox4_Click(object sender, EventArgs e) {
            this.Close();
        }


        private void panel2_MouseDown(object sender, MouseEventArgs e) {

            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
           
        }

        


    }

}
