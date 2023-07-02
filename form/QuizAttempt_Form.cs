using System.Windows.Forms;

namespace ExamApp
{
    public partial class QuizAttempt_Form : Form
    {
        public QuizAttempt_Form()
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

        public Button Export
        {
            get { return export; }
        }

        public Panel PopupPanel
        {
            get { return panel15; }
        }

        public Panel QzPR
        {
            get { return panel17; }
        }

        private void previewQuizBtn_Click(object sender, System.EventArgs e)
        {
            if (panel15.Visible == false)
            {
                int x = panel17.Left + panel17.Width / 2 - panel15.Width / 2;
                int y = panel17.Top;
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
