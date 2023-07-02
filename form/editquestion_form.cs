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
    public partial class EditQuestion_Form : Form
    {
        public EditQuestion_Form()
        {
            InitializeComponent();
        }

        public TextBox QName
        {
            get { return this.QuestionNameTextBox; }
        }

        public TextBox QMark
        {
            get { return this.DefaultMarkTextBox; }
        }

        public Trestan.TRichTextBox C1Text
        {
            get { return this.ChoiceText1; }
        }

        public Trestan.TRichTextBox C2Text
        {
            get { return this.ChoiceText2; }
        }

        public Trestan.TRichTextBox C3Text
        {
            get { return this.ChoiceText3; }
        }

        public Trestan.TRichTextBox C4Text
        {
            get { return this.ChoiceText4; }
        }

        public Trestan.TRichTextBox C5Text
        {
            get { return this.ChoiceText5; }
        }

        public Trestan.TRichTextBox QText
        {
            get { return this.QuestionTextTextBox; }
        }

        public ComboBox C1Grade
        {
            get { return this.ChoiceGrade1; }
        }

        public ComboBox C2Grade
        {
            get { return this.ChoiceGrade2; }
        }

        public ComboBox C3Grade
        {
            get { return this.ChoiceGrade3; }
        }

        public ComboBox C4Grade
        {
            get { return this.ChoiceGrade4; }
        }

        public ComboBox C5Grade
        {
            get { return this.ChoiceGrade5; }
        }

        public ComboBox QCategory
        {
            get { return CategoryComboBox; }
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

        public Panel SubMainPanel
        {
            get { return panel23; }
        }

        private void MoreChoicesButton_Click(object sender, EventArgs e)
        {
            this.MoreChoicePanel.Show();
        }

        private void label19_Click(object sender, EventArgs e)
        {
            if (panel23.Visible == true)
            {
                panel23.Hide();
                this.label20.Image = Properties.Resources.icon1;
            }
            else
            {
                panel23.Show();
                this.label20.Image = Properties.Resources.icon2;
            }
        }
    }
}
