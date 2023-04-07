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
    public partial class ExamApp : System.Windows.Forms.Form
    {
        private List<Question> questions;
        public ExamApp()
        {
            InitializeComponent();
            questions = new List<Question>();
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
                Name = questionname_textbox.Text,
                Description = questiontext_textbox.Text,
                Mark = Convert.ToDouble(defaultmark_textbox.Text)
            };
            questions.Add(question);
        }

        private void questionname_textbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void questiontext_textbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            MessageBox.Show(questions[0].Name + questions[0].Description + questions[0].Mark);
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
            EditQuestionPanel.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

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
    }
}
