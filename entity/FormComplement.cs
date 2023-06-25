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
        set { edit = value; }
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
    private readonly CheckBox checkBox;
    private readonly Label delete;

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

        Question = question;
        Panel spacing = new Panel();
        checkBox = new CheckBox();
        QName = new Label();
        Edit = new Label();
        delete = new Label();
        Size = new System.Drawing.Size(755, 25);
        BorderStyle = BorderStyle.FixedSingle;
        Controls.Add(Edit);
        Controls.Add(delete);
        Controls.Add(QName);
        Controls.Add(checkBox);
        Controls.Add(spacing);
        spacing.Dock = DockStyle.Left;
        spacing.Size = new System.Drawing.Size(10, 0);
        checkBox.Dock = DockStyle.Left;
        checkBox.Size = new System.Drawing.Size(30, 0);
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

        delete.Text = "Delete";
        delete.Dock = DockStyle.Right;
        delete.Size = new System.Drawing.Size(80, 0);
        delete.Cursor = System.Windows.Forms.Cursors.Hand;
        delete.ForeColor = System.Drawing.Color.DeepSkyBlue;
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
        Question = question;
        Panel spacing = new Panel();
        QName = new Label();
        Edit = new Label();
        Size = new System.Drawing.Size(705, 25);
        Dock = DockStyle.Top;
        Controls.Add(QName);
        Controls.Add(spacing);
        Controls.Add(Edit);
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
            Edit.Image = ExamApp.Properties.Resources.icon11;
        }
        else
        {
            Edit.Image = ExamApp.Properties.Resources.icon10;
        }
        Edit.Dock = DockStyle.Left;
        Edit.Size = new System.Drawing.Size(36, 0);
        Edit.Cursor = Cursors.Hand;
        Edit.ForeColor = Color.DeepSkyBlue;
    }
}

class QuizQuestionBlockBP5 : PartialQBlock
{
    private readonly Label index;

    public int Index
    {
        get { return Convert.ToInt32(index.Text); }
    }

    public void SetIndex(int i)
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
        Question = question;
        Panel spacing = new Panel();
        QName = new Label();
        Edit = new Label();
        index = new Label();
        Size = new System.Drawing.Size(0, 30);
        BorderStyle = BorderStyle.FixedSingle;
        Dock = DockStyle.Top;
        Controls.Add(QName);
        Controls.Add(spacing);
        Controls.Add(index);
        Controls.Add(Edit);
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
        Edit.Image = ExamApp.Properties.Resources.icon12;
        Edit.Dock = DockStyle.Right;
        Edit.Size = new System.Drawing.Size(36, 0);
        Edit.Cursor = Cursors.Hand;
    }
}

class QuizBlock : Panel
{
    private Quiz quiz;
    private readonly Label image;
    private readonly Label quizname;
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

    public void ChangeState()
    {
        image.Image = ExamApp.Properties.Resources.icon8;
        quizname.ForeColor = Color.SteelBlue;
    }

    public QuizBlock(Quiz quiz)
    {
        if (quiz == null)
        {
            MessageBox.Show("There's no questions!");
            return;
        }

        Quiz = quiz;
        image = new Label
        {
            Image = ExamApp.Properties.Resources.icon6
        };
        quizname = new Label();
        Size = new System.Drawing.Size(705, 25);
        Controls.Add(quizname);
        Controls.Add(image);
        image.Dock = DockStyle.Left;
        image.Size = new System.Drawing.Size(60, 0);
        quizname.Dock = DockStyle.Left;
        quizname.Size = new System.Drawing.Size(600, 0);
        quizname.Cursor = Cursors.Hand;
        quizname.ForeColor = Color.DeepSkyBlue;

        if (quiz.Name.Length > 60)
        {
            quizname.Text = Quiz.Name.Substring(0, 60) + "...";
        }
        else
        {
            quizname.Text = Quiz.Name;
        }
    }
}

class ButtonPlus : Button
{
    private QAnswer qAnswer;
    public QAnswer QAnswerAttached
    {
        get { return qAnswer; }
        set { qAnswer = value; }
    }
}

class CheckBoxPlus : CheckBox
{
    private Choice choice;
    public Choice ChoiceAttached
    {
        get { return choice; }
        set { choice = value; }
    }
}

class QuestionDisplay : Panel
{
    public List<CheckBoxPlus> choiceCheckBox;
    private readonly List<RichTextBox> choiceText;
    private readonly RichTextBox qText;
    private readonly List<string> choiceLetter = new List<string> { "a.", "b.", "c.", "d.", "e." };

    public QuestionDisplay(Question question)
    {
        int i = 0;
        choiceCheckBox = new List<CheckBoxPlus> { null, null, null, null, null };
        choiceText = new List<RichTextBox> { null, null, null, null, null };
        for (int j = 0; j < 5; j++)
        {
            Choice choice = question.Choices[j];
            if (choice != null)
            {
                CheckBoxPlus temp1 = new CheckBoxPlus
                {
                    ChoiceAttached = choice,
                    Text = choiceLetter[i],
                    AutoSize = true,
                    Dock = DockStyle.Top
                };
                RichTextBox temp2 = new RichTextBox();
                temp2.ContentsResized += (object sender, ContentsResizedEventArgs e) =>
                {
                    var richTextBox = (RichTextBox)sender;
                    richTextBox.Width = e.NewRectangle.Width;
                    richTextBox.Height = e.NewRectangle.Height;
                    temp2.Width += temp2.Margin.Horizontal + SystemInformation.HorizontalResizeBorderThickness;
                };
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
                choiceCheckBox[i] = temp1;
                choiceText[i] = temp2;
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
            Controls.Add(temp);
        }
        qText = new RichTextBox();
        qText.ContentsResized += (object sender, ContentsResizedEventArgs e) =>
        {
            var richTextBox = (RichTextBox)sender;
            richTextBox.Width = e.NewRectangle.Width;
            richTextBox.Height = e.NewRectangle.Height;
            qText.Width += qText.Margin.Horizontal + SystemInformation.HorizontalResizeBorderThickness;
        };
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
        Controls.Add(qText);
        Dock = DockStyle.Top;
        AutoSize = true;
    }
}