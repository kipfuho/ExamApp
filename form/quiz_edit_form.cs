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
    public partial class quiz_edit_form : Form
    {
        public quiz_edit_form()
        {
            InitializeComponent();
        }

        public void Add_Click(object sender ,EventArgs e)
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

        public CheckBox shuffleBox
        {
            get { return shuffleCheckBox; }
        }

        public Label addNewQuestionLabel
        {
            get { return this.add1Label; }
        }

        public Label addFromQuestionBankLabel
        {
            get { return this.add2Label; }
        }

        public Label addRandomQuestionLabel
        {
            get { return this.add3Label; }
        }


    }
}
