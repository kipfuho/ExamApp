using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using static System.Net.Mime.MediaTypeNames;

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
        Size = new Size(755, 25);
        BorderStyle = BorderStyle.FixedSingle;
        Controls.Add(Edit);
        Controls.Add(delete);
        Controls.Add(QName);
        Controls.Add(checkBox);
        Controls.Add(spacing);
        spacing.Dock = DockStyle.Left;
        spacing.Size = new Size(10, 0);
        checkBox.Dock = DockStyle.Left;
        checkBox.Size = new Size(30, 0);
        QName.Dock = DockStyle.Left;
        QName.Size = new Size(600, 0);

        string qDescription = question.Description;

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
        delete.Size = new Size(80, 0);
        delete.Cursor = Cursors.Hand;
        delete.ForeColor = Color.DeepSkyBlue;
        Edit.Text = "Edit";
        Edit.Dock = DockStyle.Right;
        Edit.Size = new Size(60, 0);
        Edit.Cursor = Cursors.Hand;
        Edit.ForeColor = Color.DeepSkyBlue;
    }
}

// question block in add a random question in edit quiz - BP5_1
class QuizQuestionBlockBP5_1 : PartialQBlock
{
    public QuizQuestionBlockBP5_1(Question question)
    {
        if (question == null)
        {
            MessageBox.Show("There's no questions!");
            return;
        }
        this.BorderStyle = BorderStyle.FixedSingle;
        Question = question;
        Panel spacing = new Panel();
        QName = new Label();
        Size = new Size(705, 25);
        Dock = DockStyle.Top;
        Controls.Add(QName);
        Controls.Add(spacing);
        spacing.Dock = DockStyle.Left;
        spacing.Size = new Size(10, 0);
        QName.Dock = DockStyle.Left;
        QName.Size = new Size(600, 0);

        string qDescription = question.Description;

        if (qDescription.Length > 60)
        {
            QName.Text = qDescription.Substring(0, 60) + "...";
        }
        else
        {
            QName.Text = qDescription;
        }
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
        Size = new Size(705, 25);
        Dock = DockStyle.Top;
        Controls.Add(QName);
        Controls.Add(spacing);
        Controls.Add(Edit);
        spacing.Dock = DockStyle.Left;
        spacing.Size = new Size(10, 0);
        QName.Dock = DockStyle.Left;
        QName.Size = new Size(600, 0);

        string qDescription = question.Description;

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
        Edit.Size = new Size(36, 0);
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
        Size = new Size(0, 30);
        BorderStyle = BorderStyle.FixedSingle;
        Dock = DockStyle.Top;
        Controls.Add(QName);
        Controls.Add(spacing);
        Controls.Add(index);
        Controls.Add(Edit);
        spacing.Dock = DockStyle.Left;
        spacing.Size = new Size(10, 0);
        QName.Dock = DockStyle.Left;
        QName.Size = new Size(600, 0);
        index.Dock = DockStyle.Left;
        index.Size = new Size(50, 0);
        index.Text = i.ToString();
        index.TextAlign = ContentAlignment.MiddleCenter;
        index.BackColor = Color.Gray;

        string qDescription = question.Description;

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
        Edit.Size = new Size(36, 0);
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
        Size = new Size(705, 25);
        Controls.Add(quizname);
        Controls.Add(image);
        image.Dock = DockStyle.Left;
        image.Size = new Size(60, 0);
        quizname.Dock = DockStyle.Left;
        quizname.Size = new Size(600, 0);
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

class PreviewBlock : Panel
{
    private PreviewQuiz preQuiz;
    private readonly Label attemptLabel;
    private readonly Label stateLabel;

    public PreviewQuiz PreQuiz
    {
        get { return preQuiz; }
        set { preQuiz = value; }
    }

    public Label StateLabel
    {
        get { return stateLabel; }
    }

    public PreviewBlock(PreviewQuiz preview)
    {
        if (preview == null)
        {
            MessageBox.Show("There's no Preview Quiz!");
            return;
        }

        PreQuiz = preview;
        attemptLabel = new Label
        {
            Text = "Preview",
            Dock = DockStyle.Left,
            Width = 92,
            Cursor = Cursors.Hand,
        };
        attemptLabel.MouseEnter += (sender, e) =>
        {
            // Change the background color on mouse enter
            BackColor = SystemColors.Control;
        };
        attemptLabel.MouseLeave += (sender, e) =>
        {
            // Restore the original background color on mouse leave
            BackColor = SystemColors.ControlDark;
        };
        stateLabel = new Label
        {
            Text = "Ongoing",
            Dock = DockStyle.Left,
            Width = 250,
            Cursor = Cursors.Hand,
        };
        stateLabel.MouseEnter += (sender, e) =>
        {
            // Change the background color on mouse enter
            BackColor = SystemColors.Control;
        };
        stateLabel.MouseLeave += (sender, e) =>
        {
            // Restore the original background color on mouse leave
            BackColor = SystemColors.ControlDark;
        };
        Panel spacing1 = new Panel { Dock = DockStyle.Left, Width = 150, Cursor = Cursors.Hand };
        spacing1.MouseEnter += (sender, e) =>
        {
            // Change the background color on mouse enter
            BackColor = SystemColors.Control;
        };
        spacing1.MouseLeave += (sender, e) =>
        {
            // Restore the original background color on mouse leave
            BackColor = SystemColors.ControlDark;
        };
        Panel spacing2 = new Panel { Dock = DockStyle.Left, Width = 150, Cursor = Cursors.Hand };
        spacing2.MouseEnter += (sender, e) =>
        {
            // Change the background color on mouse enter
            BackColor = SystemColors.Control;
        };
        spacing2.MouseLeave += (sender, e) =>
        {
            // Restore the original background color on mouse leave
            BackColor = SystemColors.ControlDark;
        };
        Panel spacing = new Panel { Dock = DockStyle.Fill, Width = 150, Cursor = Cursors.Hand };
        spacing.MouseEnter += (sender, e) =>
        {
            // Change the background color on mouse enter
            BackColor = SystemColors.Control;
        };
        spacing.MouseLeave += (sender, e) =>
        {
            // Restore the original background color on mouse leave
            BackColor = SystemColors.ControlDark;
        };
        Controls.Add(spacing);
        Controls.Add(stateLabel);
        Controls.Add(spacing1);
        Controls.Add(attemptLabel);
        Controls.Add(spacing2);
        Size = new Size(0, 35);
        Dock = DockStyle.Top;
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

class QuestionDisplayPanel : Panel
{
    public List<CheckBoxPlus> choiceCheckBox;
    private readonly List<Trestan.TRichTextBox> choiceText;
    private readonly Trestan.TRichTextBox qText;
    private readonly List<string> choiceLetter = new List<string> { "a.", "b.", "c.", "d.", "e." };

    public QuestionDisplayPanel(Question question)
    {
        int i = 0;
        choiceCheckBox = new List<CheckBoxPlus> { null, null, null, null, null };
        choiceText = new List<Trestan.TRichTextBox> { null, null, null, null, null };
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
                Trestan.TRichTextBox temp2 = new Trestan.TRichTextBox
                {
                    Dock = DockStyle.Fill,
                    BorderStyle = BorderStyle.None,
                    Cursor = Cursors.Default,
                    BackColor = SystemColors.Control,
                    ReadOnly = false,
                    WordWrap = false
                };
                temp2.GotFocus += (sender, e) =>
                {
                    var richTextBox = (Trestan.TRichTextBox)sender;
                    this.Focus();
                };
                temp2.ContentsResized += (object sender, ContentsResizedEventArgs e) =>
                {
                    var richTextBox = (RichTextBox)sender;
                    richTextBox.Width = e.NewRectangle.Width;
                    richTextBox.Height = e.NewRectangle.Height;
                };
                temp2.AppendText(choice.Text + Environment.NewLine);
                temp2.ContentsResized -= (object sender, ContentsResizedEventArgs e) =>
                {
                    var richTextBox = (RichTextBox)sender;
                    richTextBox.Width = e.NewRectangle.Width;
                    richTextBox.Height = e.NewRectangle.Height;
                };
                foreach (Control control in choice.ChoiceImages)
                {
                    temp2.AddControl(control);
                    temp2.Height += control.Height;
                }
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
            spacing.Size = new Size(25, 0);
            temp.Dock = DockStyle.Top;
            temp.Controls.Add(choiceText[j]);
            temp.Controls.Add(spacing);
            temp.Size = new Size(0, choiceText[j].Height);
            Controls.Add(temp);
        }
        qText = new Trestan.TRichTextBox
        {
            Dock = DockStyle.Top,
            BorderStyle = BorderStyle.None,
            Cursor = Cursors.Default,
            BackColor = SystemColors.Control,
            ScrollBars = RichTextBoxScrollBars.Both,
            ReadOnly = false
        };
        qText.GotFocus += (sender, e) =>
        {
            var richTextBox = (Trestan.TRichTextBox)sender;
            this.Focus();
        };
        qText.ContentsResized += (object sender, ContentsResizedEventArgs e) =>
        {
            var richTextBox = (RichTextBox)sender;
            richTextBox.Width = e.NewRectangle.Width;
            richTextBox.Height = e.NewRectangle.Height;
        };
        qText.AppendText(question.Description + Environment.NewLine);
        qText.ContentsResized -= (object sender, ContentsResizedEventArgs e) =>
        {
            var richTextBox = (RichTextBox)sender;
            richTextBox.Width = e.NewRectangle.Width;
            richTextBox.Height = e.NewRectangle.Height;
        };
        foreach (Control control in question.QuestionImages)
        {
            qText.AddControl(control);
            qText.Height += control.Height;
        }
        qText.ReadOnly = true;
        Controls.Add(qText);
        Dock = DockStyle.Top;
        AutoSize = true;
    }
}
