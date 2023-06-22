using System;
using System.Windows.Forms;

namespace ExamApp.form
{
    public partial class quiz_play : Form
    {
        static Timer timer;
        static DateTime startTime;
        static DateTime endTime;
        static TimeSpan timeLimit;

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

        public Label Back
        {
            get { return label9; }
        }

        public Label Next
        {
            get { return label10; }
        }

        public Panel QuestionPanel
        {
            get { return panel8; }
        }

        public FlowLayoutPanel QuestionIndexFlowLayout
        {
            get { return flowLayoutPanel1; }
        }

        public void StartTimer(int seconds)
        {
            timeLimit = TimeSpan.FromSeconds(seconds);

            // Calculate the end time based on the time limit
            startTime = DateTime.Now;
            endTime = startTime.Add(timeLimit);

            // Create and configure the Timer
            timer = new Timer();
            timer.Interval = 1000; // Update the label every 100 milliseconds
            timer.Tick += Timer_Tick;

            // Start the timer
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Calculate the remaining time
            TimeSpan remainingTime = endTime - DateTime.Now;

            // Check if the time limit has been reached
            if (remainingTime <= TimeSpan.Zero)
            {
                // Stop the timer
                timer.Stop();
                timerLabel.Text = "Time's up!";
            }
            else
            {
                // Update the label with the remaining time
                timerLabel.Text = "Time Left: " + remainingTime.ToString(@"mm\:ss");
            }
        }
    }
}
