using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ExamApp
{
    public partial class AddQuestionToQuiz1_Form : Form
    {
        public int PageNumber = -1;
        public int currentIndex = -1;
        public int numberOfQuestion = -1;
        public int currentbutton = -1;
        public List<Button> pageButtonList = new List<Button>();

        public AddQuestionToQuiz1_Form()
        {
            InitializeComponent();
            pageButtonList.Add(button1);
            pageButtonList.Add(button2);
            pageButtonList.Add(button3);
            pageButtonList.Add(button4);
            pageButtonList.Add(button5);
            pageButtonList.Add(button6);
            pageButtonList.Add(button7);
            pageButtonList.Add(button8);
            pageButtonList.Add(button9);
            pageButtonList.Add(button10);
            pageButtonList.Add(button12);
        }

        public Button AddButton
        {
            get { return button13; }
        }

        public Button LeftMorePage
        {
            get { return button14; }
        }

        public Button RightMorePage
        {
            get { return button11; }
        }

        public CheckBox SubcategoryCheckBox
        {
            get { return checkBox1; }
        }

        public ComboBox CategoryComboBox
        {
            get { return comboBox1; }
        }

        public ComboBox NumberOfQuestionComboBox
        {
            get { return comboBox2; }
        }

        public Panel QuestionDisplayPanel
        {
            get { return panel3; }
        }

        public Panel PageDirectionPanel
        {
            get { return panel4; }
        }

        public void tabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == tabControl1.SelectedIndex)
            {
                e.Graphics.DrawString(tabControl1.TabPages[e.Index].Text, new Font(tabControl1.Font, FontStyle.Regular), Brushes.Black, new PointF(e.Bounds.X, e.Bounds.Y));
            }
            else
            {
                e.Graphics.DrawString(tabControl1.TabPages[e.Index].Text, new Font(tabControl1.Font, FontStyle.Regular), Brushes.DeepSkyBlue, new PointF(e.Bounds.X, e.Bounds.Y));
            }
        }
    }
}
