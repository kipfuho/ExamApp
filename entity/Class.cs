using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
	{	get { return questionList; } 
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

    public void addQuestion(Question question)
	{
		this.QuestionList.Add(question);
	}

	public void addChild(Category subcategory)
	{
		this.Child.Add(subcategory);
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
}

class QAnswer
{
	private Question question;
	private Choice answer;

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

class PreviewQuiz
{
	Quiz quiz;
	List<QAnswer> answers;
}

class Quiz
{
	private string quizName;
	private string quizDescription;
	private List<Question> questions;
	private List<PreviewQuiz> previews; 
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

	public List<Question> QuestionList
	{
		get { return questions; }
		set { questions = value; }
	}

	public List<PreviewQuiz> Previews
	{
		get { return previews; }
		set { previews = value; }
	}

	public Timing Time
	{
		get { return time; }
        set { time = value; }
	}

	public void addquestion(Question question)
	{
		this.QuestionList.Add(question);
	}

	public void addpreview(PreviewQuiz previewQuiz)
	{
		Previews.Add(previewQuiz);
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

		if (question.Description.Length > 60)
		{
            this.label.Text = Question.Description.Substring(0, 60) + "...";
        }
		else
		{
			this.label.Text = Question.Description;
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

class AikenFormat
{
	private string filePath;
	private List<Question> importedQuestions;

	public string FilePath
	{
		set { filePath = value; }
	}

	public List<Question> ImportQuestion
	{
		get { return importedQuestions; }
	}

	public void ReadTxt()
	{
        List<Question> questions = new List<Question>();
        string[] lines = File.ReadAllLines(filePath);

        Question currentQuestion = null;
		int Phase = 1;
		int Index = 1;
		int choiceNum = 0;
        foreach (string line in lines)
        {
			if (Phase == 0)
			{
				Phase = 1;
				continue;
			}
            if (Phase == 1)
            {
				if (currentQuestion != null)
				{
					questions.Add(currentQuestion);
					currentQuestion = null;
                }
				Phase = 2;
				choiceNum = 0;
				currentQuestion = new Question
				{
					Name = "",
					Description = line,
					Mark = 1,
                    Choices = new List<Choice>(5) { null, null, null, null, null }
                };
            }
            else if (Phase == 2)
            {
                if (line.StartsWith("A. "))
                {
					currentQuestion.Choices[0] = new Choice { Text = line.Substring(3), Grade = 0};
					choiceNum++;
                }
                else if (line.StartsWith("B. "))
                {
                    currentQuestion.Choices[1] = new Choice { Text = line.Substring(3), Grade = 0};
					choiceNum++;
                }
                else if (line.StartsWith("C. "))
                {
                    currentQuestion.Choices[2] = new Choice { Text = line.Substring(3), Grade = 0 };
					choiceNum++;
                }
				else if (line.StartsWith("D. "))
                {
                    currentQuestion.Choices[3] = new Choice { Text = line.Substring(3), Grade = 0 };
					choiceNum++;
                }
                else if (line.StartsWith("E. "))
                {
                    currentQuestion.Choices[4] = new Choice { Text = line.Substring(3), Grade = 0 };
					choiceNum++;
                }
				else if (line.StartsWith("ANSWER: ")) 
				{
					if (!String.IsNullOrEmpty(line.Substring(8).Trim()))
					{
                        string answer = line.Substring(8).Trim();
                        if (answer.Length > 1)
                        {
                            MessageBox.Show($"Error at line {Index}\nANSWER seems to have more than 1 characters");
                            return;
                        }
                        else if (answer.CompareTo("A") == 0)
                        {
							currentQuestion.Choices[0].Grade = 1;
                        }
                        else if (answer.CompareTo("B") == 0)
                        {
                            currentQuestion.Choices[1].Grade = 1;
                        }
                        else if (answer.CompareTo("C") == 0)
                        {
                            currentQuestion.Choices[2].Grade = 1;
                        }
                        else if (answer.CompareTo("D") == 0)
                        {
                            currentQuestion.Choices[3].Grade = 1;
                        }
                        else if (answer.CompareTo("E") == 0)
                        {
                            currentQuestion.Choices[4].Grade = 1;
                        }
						else
						{
                            MessageBox.Show($"Error at line {Index}\nANSWER is not one of A, B, C, D, E");
                            return;
                        }
                    }
					else
					{
                        MessageBox.Show($"Error at line {Index}\nANSWER seems to be empty!");
                        return;
                    }
					Phase = 0;
				}
				else
				{
					MessageBox.Show($"Error at line {Index}");
					return;
				}
            }
        }

        if (currentQuestion != null)
        {
            questions.Add(currentQuestion);
        }

        this.importedQuestions = questions;
    }

    public void ReadDocx()
    {

        using (WordprocessingDocument doc = WordprocessingDocument.Open(this.filePath, false))
        {
            var imageElements = doc.MainDocumentPart.Document.Descendants<DocumentFormat.OpenXml.Drawing.Blip>();

            foreach (var imageElement in imageElements)
            {
                string imageId = imageElement.Embed;

                var imagePart = doc.MainDocumentPart.GetPartById(imageId) as ImagePart;
                byte[] imageData = ReadImageData(imagePart);

                int location = GetImageLocation(imageElement);

                
            }
        }
    }

    private byte[] ReadImageData(ImagePart imagePart)
    {
        using (Stream stream = imagePart.GetStream())
        using (MemoryStream memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }

    private int GetImageLocation(DocumentFormat.OpenXml.Drawing.Blip imageElement)
    {
        var run = imageElement.Ancestors<DocumentFormat.OpenXml.Wordprocessing.Run>().FirstOrDefault();
        if (run != null)
        {
            var paragraph = run.Ancestors<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().FirstOrDefault();
            if (paragraph != null)
            {
                var document = paragraph.Ancestors<DocumentFormat.OpenXml.Wordprocessing.Document>().FirstOrDefault();
                if (document != null)
                {
                    var body = document.Body;
                    int lineIndex = body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>()
                        .TakeWhile(p => p != paragraph)
                        .Count() + 1;
                    return lineIndex;
                }
            }
        }

        return -1;
    }
}