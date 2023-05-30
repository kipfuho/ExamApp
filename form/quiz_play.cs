using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExamApp.form
{
    public partial class quiz_play : Form
    {
        public quiz_play()
        {
            InitializeComponent();
        }

        public Label Timer
        {
            get { return timerLabel; }
        }

        public Label FinishAttempt
        {
            get { return label2; }
        }

        public Label QuestionIndexLabel
        {
            get { return label7; }
        }

        public Label AnswerStateLabel
        {
            get { return label5; }
        }

        public Label MarkLabel
        {
            get { return label8; }
        }

        public Label FlagLabel
        {
            get { return label4; }
        }

        public Panel QuestionPanel
        {
            get { return panel8; }
        }
    }
}
