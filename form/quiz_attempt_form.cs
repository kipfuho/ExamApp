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

        public Label TimeLimitLabel
        {
            get { return timelimitlabel; }
        }

        public Label GearSetting
        {
            get { return gearLabel; }
        }

        public Button PrviewQzButton
        {
            get { return previewQuizBtn; }
        }

        public Button StartAttemptButton
        {
            get { return startAttemptBtn; }
        }

        public FlowLayoutPanel QzPR
        {
            get { return flowLayoutPanel1; }
        }

        private void previewQuizBtn_Click(object sender, System.EventArgs e)
        {
            if(panel15.Visible == false)
            {
                int x = flowLayoutPanel1.Left + flowLayoutPanel1.Width / 2 - panel15.Width / 2;
                int y = flowLayoutPanel1.Top;
                panel15.Location = new System.Drawing.Point(x, y);
                panel15.Show();
            }
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            panel15.Hide();
        }

        private void cancelAttemptBtn_Click(object sender, System.EventArgs e)
        {
            panel15.Hide();
        }
    }
}
