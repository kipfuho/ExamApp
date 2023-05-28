using System.Windows.Forms;

class QuestionBlock : Panel
{
    private Question question;
    private CheckBox checkBox;
    private Label label;
    private Label edit;

    public Question Question
    {
        get { return question; }
        set { question = value; }
    }

    public Label Edit
    {
        get { return edit; }
    }

    public QuestionBlock(Question question)
    {
        if (question == null)
        {
            MessageBox.Show("There's no questions!");
            return;
        }

        this.Question = question;
        Panel spacing = new Panel();
        this.checkBox = new CheckBox();
        this.label = new Label();
        this.edit = new Label();
        this.Size = new System.Drawing.Size(705, 25);
        this.BorderStyle = BorderStyle.FixedSingle;
        this.Controls.Add(this.edit);
        this.Controls.Add(this.label);
        this.Controls.Add(this.checkBox);
        this.Controls.Add(spacing);
        spacing.Dock = DockStyle.Left;
        spacing.Size = new System.Drawing.Size(10, 0);
        this.checkBox.Dock = DockStyle.Left;
        this.checkBox.Size = new System.Drawing.Size(30, 0);
        this.label.Dock = DockStyle.Left;
        this.label.Size = new System.Drawing.Size(600, 0);

        string qDescription;
        try
        {
            qDescription = RtfConverter.ConvertToPlainText(question.Description);
        }
        catch
        {
            qDescription = question.Description;
        }

        if (qDescription.Length > 60)
        {
            this.label.Text = qDescription.Substring(0, 60) + "...";
        }
        else
        {
            this.label.Text = qDescription;
        }

        this.edit.Text = "Edit";
        this.edit.Dock = DockStyle.Right;
        this.edit.Size = new System.Drawing.Size(60, 0);
        this.edit.Cursor = System.Windows.Forms.Cursors.Hand;
        this.edit.ForeColor = System.Drawing.Color.DeepSkyBlue;
    }
}

class QuizBlock : Panel
{
    private Quiz quiz;
    private Label image;
    private Label quizname;
    private bool state;

    public Quiz Quiz
    {
        get { return quiz; }
        set { quiz = value; }
    }

    public bool State
    {
        get { return state; }
        set { state = value; }
    }

    public Label QName
    {
        get { return quizname; }
    }

    public void changeState()
    {
        this.image.Image = global::ExamApp.Properties.Resources.icon8;
        this.quizname.ForeColor = System.Drawing.Color.SteelBlue;
    }

    public QuizBlock(Quiz quiz)
    {
        if (quiz == null)
        {
            MessageBox.Show("There's no questions!");
            return;
        }

        this.Quiz = quiz;
        this.image = new Label();
        this.image.Image = global::ExamApp.Properties.Resources.icon6;
        this.quizname = new Label();
        this.Size = new System.Drawing.Size(705, 25);
        this.Controls.Add(this.quizname);
        this.Controls.Add(this.image);
        this.image.Dock = DockStyle.Left;
        this.image.Size = new System.Drawing.Size(60, 0);
        this.quizname.Dock = DockStyle.Left;
        this.quizname.Size = new System.Drawing.Size(600, 0);
        this.quizname.Cursor = System.Windows.Forms.Cursors.Hand;
        this.quizname.ForeColor = System.Drawing.Color.DeepSkyBlue;

        if (quiz.Name.Length > 60)
        {
            this.quizname.Text = Quiz.Name.Substring(0, 60) + "...";
        }
        else
        {
            this.quizname.Text = Quiz.Name;
        }
    }
}

