using System;
using System.Collections.Generic;
using System.Windows.Forms;

class Category
{
	private List<Category> child;
	private string categoryName;
	private string categoryInfo;
	private int categoryId;
	private List<Question> questionList;
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
	public int Id
	{
		get { return categoryId; }
		set { categoryId = value; }
	}
	public List<Question> QuestionList
	{	get { return questionList; } 
		set { questionList = value; } 
	}
	public List<Category> Child
	{
		get { return child; }
		set { child = value; }
	}
	public void addQuestion(Question question)
	{
		questionList.Add(question);
	}
}

class Choice
{
	private string choiceText;
	private double choideGrade;
	public string Text
	{
		get { return choiceText; }
		set { choiceText = value; }
	}
	public double Grade
	{
		get { return choideGrade;}
		set { choideGrade = value; }
	}
}

class Question
{
	private Category category;
	private string questionName;
	private string questionText;
	private double defaultMark;
	private List<Choice> choices;
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
	public void addChoice(Choice choice)
	{
		choices.Add(choice);
	}
}

class Quiz
{
	private string quizName;
	private string quizDescription;
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
}

class QuestionBlock : Panel
{
	private Question question;
	Label label;
	Label edit;
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
        Question = question;
		this.label = new Label();
		this.edit = new Label();
        this.Controls.Add(label);
		this.Controls.Add(edit);
        this.Size = new System.Drawing.Size(900, 30);
		this.TabIndex = 0;
        this.label.Location = new System.Drawing.Point(4, 3);
        this.label.Text = question.Name;
        this.label.TabIndex = 0;
        this.edit.Location = new System.Drawing.Point(850, 3);
        this.edit.Text = "Edit";
        this.edit.Cursor = System.Windows.Forms.Cursors.Hand;
        this.edit.ForeColor = System.Drawing.Color.DeepSkyBlue;
        this.edit.TabIndex = 1;
    }
}