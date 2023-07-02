using System.Collections.Generic;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Font = System.Drawing.Font;
using SautinSoft.Document;
using ExamApp;
using System.Threading.Tasks;

// utilities class
class Utilities
{
    // shuffle tool
    public static List<int> shuffleIndex;
    public static void Shuffle(int n)
    {
        shuffleIndex = new List<int>();
        for (int i = 0; i < n; i++)
        {
            shuffleIndex.Add(i);
        }
        Random random = new Random();
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (shuffleIndex[n], shuffleIndex[k]) = (shuffleIndex[k], shuffleIndex[n]);
        }
    }

    public static void NonShuffle(int n)
    {
        shuffleIndex = new List<int>();
        for (int i = 0; i < n; i++)
        {
            shuffleIndex.Add(i);
        }
    }

    // rtf -> plain text and vice versa
    public static string ConvertToPlainText(string rtf)
    {
        RichTextBox richTextBox = new RichTextBox
        {
            Font = new Font("Cascadia Mono", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0))),

            Rtf = rtf
        };
        return richTextBox.Text;
    }

    public static string ConvertToRtf(string plainText, byte[] imageData)
    {
        RichTextBox richTextBox = new RichTextBox
        {
            Font = new Font("Cascadia Mono", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0))),

            Text = plainText
        };
        Clipboard.SetImage(ConvertImageDataToImage(imageData));
        richTextBox.SelectionStart = richTextBox.Text.Length - 1;
        richTextBox.Paste();

        // Return the combined RTF
        return richTextBox.Rtf;
    }

    public static Image ConvertImageDataToImage(byte[] imageData)
    {
        using (MemoryStream stream = new MemoryStream(imageData))
        {
            Image image = Image.FromStream(stream);
            return image;
        }
    }

    // shorten file path for display
    public static string ShortenFilePath(string filePath, int maxLength)
    {
        const string ellipsis = "...";

        // If the file path is already shorter than the maximum length, return it as is
        if (filePath.Length <= maxLength)
        {
            return filePath;
        }

        // Split the file path into individual directory and file components
        string[] pathComponents = filePath.Split('\\');
        int numComponents = pathComponents.Length;

        // Calculate the maximum length for each component (directories and file name)
        int maxComponentLength = (maxLength - ellipsis.Length) / numComponents;

        // Construct the shortened file path by replacing intermediate directories with ellipses
        string shortenedPath = pathComponents[0];
        for (int i = 1; i < numComponents - 1; i++)
        {
            string component = pathComponents[i];
            if (component.Length > maxComponentLength)
            {
                component = component.Substring(0, maxComponentLength) + ellipsis;
            }
            shortenedPath += '\\' + component;
        }

        // Append the file name (last component) to the shortened path
        string fileName = pathComponents[numComponents - 1];
        if (fileName.Length > maxComponentLength)
        {
            fileName = fileName.Substring(0, maxComponentLength) + ellipsis;
        }
        shortenedPath += '\\' + fileName;

        return shortenedPath;
    }

    public static Size CalculateImageSize(Size originalSize, int maxWidth, int maxHeight)
    {
        // Calculate the aspect ratio of the original image
        double aspectRatio = (double)originalSize.Width / originalSize.Height;

        // Calculate the new width and height based on the aspect ratio and maximum dimensions
        int newWidth = maxWidth;
        int newHeight = (int)(maxWidth / aspectRatio);

        // If the calculated height exceeds the maximum height, recalculate based on the height instead
        if (newHeight > maxHeight)
        {
            newHeight = maxHeight;
            newWidth = (int)(maxHeight * aspectRatio);
        }

        return new Size(newWidth, newHeight);
    }
}

// just a control to check if an image enter correctly
public class ImageMessageBox : Form
{
    private PictureBox pictureBox;

    public ImageMessageBox(System.Drawing.Image image)
    {
        InitializeComponents(image);
    }

    private void InitializeComponents(System.Drawing.Image image)
    {
        pictureBox = new PictureBox
        {
            Image = image,
            SizeMode = PictureBoxSizeMode.AutoSize
        };

        Controls.Add(pictureBox);

        // Set the form properties
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Image Message Box";
    }
}

// turn rtf to pdf file
class PDFGenerator
{
    public static async Task MakePDF(Quiz quiz, string pdfFilePath, string password, Export_Form temp)
    {
        await Task.Run(() =>
        {
            List<string> choiceLabel = new List<string>(5) { "A", "B", "C", "D", "E" };
            Trestan.TRichTextBox richTextBoxTarget = new Trestan.TRichTextBox();
            int i = 1;
            int quizLen = quiz.QuestionList.Count;
            foreach (Question question in quiz.QuestionList)
            {
                temp.Invoke((MethodInvoker)(() => temp.Progress.Text = $"{i}/{quizLen}"));
                Trestan.TRichTextBox richTextBoxSource = new Trestan.TRichTextBox();
                try
                {
                    richTextBoxSource.Rtf = question.Description;
                }
                catch
                {
                    richTextBoxSource.Text = question.Description;
                }
                richTextBoxSource.Text = $"\nCâu {i}: " + richTextBoxSource.Text;
                richTextBoxSource.Font = new Font("Arial", 13, FontStyle.Regular);
                richTextBoxTarget.Select(richTextBoxTarget.TextLength, 0);
                richTextBoxTarget.SelectedRtf = richTextBoxSource.Rtf;

                int j = 0;
                foreach (Choice choice in question.Choices)
                {
                    if (choice == null)
                    {
                        continue;
                    }
                    try
                    {
                        richTextBoxSource.Rtf = choice.Text;
                    }
                    catch
                    {
                        richTextBoxSource.Text = choice.Text;
                    }
                    richTextBoxSource.Text = $"{choiceLabel[j]}. " + richTextBoxSource.Text;
                    richTextBoxSource.Font = new Font("Arial", 13, FontStyle.Regular);
                    richTextBoxTarget.Select(richTextBoxTarget.TextLength, 0);
                    richTextBoxTarget.SelectedRtf = richTextBoxSource.Rtf;
                    j++;
                }
                i++;
            }
            string tempFilePath = Path.ChangeExtension(Path.GetTempFileName(), "rtf");
            richTextBoxTarget.SaveFile(tempFilePath, RichTextBoxStreamType.RichText);

            DocumentCore dc = DocumentCore.Load(tempFilePath);
            if (!string.IsNullOrEmpty(password))
            {
                PdfSaveOptions pdfSaveOptions = new PdfSaveOptions();
                pdfSaveOptions.EncryptionDetails.UserPassword = password;
                pdfSaveOptions.EncryptionDetails.EncryptionAlgorithm = PdfEncryptionAlgorithm.RC4_128;
                pdfSaveOptions.EncryptionDetails.Permissions = PdfPermissions.Printing;
                dc.Save(pdfFilePath, pdfSaveOptions);
            }
            else
            {
                dc.Save(pdfFilePath);
            }
        });
    }
}