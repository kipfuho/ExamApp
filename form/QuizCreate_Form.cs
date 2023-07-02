using System;
using System.Windows.Forms;

namespace ExamApp
{
    public partial class QuizCreate_Form : Form
    {
        public QuizCreate_Form()
        {
            InitializeComponent();
        }

        public Button CreateQuizButton
        {
            get { return button1; }
        }

        public Button CANCELbutton
        {
            get { return button2; }
        }

        public TextBox QuizName
        {
            get { return QzName; }
        }

        public TextBox TimeCoeff
        {
            get { return timelim; }
        }

        public RichTextBox QuizDescription
        {
            get { return QzText; }
        }

        public CheckBox CBTimeO
        {
            get { return checkBox2; }
        }
        public CheckBox CBTimeC
        {
            get { return checkBox3; }
        }
        public CheckBox CBTimeL
        {
            get { return checkBox4; }
        }

        public ComboBox TimeUnit
        {
            get { return comboBox1; }
        }

        public DateTimePicker DTOpen
        {
            get { return DtOpen; }
        }

        public DateTimePicker DTClose
        {
            get { return DtClose; }
        }

        public Panel MainPanel
        {
            get { return panel1; }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            if (panel7.Visible == true)
            {
                panel7.Hide();
                this.label3.Image = Properties.Resources.icon1;
            }
            else
            {
                panel7.Show();
                this.label3.Image = Properties.Resources.icon2;
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {
            if (panel9.Visible == true)
            {
                panel9.Hide();
                this.label10.Image = Properties.Resources.icon1;
            }
            else
            {
                panel9.Show();
                this.label10.Image = Properties.Resources.icon2;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.DtOpen.Enabled == false)
            {
                this.DtOpen.Enabled = true;
            }
            else
            {
                this.DtOpen.Enabled = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (this.DtClose.Enabled == false)
            {
                this.DtClose.Enabled = true;
            }
            else
            {
                this.DtClose.Enabled = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (this.TimeCoeff.Enabled == false)
            {
                this.TimeCoeff.Enabled = true;
                this.comboBox1.Enabled = true;
            }
            else
            {
                this.TimeCoeff.Enabled = false;
                this.comboBox1.Enabled = false;
            }
        }
    }
}
