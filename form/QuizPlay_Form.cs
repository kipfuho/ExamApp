using System;
using System.Windows.Forms;

namespace ExamApp
{
    public partial class QuizPlay_Form : Form
    {
        public QuizPlay_Form()
        {
            InitializeComponent();
        }

        public Label TimerLabel
        {
            get { return timerLabel; }
            set { timerLabel = value; }
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
    }
}
