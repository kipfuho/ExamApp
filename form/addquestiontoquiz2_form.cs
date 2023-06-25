using System.Windows.Forms;

namespace ExamApp.form
{
    public partial class AddQuestionToQuiz2_Form : Form
    {
        public AddQuestionToQuiz2_Form()
        {
            InitializeComponent();
        }

        public Button AddToQuiz
        {
            get { return button1; }
        }

        public ComboBox CategoryComboBox
        {
            get { return comboBox1; }
        }

        public CheckBox SubcategoryCheckBox
        {
            get { return checkBox1; }
        }

        public CheckBox AddAll
        {
            get { return checkBox3; }
        }

        public Panel QuestionDisplay
        {
            get { return panel6; }
        }

        public Panel AddedQuestionDisplay
        {
            get { return panel7; }
        }
    }
}
