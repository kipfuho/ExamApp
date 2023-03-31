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
    public partial class Form1 : Form
    {
        private List<Question> questions;
        public Form1()
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
            panel4.Show();
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
            Question question = new Question();
            question.QuestionName = questionname_textbox.Text;
            question.QuestionText = questiontext_textbox.Text;
            question.DefaultMark = Convert.ToDouble(defaultmark_textbox.Text);
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
            MessageBox.Show(questions[0].QuestionName + questions[0].QuestionText + questions[0].DefaultMark);
        }
    }
}
