using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExamApp
{
    public partial class QuizEdit_Form : Form
    {
        public QuizEdit_Form()
        {
            InitializeComponent();
        }

        public void Add_Click(object sender, EventArgs e)
        {
            if (popupPanel.Visible == false)
            {
                popupPanel.Location = new Point(this.panel17.Right - popupPanel.Width, this.panel17.Bottom);
                popupPanel.Show();
            }
            else
            {
                popupPanel.Hide();
            }
        }

        public CheckBox ShuffleBox
        {
            get { return shuffleCheckBox; }
        }

        public Label AddNewQuestionLabel
        {
            get { return this.add1Label; }
        }

        public Label AddFromQuestionBankLabel
        {
            get { return this.add2Label; }
        }

        public Label AddRandomQuestionLabel
        {
            get { return this.add3Label; }
        }

        public Label QuestionNumberLabel
        {
            get { return this.questionNumberLabel; }
        }

        public Label TotalMarkLabel
        {
            get { return this.totalMarkLabel; }
        }

        public Label QuestionNameLabel
        {
            get { return this.label2; }
        }

        public Label PageNumberLabel
        {
            get { return this.pageNumberLabel; }
        }

        public TextBox GradeTextBox
        {
            get { return this.textBox1; }
        }

        public Button SaveQuizBtn
        {
            get { return button1; }
        }

        public Panel Popup
        {
            get { return popupPanel; }
        }

        public Panel QuestionLayout
        {
            get { return questionLayout; }
        }

        public FlowLayoutPanel PageFlowLayout
        {
            get { return pageFlowLayout; }
        }
    }
}
