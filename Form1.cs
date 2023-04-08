using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ExamApp
{
    public partial class ExamApp : System.Windows.Forms.Form
    {
        private List<Question> questions;
        private Category defaultCategory;
        public ExamApp()
        {
            InitializeComponent();
            questions = new List<Question>();
            defaultCategory = new Category
            {
                Name = "Default",
                Info = "Default Category",
                Id = 0,
                QuestionList = questions,
                Child = new List<Category> { }
            };
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MoreChoicePanel.Show();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void editmode_bigpanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void defaultmark_textbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void editquestion_panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Question question = new Question
            {
                Name = QuestionNameTextBox.Text,
                Description = QuestionTextTextBox.Text,
                Mark = Convert.ToDouble(DefaultMarkTextBox.Text),
                Choices = new List<Choice> { }
            };
            QuestionNameTextBox.ResetText();
            QuestionTextTextBox.ResetText();
            DefaultMarkTextBox.Text = "1";
            if (String.IsNullOrWhiteSpace(ChoiceText1.Text) == false)
            {
                double grade = 0;
                if(ChoiceGrade1.Text != "None")
                {
                    grade = double.Parse(ChoiceGrade1.Text.TrimEnd('%')) / 100.0;
                    ChoiceGrade1.Text = "None";
                }
                Choice choice1 = new Choice
                {
                    Text = ChoiceText1.Text,
                    Grade = grade
                };
                question.addChoice(choice1);
                ChoiceText1.ResetText();
            }
            if (String.IsNullOrWhiteSpace(ChoiceText2.Text) == false)
            {
                double grade = 0;
                if (ChoiceGrade2.Text != "None")
                {
                    grade = double.Parse(ChoiceGrade2.Text.TrimEnd('%')) / 100.0;
                    ChoiceGrade2.Text = "None";
                }
                Choice choice2 = new Choice
                {
                    Text = ChoiceText2.Text,
                    Grade = grade
                };
                question.addChoice(choice2);
                ChoiceText2.ResetText();
            }
            if(MoreChoicePanel.Visible == true)
            {
                if (String.IsNullOrWhiteSpace(ChoiceText3.Text) == false)
                {
                    double grade = 0;
                    if (ChoiceGrade3.Text != "None")
                    {
                        grade = double.Parse(ChoiceGrade3.Text.TrimEnd('%')) / 100.0;
                    }
                    Choice choice3 = new Choice
                    {
                        Text = ChoiceText3.Text,
                        Grade = grade
                    };
                    question.addChoice(choice3);
                }
                if (String.IsNullOrWhiteSpace(ChoiceText4.Text) == false)
                {
                    double grade = 0;
                    if (ChoiceGrade4.Text != "None")
                    {
                        grade = double.Parse(ChoiceGrade4.Text.TrimEnd('%')) / 100.0;
                    }
                    Choice choice4 = new Choice
                    {
                        Text = ChoiceText4.Text,
                        Grade = grade
                    };
                    question.addChoice(choice4);
                }
                if (String.IsNullOrWhiteSpace(ChoiceText5.Text) == false)
                {
                    double grade = 0;
                    if (ChoiceGrade5.Text != "None")
                    {
                        grade = double.Parse(ChoiceGrade5.Text.TrimEnd('%')) / 100.0;
                    }
                    Choice choice5 = new Choice
                    {
                        Text = ChoiceText5.Text,
                        Grade = grade
                    };
                    question.addChoice(choice5);
                }
                ChoiceText3.ResetText();
                ChoiceText4.ResetText();
                ChoiceText5.ResetText();
                ChoiceGrade3.Text = "None";
                ChoiceGrade4.Text = "None";
                ChoiceGrade5.Text = "None";
                MoreChoicePanel.Hide();
            }
            questions.Add(question);
            EditQuestionPanel.Hide();
            EditQuestionPanel.AutoScrollPosition = new Point(0, 0);
            EditPanel.Show();
        }

        private void questionname_textbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void questiontext_textbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            QuestionNameTextBox.ResetText();
            QuestionTextTextBox.ResetText();
            DefaultMarkTextBox.Text = "1";
            ChoiceText1.ResetText();
            ChoiceGrade1.Text = "None";
            ChoiceText2.ResetText();
            ChoiceGrade2.Text = "None";
            if(MoreChoicePanel.Visible == true)
            {
                ChoiceText3.ResetText();
                ChoiceGrade3.Text = "None";
                ChoiceText4.ResetText(); 
                ChoiceGrade4.Text = "None";
                ChoiceText5.ResetText();
                ChoiceGrade5.Text = "None";
                MoreChoicePanel.Hide();
            }
            EditQuestionPanel.Hide();
            EditQuestionPanel.AutoScrollPosition = new Point(0, 0);
            EditPanel.Show();
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            PopupPanel.Show();
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            EditPanel.Hide();
            label17.Text = "Adding a Multiple choices question";
            EditQuestionPanel.Show();
        }
        private void Edit_Click(object sender, System.EventArgs e, Question question)
        {
            EditPanel.Hide();
            label17.Text = "Editing a Multiple choices question";
            QuestionNameTextBox.Text = question.Name;
            QuestionTextTextBox.Text = question.Description;
            DefaultMarkTextBox.Text = question.Mark.ToString();
            int numberofchoice = question.Choices.Count;

            if (numberofchoice > 0)
            {
                ChoiceText1.Text = question.Choices[0].Text;
                if (question.Choices[0].Grade == 0)
                {
                    ChoiceGrade1.Text = "None";
                }
                else
                {
                    ChoiceGrade1.Text = (question.Choices[0].Grade*100.0).ToString() + "%";
                }
            }

            if (numberofchoice > 1)
            {
                ChoiceText2.Text = question.Choices[1].Text;
                if (question.Choices[1].Grade == 0)
                {
                    ChoiceGrade2.Text = "None";
                }
                else
                {
                    ChoiceGrade2.Text = (question.Choices[1].Grade * 100.0).ToString() + "%";
                }
            }

            if (numberofchoice > 2)
            {
                MoreChoicePanel.Show();
                ChoiceText3.Text = question.Choices[2].Text;
                if (question.Choices[2].Grade == 0)
                {
                    ChoiceGrade3.Text = "None";
                }
                else
                {
                    ChoiceGrade3.Text = (question.Choices[2].Grade * 100.0).ToString() + "%";
                }

                if (numberofchoice > 3)
                {
                    ChoiceText4.Text = question.Choices[3].Text;
                    if (question.Choices[3].Grade == 0)
                    {
                        ChoiceGrade4.Text = "None";
                    }
                    else
                    {
                        ChoiceGrade4.Text = (question.Choices[3].Grade * 100.0).ToString() + "%";
                    }
                }

                if (numberofchoice > 4)
                {
                    ChoiceText5.Text = question.Choices[4].Text;
                    if (question.Choices[4].Grade == 0)
                    {
                        ChoiceGrade5.Text = "None";
                    }
                    else
                    {
                        ChoiceGrade5.Text = (question.Choices[4].Grade * 100.0).ToString() + "%";
                    }
                }
            }
            EditQuestionPanel.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                QuestionFlowLayout.Controls.Clear();
                foreach (Question question in questions)
                {
                    QuestionBlock temp = new QuestionBlock(question);
                    temp.Edit.Click += delegate(object sender1, EventArgs e1) { this.Edit_Click(sender1, e1, temp.Question); };
                    QuestionFlowLayout.Controls.Add(temp);
                }
                QuestionFlowLayout.Show();
            }
            else
            {
                QuestionFlowLayout.Hide();
            }
        }

        private void morechoices_button_Click(object sender, EventArgs e)
        {
            if(MoreChoicePanel.Visible == false)
            {
                MoreChoicePanel.Show();
            }
        }

        private void label24_Click(object sender, EventArgs e)
        {
            EditPanel.Hide();
            HomePanel.Show();
        }

        private void label19_Click_1(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {
            EditQuestionPanel.Hide();
            HomePanel.Show();
        }

        private void label25_Click(object sender, EventArgs e)
        {
            EditQuestionPanel.Hide();
            EditPanel.Show();
        }

        private void savecontinue_button_Click(object sender, EventArgs e)
        {

        }

        private void label31_Click(object sender, EventArgs e)
        {

        }

        private void label29_Click(object sender, EventArgs e)
        {
            EditPanel.Hide();
            HomePanel.Show();
        }

        private void label18_Click_1(object sender, EventArgs e)
        {

        }

        private void label19_Click_2(object sender, EventArgs e)
        {
            PopupPanel.Hide();
            HomePanel.Hide();
            EditPanel.Show();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint_2(object sender, PaintEventArgs e)
        {

        }
    }
}
