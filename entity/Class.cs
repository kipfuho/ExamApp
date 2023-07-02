using DocumentFormat.OpenXml.Drawing;
using ExamApp;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

class Category
{
    private Category parent;
    private List<Category> child;
    private string categoryName;
    private string categoryInfo;
    private int categoryId;
    private int gen;
    private List<Question> questionList;

    public Category(Category parent, List<Category> child, string categoryName, string categoryInfo, int categoryId, int gen, List<Question> questionList)
    {
        this.Parent = parent;
        this.child = child;
        this.Name = categoryName;
        this.Info = categoryInfo;
        this.Id = categoryId;
        this.Gen = gen;
        this.QuestionList = questionList;
    }

    public string Name
    {
        get { return categoryName; }
        set { categoryName = value; }
    }

    public string Info
    {
        get { return categoryInfo; }
        set { categoryInfo = value; }
    }

    public int Gen
    {
        get { return gen; }
        set { gen = value; }
    }

    public int Id
    {
        get { return categoryId; }
        set { categoryId = value; }
    }

    public List<Question> QuestionList
    {
        get { return questionList; }
        set { questionList = value; }
    }

    public Category Parent
    {
        get { return parent; }
        set { parent = value; }
    }

    public List<Category> Child
    {
        get { return child; }
        set { child = value; }
    }

    public string NameAndGen
    {
        get
        {
            if (QuestionList.Count > 0)
            {
                return $"{new string(' ', Gen)}{Name}({QuestionList.Count})";
            }
            else
            {
                return $"{new string(' ', Gen)}{Name}";
            }
        }
    }

    public void AddQuestion(Question question)
    {
        this.QuestionList.Add(question);
    }

    public void AddChild(Category subcategory)
    {
        this.Child.Add(subcategory);
    }
}

class Choice
{
    private string choiceText;
    private double choideGrade;
    private List<Control> images;

    public string Text
    {
        get { return choiceText; }
        set { choiceText = value; }
    }

    public double Grade
    {
        get { return choideGrade; }
        set { choideGrade = value; }
    }

    public List<Control> ChoiceImages
    {
        get { return images; }
        set { images = value; }
    }
}

class Question
{
    private Category category;
    private string questionName;
    private string questionText;
    private double defaultMark;
    private List<Choice> choices;
    private List<Control> images;
    public QuestionBlock Tag;

    public Category Category
    {
        get { return category; }
        set { category = value; }
    }

    public string Name
    {
        get { return questionName; }
        set { questionName = value; }
    }

    public string Description
    {
        get { return questionText; }
        set { questionText = value; }
    }

    public double Mark
    {
        get { return defaultMark; }
        set { defaultMark = value; }
    }

    public List<Choice> Choices
    {
        get { return choices; }
        set { choices = value; }
    }

    public List<Control> QuestionImages
    {
        get { return images; }
        set { images = value; }
    }
}

class QAnswer
{
    private Question question;
    private Choice answer;
    private bool flag;

    public Question Question
    {
        get { return question; }
        set { question = value; }
    }

    public Choice Answer
    {
        get { return answer; }
        set { answer = value; }
    }

    public bool Flag
    {
        get { return flag; }
        set { flag = value; }
    }
}

class Timing
{
    private DateTime open;
    private DateTime close;
    private int timecoeff;
    private string unit;

    public DateTime TimeOpen
    {
        get { return open; }
        set { open = value; }
    }

    public DateTime TimeClose
    {
        get { return close; }
        set { close = value; }
    }

    public int TimeCoefficient
    {
        get { return timecoeff; }
        set { timecoeff = value; }
    }
    public string TimeUnit
    {
        get { return unit; }
        set { unit = value; }
    }

    // return time limit with unit of seconds
    public int TimeLimit
    {
        get
        {
            if (this.TimeUnit == "minutes")
            {
                return 60 * timecoeff;
            }
            else if (this.TimeUnit == "hours")
            {
                return 3600 * timecoeff;
            }
            else
            {
                return timecoeff;
            }
        }
    }
}

class TimerPlus : Timer
{
    private DateTime startTime;
    private DateTime endTime;
    private TimeSpan timeLimit;
    public Label timerLabel;
    public bool active;

    public TimerPlus(int seconds, Label label)
    {
        if (seconds == 0)
        {
            active = false;
            return;
        }
        active = true;
        timerLabel = label;
        timeLimit = TimeSpan.FromSeconds(seconds);

        // Calculate the end time based on the time limit
        startTime = DateTime.Now;
        endTime = startTime.Add(timeLimit);

        Interval = 1000; // Update the label every 1 second
        Tick += new EventHandler(ForeGroundTimer_Tick);
    }

    public void ChangeMode(bool mode)
    {
        if (mode)
        {
            Tick += new EventHandler(ForeGroundTimer_Tick);
            Tick -= new EventHandler(BackGroundTimer_Tick);
        }
        else
        {
            Tick -= new EventHandler(ForeGroundTimer_Tick);
            Tick += new EventHandler(BackGroundTimer_Tick);
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
            Stop();
            timerLabel.Text = "Time's up!";
            Program.Instance.BP6_Timer_TimeUp(Tag, true);
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
            Stop();
            timerLabel.Text = "Time's up!";
            Program.Instance.BP6_Timer_TimeUp(Tag, false);
        }
    }
}

class PreviewQuiz
{
    private Quiz tag;
    private List<QAnswer> answers;
    public List<QuestionDisplayPanel> qLayouts;
    private DateTime startTime;
    private DateTime endTime;
    private double mark;
    private TimerPlus timer;
    private PreviewBlock containBlock;

    public Quiz Tag
    {
        get { return tag; } 
        set { tag = value; }
    }

    public List<QAnswer> QAnswerList
    {
        get { return answers; }
        set { answers = value; }
    }

    public DateTime StartTime
    {
        get { return startTime; }
        set { startTime = value; }
    }

    public DateTime EndTime
    {
        get { return endTime; }
        set { endTime = value; }
    }

    public double Mark
    {
        get { return mark; }
        set { mark = value; }
    }

    // get time taken as in seconds
    public TimeSpan GetRemainingTime()
    {
        TimeSpan timeTaken = endTime - startTime;
        return timeTaken;
    }

    public TimerPlus Timer
    {
        get { return timer; }
        set { timer = value; }
    }

    public PreviewBlock Container
    {
        get { return containBlock; }
        set {  containBlock = value; }
    }
}

class Quiz
{
    private string quizName;
    private string quizDescription;
    private List<Question> questions;
    private double maxGrade;
    private double totalMark;
    private bool shuffle;
    private List<Question> pendingQuestions;
    private List<PreviewQuiz> previews;
    private PreviewQuiz ongoingPreview;
    Timing time;

    public string Name
    {
        get { return quizName; }
        set { quizName = value; }
    }

    public string Description
    {
        get { return quizDescription; }
        set { quizDescription = value; }
    }

    public double MaxGrade
    {
        get { return maxGrade; }
        set { maxGrade = value; }
    }

    public double TotalMark
    {
        get { return totalMark; }
        set { totalMark = value; }
    }

    public bool Shuffle
    {
        get { return shuffle; }
        set { shuffle = value; }
    }

    public List<Question> QuestionList
    {
        get { return questions; }
        set { questions = value; }
    }

    public List<Question> PendingList
    {
        get { return pendingQuestions; }
        set { pendingQuestions = value; }
    }

    public List<PreviewQuiz> Previews
    {
        get { return previews; }
        set { previews = value; }
    }

    public PreviewQuiz OngoingPreview
    {
        get { return ongoingPreview; }
        set { ongoingPreview = value; }
    }

    public Timing Time
    {
        get { return time; }
        set { time = value; }
    }
}

