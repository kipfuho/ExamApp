using System;
using System.Collections.Generic;

class Category
{
	private Category parent;
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
	public int CategoryId
	{
		get { return categoryId; }
		set { categoryId = value; }
	}
}

class Choice
{
	string choiceText;
	double choideGrade;
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
