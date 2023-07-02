using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ExamApp
{
    public partial class Export_Form : Form
    {
        public string exportFilePath;

        public Export_Form()
        {
            InitializeComponent();
        }

        public Label PathLabel
        {
            get { return label2; }
        }

        public Label Progress
        {
            get { return label4; }
        }

        public TextBox PasswordTextBox
        {
            get { return textBox1; }
        }

        public Button ExportButton
        {
            get { return button1; }
        }

        public Button CloseButton
        {
            get { return button2; }
        }

        public Button SelectPathButton
        {
            get { return button3; }
        }

        public Panel ProgressPanel
        {
            get { return panel2; }
        }

        // drag event
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private static extern bool SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox1.UseSystemPasswordChar = false;
            }
            else
            {
                textBox1.UseSystemPasswordChar = true;
            }
        }
    }
}
