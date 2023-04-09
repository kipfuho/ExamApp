﻿using System;
using System.Collections.Generic;
using System.Linq;
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
		if(question == null)
		{
			MessageBox.Show("There's no questions!");
			return ;
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

		if (question.Name.Length > 60)
		{
            this.label.Text = Question.Name.Substring(0, 60) + "...";

        }
		else
		{
			this.label.Text = Question.Name;
		}

        this.edit.Text = "Edit";
		this.edit.Dock = DockStyle.Right;
		this.edit.Size = new System.Drawing.Size(60, 0);
        this.edit.Cursor = System.Windows.Forms.Cursors.Hand;
        this.edit.ForeColor = System.Drawing.Color.DeepSkyBlue;
    }
}