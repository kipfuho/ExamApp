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
    public partial class editquestion : Form
    {
        public editquestion()
        {
            InitializeComponent();
        } 

        public TextBox QName
        {
            get { return this.QuestionNameTextBox; }
        }

        public RichTextBox QText
        {
            get { return this.QuestionTextTextBox; }
        }

        public TextBox QMark
        {
            get { return this.DefaultMarkTextBox; }
        }

        public TextBox C1Text
        {
            get { return this.ChoiceText1; }
        }

        public ComboBox C1Grade
        {
            get { return this.ChoiceGrade1; }
        }

        public TextBox C2Text
        {
            get { return this.ChoiceText2; }
        }
        public ComboBox C2Grade
        {
            get { return this.ChoiceGrade2; }
        }

        public TextBox C3Text
        {
            get { return this.ChoiceText3; }
        }
        public ComboBox C3Grade
        {
            get { return this.ChoiceGrade3; }
        }

        public TextBox C4Text
        {
            get { return this.ChoiceText4; }
        }
        public ComboBox C4Grade
        {
            get { return this.ChoiceGrade4; }
        }

        public TextBox C5Text
        {
            get { return this.ChoiceText5; }
        }
        public ComboBox C5Grade
        {
            get { return this.ChoiceGrade5; }
        }

        public Button FunctionButton1
        {
            get { return this.SaveQuitButton; }
        }

        public Button FunctionButton2
        {
            get { return this.CancelButton; }
        }

        public Button FunctionButton3
        {
            get { return this.SaveContinueButton; }
        }

        public Label BigLabel
        {
            get { return label17; }
        }

        public Panel MainPanel
        {
            get { return EditQuestionPanel; }
        }

        public Panel MoreChoiceP
        {
            get { return MoreChoicePanel; }
        }

        public Panel ChoiceP
        {
            get { return ChoicePanel; }
        }

        private void MoreChoicesButton_Click(object sender, EventArgs e)
        {
            this.MoreChoicePanel.Show();
        }

        private void SaveQuitButton_Click(object sender, EventArgs e)
        {
            this.MoreChoicePanel.Hide();
        }
    }
}
