using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ExamApp;
using System.Drawing;
using Control = System.Windows.Forms.Control;
using DocumentFormat.OpenXml.Drawing.Charts;

class ImageWithLine
{
    private byte[] imageData;
    private int line;

    public byte[] ImageData
    {
        get { return imageData; }
        set { imageData = value; }
    }

    public int Line
    {
        get { return line; }
        set { line = value; }
    }
}

// Class to process Aiken format for questions text file
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

    // Read .txt file and convert its content to list of questions
    public Boolean ReadTxt()
    {
        try
        {
            List<Question> questions = new List<Question>();
            string[] lines = File.ReadAllLines(filePath);

            Question currentQuestion = null;
            int Phase = 1; // 1 is question text, 2 is question choices and 3 is the answer,
                           // 0 just represent checking the empty line in between
            int lineIndex = 0;
            int choiceNum = 0; // Number of choices of each questions
            foreach (string line in lines)
            {
                lineIndex++;
                if (Phase == 0)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        MessageBox.Show($"Error at line {lineIndex}\nNeeded empty line between 2 questions");
                        return false;
                    }
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
                        Choices = new List<Choice>(5) { null, null, null, null, null },
                        QuestionImages = new List<Control>()
                    };
                }
                else if (Phase == 2)
                {
                    if (line.StartsWith("A. "))
                    {
                        currentQuestion.Choices[0] = new Choice { Text = line.Substring(3), Grade = 0, ChoiceImages = new List<Control>() };
                        choiceNum++;
                    }
                    else if (line.StartsWith("B. "))
                    {
                        currentQuestion.Choices[1] = new Choice { Text = line.Substring(3), Grade = 0, ChoiceImages = new List<Control>() };
                        choiceNum++;
                    }
                    else if (line.StartsWith("C. "))
                    {
                        currentQuestion.Choices[2] = new Choice { Text = line.Substring(3), Grade = 0, ChoiceImages = new List<Control>() };
                        choiceNum++;
                    }
                    else if (line.StartsWith("D. "))
                    {
                        currentQuestion.Choices[3] = new Choice { Text = line.Substring(3), Grade = 0, ChoiceImages = new List<Control>() };
                        choiceNum++;
                    }
                    else if (line.StartsWith("E. "))
                    {
                        currentQuestion.Choices[4] = new Choice { Text = line.Substring(3), Grade = 0, ChoiceImages = new List<Control>() };
                        choiceNum++;
                    }
                    else if (line.StartsWith("ANSWER: "))
                    {
                        if (choiceNum < 2)
                        {
                            MessageBox.Show($"Error at line {lineIndex}\nQuestion has less than 2 choices!");
                            return false;
                        }
                        if (!String.IsNullOrEmpty(line.Substring(8).Trim()))
                        {
                            string answer = line.Substring(8).Trim();
                            if (answer.Length > 1)
                            {
                                MessageBox.Show($"Error at line {lineIndex}\nANSWER seems to have more than 1 characters");
                                return false;
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
                                MessageBox.Show($"Error at line {lineIndex}\nANSWER is not one of A, B, C, D, E");
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Error at line {lineIndex}\nANSWER seems to be empty!");
                            return false;
                        }
                        Phase = 0;
                    }
                    else
                    {
                        MessageBox.Show($"Error at line {lineIndex}\nError with input aiken format text file!");
                        return false;
                    }
                }
            }

            if (currentQuestion != null)
            {
                questions.Add(currentQuestion);
            }
            this.importedQuestions = questions;

            return true;
        }
        catch
        {
            MessageBox.Show("Cannot open file, possibly it is opened");
            return false;
        }
    }

    // Read .docx file and convert its content to list of questions
    public Boolean ReadDocx()
    {
        try
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(this.filePath, false))
            {
                List<Question> questions = new List<Question>();
                MainDocumentPart mainPart = doc.MainDocumentPart;
                var imageElements = doc.MainDocumentPart.Document.Descendants<DocumentFormat.OpenXml.Drawing.Blip>();

                // Get the images first
                List<ImageWithLine> images = new List<ImageWithLine>();
                foreach (var imageElement in imageElements)
                {
                    string imageId = imageElement.Embed;

                    var imagePart = doc.MainDocumentPart.GetPartById(imageId) as ImagePart;
                    byte[] imageData = ReadImage(imagePart);
                    int location = GetImageLocation(imageElement);

                    images.Add(new ImageWithLine { ImageData = imageData, Line = location });
                }

                if (mainPart != null)
                {
                    // Read the document text
                    var paragraphs = mainPart.Document.Body.Descendants<Paragraph>();

                    Question currentQuestion = null;
                    int Phase = 1; // 1 is question text, 2 is question choices and 3 is the answer,
                                   // 0 just represent checking the empty line in between
                    int lineIndex = 0; // Line index
                    int choiceNum = 0; // Number of choices of each questions
                    int imageIndex = 0; // ImageData[imageIndex].Line -> if has the same value as Index, we add it to the context
                    int imagesLen = images.Count;

                    foreach (var paragraph in paragraphs)
                    {
                        lineIndex++;

                        string text = paragraph.InnerText;
                        string[] lines = text.Split('\n');

                        // Process each line
                        foreach (var line in lines)
                        {
                            if (Phase == 0)
                            {
                                if (!string.IsNullOrEmpty(line))
                                {
                                    MessageBox.Show($"Error at line {lineIndex}\nNeeded empty line between 2 questions");
                                    return false;
                                }
                                Phase = 1;
                                continue;
                            }
                            else if (Phase == 1)
                            {
                                if (currentQuestion != null)
                                {
                                    questions.Add(currentQuestion);
                                    currentQuestion = null;
                                }
                                Phase = 2;
                                choiceNum = 0;

                                string qText = line;
                                List<Control> controls = new List<Control>();
                                while (imageIndex < imagesLen && images[imageIndex].Line == lineIndex)
                                {
                                    Image image = Utilities.ConvertImageDataToImage(images[imageIndex].ImageData);
                                    Image resizedImage = new Bitmap(image, Utilities.CalculateImageSize(image.Size, 400, 300));
                                    if (!Trestan.Utility.isImageCorrupted(image))
                                    {
                                        PictureBox thePic = new PictureBox
                                        {
                                            Image = resizedImage,
                                            Size = resizedImage.Size
                                        };
                                        controls.Add(thePic);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error!?");
                                    }
                                    imageIndex++;
                                }

                                currentQuestion = new Question
                                {
                                    Name = "",
                                    Description = qText,
                                    Mark = 1,
                                    Choices = new List<Choice>(5) { null, null, null, null, null },
                                    QuestionImages = controls
                                };
                            }
                            else if (Phase == 2)
                            {
                                if (line.StartsWith("A. "))
                                {
                                    string cText = line.Substring(3);
                                    List<Control> controls = new List<Control>();
                                    while (imageIndex < imagesLen && images[imageIndex].Line == lineIndex)
                                    {
                                        Image image = Utilities.ConvertImageDataToImage(images[imageIndex].ImageData);
                                        if (!Trestan.Utility.isImageCorrupted(image))
                                        {
                                            PictureBox thePic = new PictureBox
                                            {
                                                Image = image,
                                                Size = image.Size
                                            };
                                            controls.Add(thePic);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Error!?");
                                        }
                                        imageIndex++;
                                    }

                                    currentQuestion.Choices[0] = new Choice { Text = cText, Grade = 0, ChoiceImages = controls };
                                    choiceNum++;
                                }
                                else if (line.StartsWith("B. "))
                                {
                                    string cText = line.Substring(3);
                                    List<Control> controls = new List<Control>();
                                    while (imageIndex < imagesLen && images[imageIndex].Line == lineIndex)
                                    {
                                        Image image = Utilities.ConvertImageDataToImage(images[imageIndex].ImageData);
                                        if (!Trestan.Utility.isImageCorrupted(image))
                                        {
                                            PictureBox thePic = new PictureBox
                                            {
                                                Image = image,
                                                Size = image.Size
                                            };
                                            controls.Add(thePic);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Error!?");
                                        }
                                        imageIndex++;
                                    }

                                    currentQuestion.Choices[1] = new Choice { Text = cText, Grade = 0, ChoiceImages = controls };
                                    choiceNum++;
                                }
                                else if (line.StartsWith("C. "))
                                {
                                    string cText = line.Substring(3);
                                    List<Control> controls = new List<Control>();
                                    while (imageIndex < imagesLen && images[imageIndex].Line == lineIndex)
                                    {
                                        Image image = Utilities.ConvertImageDataToImage(images[imageIndex].ImageData);
                                        if (!Trestan.Utility.isImageCorrupted(image))
                                        {
                                            PictureBox thePic = new PictureBox
                                            {
                                                Image = image,
                                                Size = image.Size
                                            };
                                            controls.Add(thePic);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Error!?");
                                        }
                                        imageIndex++;
                                    }

                                    currentQuestion.Choices[2] = new Choice { Text = cText, Grade = 0, ChoiceImages = controls };
                                    choiceNum++;
                                }
                                else if (line.StartsWith("D. "))
                                {
                                    string cText = line.Substring(3);
                                    List<Control> controls = new List<Control>();
                                    while (imageIndex < imagesLen && images[imageIndex].Line == lineIndex)
                                    {
                                        Image image = Utilities.ConvertImageDataToImage(images[imageIndex].ImageData);
                                        if (!Trestan.Utility.isImageCorrupted(image))
                                        {
                                            PictureBox thePic = new PictureBox
                                            {
                                                Image = image,
                                                Size = image.Size
                                            };
                                            controls.Add(thePic);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Error!?");
                                        }
                                        imageIndex++;
                                    }

                                    currentQuestion.Choices[3] = new Choice { Text = cText, Grade = 0, ChoiceImages = controls };
                                    choiceNum++;
                                }
                                else if (line.StartsWith("E. "))
                                {
                                    string cText = line.Substring(3);
                                    List<Control> controls = new List<Control>();
                                    while (imageIndex < imagesLen && images[imageIndex].Line == lineIndex)
                                    {
                                        Image image = Utilities.ConvertImageDataToImage(images[imageIndex].ImageData);
                                        if (!Trestan.Utility.isImageCorrupted(image))
                                        {
                                            PictureBox thePic = new PictureBox
                                            {
                                                Image = image,
                                                Size = image.Size
                                            };
                                            controls.Add(thePic);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Error!?");
                                        }
                                        imageIndex++;
                                    }

                                    currentQuestion.Choices[4] = new Choice { Text = cText, Grade = 0 , ChoiceImages = controls };
                                    choiceNum++;
                                }
                                else if (line.StartsWith("ANSWER: "))
                                {
                                    if (choiceNum < 2)
                                    {
                                        MessageBox.Show($"Error at line {lineIndex}\nQuestion has less than 2 choices!");
                                        return false;
                                    }
                                    if (!String.IsNullOrEmpty(line.Substring(8).Trim()))
                                    {
                                        string answer = line.Substring(8).Trim();
                                        if (answer.Length > 1)
                                        {
                                            MessageBox.Show($"Error at line {lineIndex}\nANSWER seems to have more than 1 characters");
                                            return false;
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
                                            MessageBox.Show($"Error at line {lineIndex}\nANSWER is not one of A, B, C, D, E");
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show($"Error at line {lineIndex}\nANSWER seems to be empty!");
                                        return false;
                                    }
                                    Phase = 0;
                                }
                                else
                                {
                                    MessageBox.Show($"Error at line {lineIndex}\nError with input aiken format text file!");
                                    return false;
                                }
                            }
                        }
                    }

                    if (currentQuestion != null)
                    {
                        questions.Add(currentQuestion);
                    }
                    this.importedQuestions = questions;
                }
            }
            return true;
        }
        catch
        {
            MessageBox.Show("Cannnot open file, possible it is opened");
            return false;
        }
    }

    private byte[] ReadImage(ImagePart imagePart)
    {
        byte[] imageData;
        using (Stream stream = imagePart.GetStream())
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                imageData = memoryStream.ToArray();
            }
        }
        return imageData;
    }

    private int GetImageLocation(DocumentFormat.OpenXml.Drawing.Blip imageElement)
    {
        var run = imageElement.Ancestors<Run>().FirstOrDefault();
        if (run != null)
        {
            var paragraph = run.Ancestors<Paragraph>().FirstOrDefault();
            if (paragraph != null)
            {
                var document = paragraph.Ancestors<Document>().FirstOrDefault();
                if (document != null)
                {
                    var body = document.Body;
                    int lineIndex = body.Descendants<Paragraph>()
                        .TakeWhile(p => p != paragraph)
                        .Count() + 1;
                    return lineIndex;
                }
            }
        }

        return -1;
    }
}