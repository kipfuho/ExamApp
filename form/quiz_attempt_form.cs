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
    public partial class quiz_attempt_form : Form
    {
        public quiz_attempt_form()
        {
            InitializeComponent();
        }

        public Label QuizName
        {
            get { return label1; }
        }

        public Label TimeCoeff
        {
            get { return timelimitlabel; }
        }

        public Label GearSetting
        {
            get { return gearLabel; }
        }

        public Button PrviewQzButton
        {
            get { return previewquizbutton; }
        }

        public FlowLayoutPanel QzPR
        {
            get { return flowLayoutPanel1; }
        }
    }
}
