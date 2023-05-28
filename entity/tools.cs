using System.Drawing;
using System.IO;
using System.Windows.Forms;

class RtfConverter
{
    public static string ConvertToPlainText(string rtf)
    {
        RichTextBox richTextBox = new RichTextBox();
        richTextBox.Font = new Font("Cascadia Mono", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

        richTextBox.Rtf = rtf;
        return richTextBox.Text;
    }

    public static string ConvertToRtf(string plainText, byte[] imageData)
    {
        RichTextBox richTextBox = new RichTextBox();
        richTextBox.Font = new Font("Cascadia Mono", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

        richTextBox.Text = plainText;
        Clipboard.SetImage(ConvertImageDataToImage(imageData));
        richTextBox.SelectionStart = richTextBox.Text.Length - 1;
        richTextBox.Paste();

        // Return the combined RTF
        return richTextBox.Rtf;
    }

    private static Image ConvertImageDataToImage(byte[] imageData)
    {
        using (MemoryStream stream = new MemoryStream(imageData))
        {
            Image image = Image.FromStream(stream);
            return image;
        }
    }
}

public class ImageMessageBox : Form
{
    private PictureBox pictureBox;

    public ImageMessageBox(Image image)
    {
        InitializeComponents(image);
    }

    private void InitializeComponents(Image image)
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