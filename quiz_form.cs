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
    public partial class quiz_form : Form
    {
        public quiz_form()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            if (panel7.Visible == true)
            {
                panel7.Hide();
                this.label3.Image = global::ExamApp.Properties.Resources.icon1;
            }
            else
            {
                panel7.Show();
                this.label3.Image = global::ExamApp.Properties.Resources.icon2;
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {
            if (panel9.Visible == true)
            {
                panel9.Hide();
                this.label10.Image = global::ExamApp.Properties.Resources.icon1;
            }
            else
            {
                panel9.Show();
                this.label10.Image = global::ExamApp.Properties.Resources.icon2;
            }
        }
    }
}
