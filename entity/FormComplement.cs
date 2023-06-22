using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;

// parent class for inheritance
class PartialQBlock : Panel
{
    private Question question;
    private Label edit;
    private Label qName;

    public Question Question
    {
        get { return question; }
        set { question = value; }
    }

    public Label Edit
    {
        get { return edit; }
        set {  edit = value; }
    }

    public Label QName
    {
        get { return qName; }
        set { qName = value; }
    }
}

// question block in question tab in edit question BP1
class QuestionBlock : PartialQBlock
{
    private CheckBox checkBox;
    private Label delete;

    public Label Delete
    {
        get { return delete; }
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
        QName = new Label();
        Edit = new Label();
        this.delete = new Label();
        this.Size = new System.Drawing.Size(755, 25);
        this.BorderStyle = BorderStyle.FixedSingle;
        this.Controls.Add(Edit);
        this.Controls.Add(this.delete);
        this.Controls.Add(QName);
        this.Controls.Add(this.checkBox);
        this.Controls.Add(spacing);
        spacing.Dock = DockStyle.Left;
        spacing.Size = new System.Drawing.Size(10, 0);
        this.checkBox.Dock = DockStyle.Left;
        this.checkBox.Size = new System.Drawing.Size(30, 0);
        QName.Dock = DockStyle.Left;
        QName.Size = new System.Drawing.Size(600, 0);

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
            QName.Text = qDescription.Substring(0, 60) + "...";
        }
        else
        {
            QName.Text = qDescription;
        }

        this.delete.Text = "Delete";
        this.delete.Dock = DockStyle.Right;
        this.delete.Size = new System.Drawing.Size(80, 0);
        this.delete.Cursor = System.Windows.Forms.Cursors.Hand;
        this.delete.ForeColor = System.Drawing.Color.DeepSkyBlue;
        Edit.Text = "Edit";
        Edit.Dock = DockStyle.Right;
        Edit.Size = new System.Drawing.Size(60, 0);
        Edit.Cursor = System.Windows.Forms.Cursors.Hand;
        Edit.ForeColor = System.Drawing.Color.DeepSkyBlue;
    }
}

// question block in add question from question bank in edit quiz - BP5_2
class QuizQuestionBlockBP5_2 : PartialQBlock
{
    public QuizQuestionBlockBP5_2(Question question, bool pending)
    {
        if (question == null)
        {
            MessageBox.Show("There's no questions!");
            return;
        }
        this.Question = question;
        Panel spacing = new Panel();
        QName = new Label();
        Edit = new Label();
        this.Size = new System.Drawing.Size(705, 25);
        this.Dock = DockStyle.Top;
        this.Controls.Add(QName);
        this.Controls.Add(spacing);
        this.Controls.Add(Edit);
        spacing.Dock = DockStyle.Left;
        spacing.Size = new System.Drawing.Size(10, 0);
        QName.Dock = DockStyle.Left;
        QName.Size = new System.Drawing.Size(600, 0);

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
            QName.Text = qDescription.Substring(0, 60) + "...";
        }
        else
        {
            QName.Text = qDescription;
        }

        Edit.Text = " ";
        if (pending)
        {
            Edit.Image = global::ExamApp.Properties.Resources.icon11;
        }
        else
        {
            Edit.Image = global::ExamApp.Properties.Resources.icon10;
        }
        Edit.Dock = DockStyle.Left;
        Edit.Size = new System.Drawing.Size(36, 0);
        Edit.Cursor = System.Windows.Forms.Cursors.Hand;
        Edit.ForeColor = System.Drawing.Color.DeepSkyBlue;
    }
}

class QuizQuestionBlockBP5 : PartialQBlock
{
    private Label index;

    public int Index
    {
        get { return Convert.ToInt32(index.Text); }
    }

    public void setIndex(int i)
    {
        index.Text = i.ToString();
    }

    public QuizQuestionBlockBP5(Question question, int i)
    {
        if (question == null)
        {
            MessageBox.Show("There's no questions!");
            return;
        }
        this.Question = question;
        Panel spacing = new Panel();
        QName = new Label();
        Edit = new Label();
        index = new Label();
        this.Size = new System.Drawing.Size(0, 30);
        this.BorderStyle = BorderStyle.FixedSingle;
        this.Dock = DockStyle.Top;
        this.Controls.Add(QName);
        this.Controls.Add(spacing);
        this.Controls.Add(index);
        this.Controls.Add(Edit);
        spacing.Dock = DockStyle.Left;
        spacing.Size = new System.Drawing.Size(10, 0);
        QName.Dock = DockStyle.Left;
        QName.Size = new System.Drawing.Size(600, 0);
        index.Dock = DockStyle.Left;
        index.Size = new System.Drawing.Size(50, 0);
        index.Text = i.ToString();
        index.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        index.BackColor = System.Drawing.Color.Gray;

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
            QName.Text = qDescription.Substring(0, 60) + "...";
        }
        else
        {
            QName.Text = qDescription;
        }

        Edit.Text = " ";
        Edit.Image = global::ExamApp.Properties.Resources.icon12;
        Edit.Dock = DockStyle.Right;
        Edit.Size = new System.Drawing.Size(36, 0);
        Edit.Cursor = System.Windows.Forms.Cursors.Hand;
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
        this.quizname.ForeColor = Color.SteelBlue;
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
        this.quizname.Cursor = Cursors.Hand;
        this.quizname.ForeColor = Color.DeepSkyBlue;

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

class QuizNavigationQuestionBtn : Button
{
    private int index;

    public int Index
    {
        get { return index; }
    }

    QuizNavigationQuestionBtn(int index)
    {
        this.index = index;
        this.Text = "index";
        this.AutoSize = true;
    }
}

class ButtonPlus : Button
{
    private Question question;
    public Question QuestionAttached
    {
        get { return question; }
        set { question = value; }
    }
}

class QuestionDisplay : Panel
{
    private List<CheckBox> choiceCheckBox;
    private List<RichTextBox> choiceText;
    private RichTextBox qText;
    private List<String> choiceLetter = new List<String> { "a.", "b.", "c.", "d.", "e."};

    public QuestionDisplay(Question question)
    {
        int i = 0;
        choiceCheckBox = new List<CheckBox>();
        choiceText = new List<RichTextBox>();
        foreach (Choice choice in question.Choices)
        {
            if(choice != null)
            {
                CheckBox temp1 = new CheckBox();
                temp1.Text = choiceLetter[i];
                temp1.AutoSize = true;
                temp1.Dock = DockStyle.Top;
                RichTextBox temp2 = new RichTextBox();
                temp2.ReadOnly = false;
                try
                {
                    temp2.Rtf = choice.Text;
                }
                catch
                {
                    temp2.Text = choice.Text;
                }
                temp2.Dock = DockStyle.Fill;
                temp2.ReadOnly = true;
                temp2.BorderStyle = BorderStyle.None;
                temp2.Cursor = Cursors.Default;
                temp2.BackColor = SystemColors.Control;
                temp2.ReadOnly = true;
                temp2.ContentsResized += (object sender, ContentsResizedEventArgs e) =>
                {
                    var richTextBox = (RichTextBox)sender;
                    richTextBox.Width = e.NewRectangle.Width;
                    richTextBox.Height = e.NewRectangle.Height;
                    temp2.Width += temp2.Margin.Horizontal + SystemInformation.HorizontalResizeBorderThickness;
                };
                choiceCheckBox.Add(temp1);
                choiceText.Add(temp2);
                i++;
            }
        }
        for (int j = i - 1; j >= 0; j--)
        {
            Panel temp = new Panel();
            Panel spacing = new Panel();
            spacing.Controls.Add(choiceCheckBox[j]);
            spacing.Dock = DockStyle.Left;
            spacing.Size = new System.Drawing.Size(50, 0);
            temp.Dock = DockStyle.Top;
            temp.Controls.Add(choiceText[j]);
            temp.Controls.Add(spacing);
            temp.Size = new System.Drawing.Size(0, choiceText[j].Height);
            this.Controls.Add(temp);
        }
        qText = new RichTextBox();
        qText.ReadOnly = false;
        try
        {
            qText.Rtf = question.Description;
        }
        catch
        {
            qText.Text = question.Description;
        }
        qText.Dock = DockStyle.Top;
        qText.BorderStyle = BorderStyle.None;
        qText.Cursor = Cursors.Default;
        qText.BackColor = SystemColors.Control;
        qText.ReadOnly = true;
        qText.ContentsResized += (object sender, ContentsResizedEventArgs e) =>
        {
            var richTextBox = (RichTextBox)sender;
            richTextBox.Width = e.NewRectangle.Width;
            richTextBox.Height = e.NewRectangle.Height;
            qText.Width += qText.Margin.Horizontal + SystemInformation.HorizontalResizeBorderThickness;
        };
        this.Controls.Add(qText);
        this.Dock = DockStyle.Top;
        this.AutoSize = true;
    }
}