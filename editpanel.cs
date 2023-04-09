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
    public partial class editpanel : Form
    {
        public editpanel()
        {
            InitializeComponent();
        }

        public Button DirectingButton1
        {
            get { return this.CreateQuestionButton; }
        }

        public FlowLayoutPanel QBox
        {
            get { return QuestionFlowLayout; }
        }

        public CheckBox SubcategoriesQ
        {
            get { return this.ShowQuestionsSubcategoriesCheckBox; }
        }
    }
}
