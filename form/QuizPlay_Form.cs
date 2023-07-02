using System;
using System.Windows.Forms;

namespace ExamApp
{
    public partial class QuizPlay_Form : Form
    {
        private Timer timer;
        private DateTime startTime;
        private DateTime endTime;
        private TimeSpan timeLimit;

        public QuizPlay_Form()
        {
            InitializeComponent();
        }

        public Label TimerLabel
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

        public Label Back
        {
            get { return label9; }
        }

        public Label Next
        {
            get { return label10; }
        }

        public Label AttemptInformation_StartedOn
        {
            get { return label17; }
        }

        public Label AttemptInformation_State
        {
            get { return label18; }
        }
        public Label AttemptInformation_CompletedOn
        {
            get { return label19; }
        }
        public Label AttemptInformation_TimeTaken
        {
            get { return label20; }
        }
        public Label AttemptInformation_Marks
        {
            get { return label21; }
        }
        public Label AttemptInformation_Grade
        {
            get { return label22; }
        }

        public TextBox CorrectAnswerTextBox
        {
            get { return textBox1; }
        }

        public Panel QuestionPanel
        {
            get { return panel8; }
        }

        public Panel AttemptInformationPanel
        {
            get { return panel5; }
        }

        public FlowLayoutPanel QuestionIndexFlowLayout
        {
            get { return flowLayoutPanel1; }
        }

        public Timer Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        public void StartTimer(int seconds)
        {
            if (timer != null)
            {
                timer.Tick -= new EventHandler(ForeGroundTimer_Tick);
                timer.Tick += new EventHandler(BackGroundTimer_Tick);
                return;
            }
            timeLimit = TimeSpan.FromSeconds(seconds);

            // Calculate the end time based on the time limit
            startTime = DateTime.Now;
            endTime = startTime.Add(timeLimit);

            // Create and configure the Timer
            timer = new Timer
            {
                Interval = 1000 // Update the label every 1 second
            };
            timer.Tick += new EventHandler(ForeGroundTimer_Tick);

            // Start the timer
            timer.Start();
        }

        public void ContinueTimer(Timer oldTimer)
        {
            timer = oldTimer;
        }

        public void StopTimer()
        {
            if(timer != null)
            {
                timer.Stop();
                timer.Dispose();
                timer = null;
            }
        }

        private void ForeGroundTimer_Tick(object sender, EventArgs e)
        {
            // Calculate the remaining time
            TimeSpan remainingTime = endTime - DateTime.Now;

            // Check if the time limit has been reached
            if (remainingTime <= TimeSpan.Zero)
            {
                // Stop the timer
                timer.Stop();
                timerLabel.Text = "Time's up!";
                ExamApp temp = this.Tag as ExamApp;
                temp.BP6_Timer_TimeUp(timer.Tag, true);
            }
            else
            {
                // Update the label with the remaining time
                timerLabel.Text = "Time Left: " + remainingTime.ToString(@"mm\:ss");
            }
        }

        private void BackGroundTimer_Tick(object sender, EventArgs e)
        {
            // Calculate the remaining time
            TimeSpan remainingTime = endTime - DateTime.Now;

            // Check if the time limit has been reached
            if (remainingTime <= TimeSpan.Zero)
            {
                // Stop the timer
                timer.Stop();
                timerLabel.Text = "Time's up!";
                ExamApp temp = this.Tag as ExamApp;
                temp.BP6_Timer_TimeUp(timer.Tag, false);
            }
        }
    }
}
