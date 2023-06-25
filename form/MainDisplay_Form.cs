using ExamApp.form;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExamApp
{
    public partial class ExamApp : Form
    {
        // global variables
        private Edit_Form BigPanel1;
        private EditQuestion_Form BigPanel2;
        private QuizCreate_Form BigPanel3;
        private QuizAttempt_Form BigPanel4;
        private QuizEdit_Form BigPanel5;
        private AddQuestionToQuiz2_Form BigPanel5_2;
        private QuizPlay_Form BigPanel6;
        private Question CurrentQuestion;
        private readonly List<Category> Categories;
        private Category CurrentCategory;
        private readonly List<Quiz> Quizzes;
        private Quiz CurrentQuiz;
        private string importFilePath;

        public ExamApp()
        {
            InitializeComponent();
            Categories = new List<Category>
            {
                new Category(null, new List<Category>(), "Default", "Default category", -1, 0, new List<Question>())
            };
            Quizzes = new List<Quiz>();
        }

        //
/* form opening */

        // function to open edit form
        private void OpenEditPanel(Edit_Form form, int index)
        {
            BigPanel1 = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            // Events
            form.DirectingButton1.Click += new EventHandler(BP1_CreateQuestion_Click);
            form.SubcategoriesQ.CheckedChanged += new EventHandler(BP1_ShowSubcategoriesCheckBox_CheckChanged);
            form.addCategoryButton.Click += new EventHandler(BP1_AddCategory_Click);

            form.ImportPanel.DragEnter += BP1_ImportPanel_DragEnter;
            form.ImportPanel.DragDrop += BP1_ImportPanel_DragDrop;
            form.ImportButton.Click += new EventHandler(BP1_ImportButton_Click);
            //
            // Category ComboBox in Questions tab
            //
            form.QCategory.SelectedIndexChanged += new EventHandler(BP1_QuestionCategory_SelectIndexChanged);
            form.QCategory.Items.Clear();
            form.QCategory.DataSource = Categories;
            form.QCategory.DisplayMember = "NameAndGen";
            //
            // Category ComboBox in Categories tab
            //
            form.PCategory.SelectedIndexChanged += new EventHandler(BP1_Category_SelectIndexChanged);
            form.PCategory.Items.Clear();
            form.PCategory.DataSource = Categories;
            form.PCategory.DisplayMember = "NameAndGen";
            //
            form.DirectTab.SelectedIndex = index;
            mainpanel.Controls.Add(form);
            mainpanel.Tag = form;
            form.Show();
        }

        // function to open edit question form
        private void OpenEditQuestionPanel(EditQuestion_Form form, Boolean show)
        {
            BigPanel2 = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            //
            // add events to buttons
            //
            form.FunctionButton1.Click += delegate (object sender, EventArgs e) { BP2_SaveAndQuitButton_Click(sender, e, CurrentQuestion); };
            form.FunctionButton2.Click += new EventHandler(BP2_CancelButton_Click);
            form.FunctionButton3.Click += delegate (object sender, EventArgs e) { BP2_SaveAndContinueButton_Click(sender, e, CurrentQuestion); };
            //
            // Category ComboBox
            //
            form.QCategory.SelectedIndexChanged += new EventHandler(BP2_QuestionCategory_SelectIndexChanged);
            form.QCategory.Items.Clear();
            form.QCategory.DataSource = Categories;
            form.QCategory.DisplayMember = "NameAndGen";
            //
            if (show)
            {
                mainpanel.Controls.Add(form);
                mainpanel.Tag = form;
                form.Show();
            }
        }

        // function to open quiz form
        private void OpenQuizCreatePanel(QuizCreate_Form form)
        {
            BigPanel3 = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            //
            form.CreateQuizButton.Click += new EventHandler(BP3_CreateButton_Click);
            form.CANCELbutton.Click += new EventHandler(BP3_CancelButton_Click);
            //
            mainpanel.Controls.Add(form);
            mainpanel.Tag = form;
            form.Show();
        }

        // function to open quiz attempt form
        private void OpenQuizAttemptPanel(QuizAttempt_Form form)
        {
            BigPanel4 = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            //
            form.GearSetting.Click += delegate (object sender, EventArgs e) { BP4_GearIconLabel_Click(sender, e, CurrentQuiz); };
            form.StartAttemptButton.Click += new EventHandler(BP4_Popup_StartQuizAttempt_Click);
            //
            mainpanel.Controls.Add(form);
            mainpanel.Tag = form;
            form.Show();
        }

        // function to open quiz attempt form
        private void OpenQuizEditPanel(QuizEdit_Form form)
        {
            BigPanel5 = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            //
            BigPanel5.AddFromQuestionBankLabel.Click += new EventHandler(BP5_Popup_AddQuestionFromQuestionBank_Click);
            BigPanel5.SaveQuizBtn.Click += new EventHandler(BP5_SaveButton_Click);
            //
            mainpanel.Controls.Add(form);
            mainpanel.Tag = form;
            form.Show();
        }

        // function to open add question to quiz 2 form ("from question bank")
        private void OpenAddQuestionToQuiz2Panel(AddQuestionToQuiz2_Form form)
        {
            BigPanel5_2 = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            //
            form.SubcategoryCheckBox.CheckedChanged += new EventHandler(BP5_2_ShowSubcategoriesCheckBox_CheckChanged);
            form.AddToQuiz.Click += new EventHandler(BP5_2_AddSelectedToQuizButton_Click);
            //
            mainpanel.Controls.Add(form);
            mainpanel.Tag = form;
            form.Show();
        }

        // function to open quiz_play form
        private void OpenQuizPlayPanel(QuizPlay_Form form)
        {
            BigPanel6 = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            //
            BigPanel6.FlagLabel.Click += new EventHandler(BP6_FlagLabel_Click);
            BigPanel6.Back.Click += new EventHandler(BP6_BackLabel_Click);
            BigPanel6.Next.Click += new EventHandler(BP6_NextLabel_Click);
            BigPanel6.FinishAttempt.Click += new EventHandler(BP6_FinishAttemptLabel_Click);
            //
            mainpanel.Controls.Add(form);
            mainpanel.Tag = form;
            form.Show();
        }

        //
/* Main Display Form */
/* MainPanel */

        private void MP_UpdateQuizDisplay()
        {
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            quizFlowLayout.Controls.Clear();
            foreach (Quiz quiz in Quizzes)
            {
                QuizBlock temp = new QuizBlock(quiz);
                temp.QName.Click += delegate (object sender1, EventArgs e1) { MP_Quiz_NameLabel_Click(sender1, e1, temp.Quiz); };
                quizFlowLayout.Controls.Add(temp);
            }

            mainpanel.Controls.Add(quizFlowLayout);
        }

        // "setting" label (gear icon)
        private void MP_GearIconLabel_Click(object sender, EventArgs e)
        {
            if (button1.Visible == true)
            {
                if (popup.Visible == false)
                {
                    popup.Location = new Point(heading.Right - popup.Width, heading.Bottom - 60);
                    popup.Show();
                }
                else
                {
                    popup.Hide();
                }
            }
        }

        // "turn edit on" button
        private void MP_TurnEditOnButton_Click(object sender, EventArgs e)
        {
            button1.Hide();
            functionbutton1.Hide();
            popup.Hide();
            heading.Size = new Size(1114, 86);

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (BigPanel3 == null)
            {
                OpenQuizCreatePanel(new QuizCreate_Form());
            }
            else
            {
                mainpanel.Controls.Add(BigPanel3);
                mainpanel.Tag = BigPanel3;
                BigPanel3.Show();
            }
            direction1.Cursor = Cursors.Hand;
            direction1.ForeColor = Color.IndianRed;
            direction1.Click += new EventHandler(MP_FirstDirection_Click);
            slash1.Show();
            direction2.Text = "Adding a new quiz";
            direction2.ForeColor = SystemColors.ControlText;
            direction2.Cursor = Cursors.Default;
            direction2.Show();
        }

        // click event for clicking a quiz name on home page, directing to quiz attempt form
        private void MP_Quiz_NameLabel_Click(object _1, EventArgs _2, Quiz quiz)
        {
            button1.Hide();
            functionbutton1.Hide();
            popup.Hide();
            heading.Size = new Size(1114, 86);

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }
            if (BigPanel4 == null)
            {
                OpenQuizAttemptPanel(new QuizAttempt_Form());
            }
            else
            {
                mainpanel.Controls.Add(BigPanel4);
                mainpanel.Tag = BigPanel4;
                BigPanel4.Show();
            }
            //
            CurrentQuiz = quiz;
            BigPanel4.QuizName.Text = quiz.Name;
            if (quiz.Time.TimeCoefficient == 0)
            {
                BigPanel4.TimeLimitLabel.Text = "None";
            }
            else
            {
                BigPanel4.TimeLimitLabel.Text = $"{quiz.Time.TimeCoefficient} {quiz.Time.TimeUnit}";
            }
            //
            direction1.Cursor = Cursors.Hand;
            direction1.ForeColor = Color.IndianRed;
            direction1.Click += new EventHandler(MP_FirstDirection_Click);
            direction2.ForeColor = SystemColors.ControlText;
            direction2.Cursor = Cursors.Default;
            if (quiz.Name.Length > 50)
            {
                direction2.Text = quiz.Name.Substring(0, 40) + "...";
            }
            else
            {
                direction2.Text = quiz.Name;
            }
            slash1.Show();
            direction2.Show();
        }

        // direct to questions tab in edit form
        private void MP_Popup_Questions_Click(object sender, EventArgs e)
        {
            popup.Hide();
            functionbutton1.Hide();
            button1.Hide();
            heading.Size = new Size(1114, 86);

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (BigPanel1 == null)
            {
                OpenEditPanel(new Edit_Form(), 0);
            }
            else
            {
                mainpanel.Controls.Add(BigPanel1);
                mainpanel.Tag = BigPanel1;
                BigPanel1.DirectTab.SelectedIndex = 0;
                BigPanel1.Show();
            }

            direction1.Cursor = Cursors.Hand;
            direction1.ForeColor = Color.IndianRed;
            direction1.Click += new EventHandler(MP_FirstDirection_Click);
        }

        // direct to categories tab in edit form
        private void MP_Popup_Categories_Click(object sender, EventArgs e)
        {
            popup.Hide();
            functionbutton1.Hide();
            button1.Hide();
            heading.Size = new Size(1114, 86);

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (BigPanel1 == null)
            {
                OpenEditPanel(new Edit_Form(), 1);
            }
            else
            {
                mainpanel.Controls.Add(BigPanel1);
                mainpanel.Tag = BigPanel1;
                BigPanel1.DirectTab.SelectedIndex = 1;
                BigPanel1.Show();
            }

            direction1.Cursor = Cursors.Hand;
            direction1.ForeColor = Color.IndianRed;
            direction1.Click += new EventHandler(MP_FirstDirection_Click);
        }

        // direct to import tab in edit form
        private void MP_Popup_Import_Click(object sender, EventArgs e)
        {
            popup.Hide();
            functionbutton1.Hide();
            button1.Hide();
            heading.Size = new Size(1114, 86);

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (BigPanel1 == null)
            {
                OpenEditPanel(new Edit_Form(), 2);
            }
            else
            {
                mainpanel.Controls.Add(BigPanel1);
                mainpanel.Tag = BigPanel1;
                BigPanel1.DirectTab.SelectedIndex = 2;
                BigPanel1.Show();
            }

            direction1.Cursor = Cursors.Hand;
            direction1.ForeColor = Color.IndianRed;
            direction1.Click += new EventHandler(MP_FirstDirection_Click);
        }


        //
/* Edit Form */
/* BigPanel1 */

        // function to update questions display in questions tab in edit form
        private void BP1_UpdateQuestionDisplay(bool mode)
        {
            CurrentCategory = BigPanel1.QCategory.SelectedItem as Category;
            if (CurrentCategory != null)
            {
                BigPanel1.QBox.Controls.Clear();
                if (mode)
                {
                    foreach (Question question in CurrentCategory.QuestionList)
                    {
                        QuestionBlock temp = new QuestionBlock(question);
                        BigPanel1.QBox.Controls.Add(temp);
                        temp.Edit.Click += delegate (object sender1, EventArgs e1) { BP1_Question_Edit_Click(sender1, e1, temp.Question); };
                        temp.Delete.Click += delegate (object sender1, EventArgs e1) { BP1_Question_Delete_Click(sender1, e1, temp.Question); };
                    }

                    foreach (Category subcategory in CurrentCategory.Child)
                    {
                        foreach (Question question in subcategory.QuestionList)
                        {
                            QuestionBlock temp = new QuestionBlock(question);
                            BigPanel1.QBox.Controls.Add(temp);
                            temp.Edit.Click += delegate (object sender1, EventArgs e1) { BP1_Question_Edit_Click(sender1, e1, temp.Question); };
                            temp.Delete.Click += delegate (object sender1, EventArgs e1) { BP1_Question_Delete_Click(sender1, e1, temp.Question); };
                        }
                    }
                }
                else
                {
                    foreach (Question question in CurrentCategory.QuestionList)
                    {
                        QuestionBlock temp = new QuestionBlock(question);
                        BigPanel1.QBox.Controls.Add(temp);
                        temp.Edit.Click += delegate (object sender1, EventArgs e1) { BP1_Question_Edit_Click(sender1, e1, temp.Question); };
                        temp.Delete.Click += delegate (object sender1, EventArgs e1) { BP1_Question_Delete_Click(sender1, e1, temp.Question); };
                    }
                }
            }
        }

        // event when category items changed, it will update the category combobox
        private void BP1_Category_ItemsChanged()
        {
            if (BigPanel1 != null)
            {
                BigPanel1.QCategory.DataSource = null;
                BigPanel1.QCategory.DataSource = Categories;
                BigPanel1.QCategory.DisplayMember = "NameAndGen";
                //
                BigPanel1.PCategory.DataSource = null;
                BigPanel1.PCategory.DataSource = Categories;
                BigPanel1.PCategory.DisplayMember = "NameAndGen";
                //
            }
            if (BigPanel2 != null)
            {
                BigPanel2.QCategory.DataSource = null;
                BigPanel2.QCategory.DataSource = Categories;
                BigPanel2.QCategory.DisplayMember = "NameAndGen";
            }
        }

        // set CurrentCategory as the selected items in category combobox in questions tab
        private void BP1_QuestionCategory_SelectIndexChanged(object sender, EventArgs e)
        {
            if (BigPanel1.QCategory.SelectedItem != null)
            {
                BP1_UpdateQuestionDisplay(BigPanel1.SubcategoriesQ.Checked);
            }
        }

        // set CurrentCategory as the selected items in category combobox in categories tab
        private void BP1_Category_SelectIndexChanged(object sender, EventArgs e)
        {
            if (BigPanel1.PCategory.SelectedItem != null)
            {
                CurrentCategory = BigPanel1.PCategory.SelectedItem as Category;
            }
        }

        // direct to edit question form when click the create question button
        private void BP1_CreateQuestion_Click(object sender, EventArgs e)
        {
            if (BigPanel1.Visible == false)
            {
                return;
            }

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (BigPanel2 == null)
            {
                OpenEditQuestionPanel(new EditQuestion_Form(), true);
            }
            else
            {
                BigPanel2.MainPanel.AutoScrollPosition = new Point(0, 0);
                BigPanel2.SubMainPanel.AutoScrollPosition = new Point(0, 0);
                BigPanel2.ChoiceP.AutoScrollPosition = new Point(0, 0);
                BigPanel2.MoreChoiceP.Hide();
                mainpanel.Controls.Add(BigPanel2);
                mainpanel.Tag = BigPanel2;
                BigPanel2.Show();
            }

            BigPanel2.BigLabel.Text = "Adding a Multiple choices question";
            BigPanel1.Hide();
            slash1.Show();
            slash2.Show();
            direction2.Text = "Question Bank";
            direction2.Show();
            direction3.Show();
            direction2.Cursor = Cursors.Hand;
            direction2.ForeColor = Color.IndianRed;
            direction2.Click += new EventHandler(MP_SecondDirection_Click);
        }

        // checkbox to also show questions from subcategories
        private void BP1_ShowSubcategoriesCheckBox_CheckChanged(object sender, EventArgs e)
        {
            BP1_UpdateQuestionDisplay(BigPanel1.SubcategoriesQ.Checked);
        }

        // event for edit label beside each question in questions, edit form to edit the selected question
        private void BP1_Question_Edit_Click(object _1, EventArgs _2, Question question)
        {
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (BigPanel2 == null)
            {
                OpenEditQuestionPanel(new EditQuestion_Form(), false);
            }

            CurrentQuestion = question;
            BigPanel2.QName.Text = question.Name;
            try
            {
                BigPanel2.QText.Rtf = question.Description;
            }
            catch
            {
                BigPanel2.QText.Text = question.Description;
            }
            BigPanel2.QMark.Text = Convert.ToString(question.Mark);

            if (question.Choices[2] != null || question.Choices[3] != null || question.Choices[4] != null)
            {
                BigPanel2.MoreChoiceP.Show();
            }

            for (int i = 0; i <= 4; i++)
            {
                Choice choice = CurrentQuestion.Choices[i];
                if (choice != null)
                {
                    if (i == 0)
                    {
                        try
                        {
                            BigPanel2.C1Text.Rtf = choice.Text;
                        }
                        catch
                        {
                            BigPanel2.C1Text.Text = choice.Text;
                        }
                        if (choice.Grade != 0)
                        {
                            BigPanel2.C1Grade.Text = Convert.ToString(choice.Grade * 100.0) + "%";
                        }
                        else
                        {
                            BigPanel2.C1Grade.Text = "None";
                        }
                    }
                    else if (i == 1)
                    {
                        try
                        {
                            BigPanel2.C2Text.Rtf = choice.Text;
                        }
                        catch
                        {
                            BigPanel2.C2Text.Text = choice.Text;
                        }
                        if (choice.Grade != 0)
                        {
                            BigPanel2.C2Grade.Text = Convert.ToString(choice.Grade * 100.0) + "%";
                        }
                        else
                        {
                            BigPanel2.C2Grade.Text = "None";
                        }
                    }
                    else if (i == 2)
                    {
                        try
                        {
                            BigPanel2.C3Text.Rtf = choice.Text;
                        }
                        catch
                        {
                            BigPanel2.C3Text.Text = choice.Text;
                        }
                        if (choice.Grade != 0)
                        {
                            BigPanel2.C3Grade.Text = Convert.ToString(choice.Grade * 100.0) + "%";
                        }
                        else
                        {
                            BigPanel2.C3Grade.Text = "None";
                        }
                    }
                    else if (i == 3)
                    {
                        try
                        {
                            BigPanel2.C4Text.Rtf = choice.Text;
                        }
                        catch
                        {
                            BigPanel2.C4Text.Text = choice.Text;
                        }
                        if (choice.Grade != 0)
                        {
                            BigPanel2.C4Grade.Text = Convert.ToString(choice.Grade * 100.0) + "%";
                        }
                        else
                        {
                            BigPanel2.C4Grade.Text = "None";
                        }
                    }
                    else if (i == 4)
                    {
                        try
                        {
                            BigPanel2.C5Text.Rtf = choice.Text;
                        }
                        catch
                        {
                            BigPanel2.C5Text.Text = choice.Text;
                        }
                        if (choice.Grade != 0)
                        {
                            BigPanel2.C5Grade.Text = Convert.ToString(choice.Grade * 100.0) + "%";
                        }
                        else
                        {
                            BigPanel2.C5Grade.Text = "None";
                        }
                    }
                }
            }

            // Change the heading
            slash2.Show();
            direction3.Show();
            direction2.Cursor = Cursors.Hand;
            direction2.ForeColor = Color.IndianRed;
            direction2.Click += new EventHandler(MP_SecondDirection_Click);

            // Switch big panel
            BigPanel1.Hide();
            BigPanel2.BigLabel.Text = "Editing a Multiple choices question";
            BigPanel2.MainPanel.AutoScrollPosition = new Point(0, 0);
            BigPanel2.SubMainPanel.AutoScrollPosition = new Point(0, 0);
            BigPanel2.ChoiceP.AutoScrollPosition = new Point(0, 0);
            mainpanel.Controls.Add(BigPanel2);
            mainpanel.Tag = BigPanel2;
            BigPanel2.Show();
        }

        // event for delete label beside each question in questions, delete the selected question
        private void BP1_Question_Delete_Click(object _1, EventArgs _2, Question question)
        {
            if (question != null)
            {
                question.Category.QuestionList.Remove(question);
                BP1_Category_ItemsChanged();
                BP1_UpdateQuestionDisplay(BigPanel1.SubcategoriesQ.Checked);
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        // add category event for button
        private void BP1_AddCategory_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BigPanel1.CName.Text))
            {
                MessageBox.Show("Category name is invalid");
                return;
            }

            int cid = -1;
            if (!string.IsNullOrWhiteSpace(BigPanel1.CId.Text))
            {
                try
                {
                    cid = Convert.ToInt32(cid);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Please enter a valid Id");
                }
            }

            Category Cparent = BigPanel1.PCategory.SelectedItem as Category;
            Category category = new Category(Cparent, new List<Category>(), BigPanel1.CName.Text, BigPanel1.CInfo.Text, cid, Cparent.Gen + 1, new List<Question>());
            Cparent.AddChild(category);
            int index = Categories.FindIndex(cat => cat == Cparent);

            if (index != -1)
            {
                Categories.Insert(index + 1, category);
            }
            else
            {
                Categories.Add(category);
            }

            // reset textbox
            BigPanel1.CName.ResetText();
            BigPanel1.CInfo.ResetText();
            BigPanel1.CId.ResetText();
            BigPanel1.PCategory.SelectedIndex = 0;
            //
            BP1_Category_ItemsChanged();
            BigPanel1.DirectTab.SelectedIndex = 0;
            MessageBox.Show("Successfully created a new category!");
        }

        // drag enter event when drag files on import panel
        private void BP1_ImportPanel_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the dragged data contains file(s)
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get the array of dropped files
                string[] filepaths = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Check one file limit entrance
                if (filepaths.Length > 1)
                {
                    MessageBox.Show("Please drop 1 file at once");
                    return;
                }

                // Check the file type and size for each dropped file
                foreach (string filepath in filepaths)
                {
                    // Check file extension
                    string extension = System.IO.Path.GetExtension(filepath);
                    if (extension != ".txt" && extension != ".docx")
                    {
                        // Invalid file type, reject the drag and drop operation
                        e.Effect = DragDropEffects.None;
                        return;
                    }

                    // Check file size
                    long fileSize = new System.IO.FileInfo(filepath).Length;
                    if (fileSize > 50 * 1024 * 1024) // MAX 50MB
                    {
                        // File size exceeds the limit, reject the drag and drop operation
                        e.Effect = DragDropEffects.None;
                        return;
                    }
                }

                // All files are valid, allow the drag and drop operation
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                // No file(s) present in the dragged data, reject the drag and drop operation
                e.Effect = DragDropEffects.None;
            }
        }

        // drag drop event when drag files on import panel
        private void BP1_ImportPanel_DragDrop(object sender, DragEventArgs e)
        {
            // Get the dropped data
            string[] filepaths = (string[])e.Data.GetData(DataFormats.FileDrop);
            importFilePath = filepaths[0];
            string extension = System.IO.Path.GetExtension(importFilePath);

            BigPanel1.closeImportBtn.Show();
            BigPanel1.closeImportBtn.Click += new EventHandler(BP1_CloseImportFileButton_Click);
            BigPanel1.importLabel.Text = importFilePath;
            if (extension == ".txt")
            {
                BigPanel1.importImageLabel.Image = Properties.Resources.txtIcon;
            }
            else
            {
                BigPanel1.importImageLabel.Image = Properties.Resources.docxIcon;
            }
        }

        // click event for "x" button appeared when drag a file successfully
        private void BP1_CloseImportFileButton_Click(object sender, EventArgs e)
        {
            BigPanel1.importImageLabel.Image = Properties.Resources.icon4;
            BigPanel1.importLabel.Text = "You can drag and drop files here to add them.";
            importFilePath = "";
            BigPanel1.closeImportBtn.Hide();
            BigPanel1.closeImportBtn.Click -= new EventHandler(BP1_CloseImportFileButton_Click);
        }

        // click event for IMPORT button
        private void BP1_ImportButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(importFilePath))
            {
                MessageBox.Show("Please enter a file first!");
                return;
            }

            AikenFormat aikenFormat = new AikenFormat { FilePath = importFilePath };
            string extension = System.IO.Path.GetExtension(importFilePath);

            bool isDone;
            if (extension == ".txt")
            {
                isDone = aikenFormat.ReadTxt();
            }
            else
            {
                isDone = aikenFormat.ReadDocx();
            }

            if (isDone)
            {
                if (aikenFormat.ImportQuestion != null)
                {
                    foreach (Question question in aikenFormat.ImportQuestion)
                    {
                        question.Category = Categories[0];
                        Categories[0].AddQuestion(question);
                    }
                    BP1_UpdateQuestionDisplay(false);
                }

                int importedQuestionNum = aikenFormat.ImportQuestion.Count();
                if (importedQuestionNum == 0)
                {
                    MessageBox.Show("Nothing imported!");
                }
                else if (importedQuestionNum == 1)
                {
                    MessageBox.Show("Successfully imported 1 question");
                }
                else
                {
                    MessageBox.Show($"Successfully imported {importedQuestionNum} questions");
                }
            }

            BigPanel1.importImageLabel.Image = Properties.Resources.icon4;
            BigPanel1.importLabel.Text = "You can drag and drop files here to add them.";
            importFilePath = "";
            BigPanel1.closeImportBtn.Hide();
            BigPanel1.closeImportBtn.Click -= new EventHandler(BP1_CloseImportFileButton_Click);
        }

        //
/* Edit Question Form */
/* BigPanel2 */

        // set CurrentCategory as the selected items in category combobox in edit question
        private void BP2_QuestionCategory_SelectIndexChanged(object sender, EventArgs e)
        {
            if (BigPanel2.QCategory.SelectedItem != null)
            {
                CurrentCategory = BigPanel2.QCategory.SelectedItem as Category;
            }
        }

        // save and quit button in edit question form
        private void BP2_SaveAndQuitButton_Click(object _1, EventArgs _2, Question question)
        {
            if (string.IsNullOrWhiteSpace(BigPanel2.QName.Text))
            {
                MessageBox.Show("Question Name is empty");
                return;
            }

            if (string.IsNullOrWhiteSpace(BigPanel2.QText.Text))
            {
                MessageBox.Show("Question Text is empty");
                return;
            }

            if (string.IsNullOrWhiteSpace(BigPanel2.QMark.Text))
            {
                MessageBox.Show("Default Mark is empty");
                return;
            }

            double mark;

            try
            {
                mark = Convert.ToDouble(BigPanel2.QMark.Text);
            }

            catch (FormatException)
            {
                MessageBox.Show("Please input a valid value for Default Mark!");
                return;
            }


            Question tempQuestion = new Question
            {
                Name = BigPanel2.QName.Text,
                Description = BigPanel2.QText.Rtf,
                Mark = mark,
                Choices = new List<Choice>(5) { null, null, null, null, null }
            };

            int choiceNum = 0;
            if (!string.IsNullOrWhiteSpace(BigPanel2.C1Text.Text))
            {
                double grade = 0;

                if (BigPanel2.C1Grade.Text != "None")
                {
                    grade = Convert.ToDouble(BigPanel2.C1Grade.Text.TrimEnd('%')) / 100.0;
                }

                Choice choice = new Choice
                {
                    Text = BigPanel2.C1Text.Rtf,
                    Grade = grade
                };

                tempQuestion.Choices[0] = choice;
                choiceNum++;
            }

            if (!string.IsNullOrWhiteSpace(BigPanel2.C2Text.Text))
            {
                double grade = 0;
                if (BigPanel2.C2Grade.Text != "None")
                {
                    grade = Convert.ToDouble(BigPanel2.C2Grade.Text.TrimEnd('%')) / 100.0;
                }
                Choice choice = new Choice
                {
                    Text = BigPanel2.C2Text.Rtf,
                    Grade = grade
                };
                tempQuestion.Choices[1] = choice;
                choiceNum++;
            }

            if (!string.IsNullOrWhiteSpace(BigPanel2.C3Text.Text))
            {
                double grade = 0;
                if (BigPanel2.C3Grade.Text != "None")
                {
                    grade = Convert.ToDouble(BigPanel2.C3Grade.Text.TrimEnd('%')) / 100.0;
                }
                Choice choice = new Choice
                {
                    Text = BigPanel2.C3Text.Rtf,
                    Grade = grade
                };
                tempQuestion.Choices[2] = choice;
                choiceNum++;
            }

            if (!string.IsNullOrWhiteSpace(BigPanel2.C4Text.Text))
            {
                double grade = 0;
                if (BigPanel2.C4Grade.Text != "None")
                {
                    grade = Convert.ToDouble(BigPanel2.C4Grade.Text.TrimEnd('%')) / 100.0;
                }
                Choice choice = new Choice
                {
                    Text = BigPanel2.C4Text.Rtf,
                    Grade = grade
                };
                tempQuestion.Choices[3] = choice;
                choiceNum++;
            }

            if (!string.IsNullOrWhiteSpace(BigPanel2.C5Text.Text))
            {
                double grade = 0;
                if (BigPanel2.C5Grade.Text != "None")
                {
                    grade = Convert.ToDouble(BigPanel2.C5Grade.Text.TrimEnd('%')) / 100.0;
                }
                Choice choice = new Choice
                {
                    Text = BigPanel2.C5Text.Rtf,
                    Grade = grade
                };
                tempQuestion.Choices[4] = choice;
                choiceNum++;
            }

            if (choiceNum < 2)
            {
                MessageBox.Show("Please enter more than one choice");
                return;
            }

            if (question == null)
            {
                tempQuestion.Category = CurrentCategory;
                CurrentCategory.AddQuestion(tempQuestion);
                BP1_Category_ItemsChanged();
            }
            else
            {
                question.Name = tempQuestion.Name;
                question.Description = tempQuestion.Description;
                question.Mark = tempQuestion.Mark;
                question.Choices = tempQuestion.Choices;
                if (CurrentCategory != question.Category)
                {
                    question.Category.QuestionList.Remove(question);
                    question.Category = CurrentCategory;
                    CurrentCategory.AddQuestion(question);
                    BP1_Category_ItemsChanged();
                }
                CurrentQuestion = null;
            }

            //
            BigPanel2.QName.ResetText();
            BigPanel2.QText.ResetText();
            BigPanel2.QMark.Text = "1";
            BigPanel2.C1Text.ResetText();
            BigPanel2.C2Text.ResetText();
            BigPanel2.C3Text.ResetText();
            BigPanel2.C4Text.ResetText();
            BigPanel2.C5Text.ResetText();
            BigPanel2.C1Grade.Text = "None";
            BigPanel2.C2Grade.Text = "None";
            BigPanel2.C3Grade.Text = "None";
            BigPanel2.C4Grade.Text = "None";
            BigPanel2.C5Grade.Text = "None";
            //
            slash2.Hide();
            direction3.Hide();
            direction2.Click -= new EventHandler(MP_SecondDirection_Click);
            direction2.ForeColor = Color.Black;
            direction2.Cursor = Cursors.Default;
            BP1_UpdateQuestionDisplay(BigPanel1.SubcategoriesQ.Checked);
            //
            mainpanel.Controls.RemoveAt(0);
            mainpanel.Controls.Add(BigPanel1);
            mainpanel.Tag = BigPanel1;
            BigPanel1.Show();
        }

        // save and continue button in edit question form
        private void BP2_SaveAndContinueButton_Click(object _1, EventArgs _2, Question question)
        {
            if (string.IsNullOrWhiteSpace(BigPanel2.QName.Text))
            {
                MessageBox.Show("Question Name is empty");
                return;
            }

            if (string.IsNullOrWhiteSpace(BigPanel2.QText.Text))
            {
                MessageBox.Show("Question Text is empty");
                return;
            }

            if (String.IsNullOrWhiteSpace(BigPanel2.QMark.Text))
            {
                MessageBox.Show("Default Mark is empty");
                return;
            }

            double mark;

            try
            {
                mark = Convert.ToDouble(BigPanel2.QMark.Text);
            }

            catch (FormatException)
            {
                MessageBox.Show("Please input a valid value for Default Mark!");
                return;
            }

            DialogResult result = MessageBox.Show("Do you want to save this question?", "Save question", MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
            {
                return;
            }

            if (question == null)
            {
                question = new Question
                {
                    Name = BigPanel2.QName.Text,
                    Description = BigPanel2.QText.Rtf,
                    Mark = mark,
                    Choices = new List<Choice>(5) { null, null, null, null, null },
                    Category = CurrentCategory
                };

                CurrentCategory.AddQuestion(question);
                BP1_Category_ItemsChanged();

                if (!string.IsNullOrWhiteSpace(BigPanel2.C1Text.Text))
                {
                    double grade = 0;

                    if (BigPanel2.C1Grade.Text != "None")
                    {
                        grade = Convert.ToDouble(BigPanel2.C1Grade.Text.TrimEnd('%')) / 100.0;
                    }

                    Choice choice = new Choice
                    {
                        Text = BigPanel2.C1Text.Text,
                        Grade = grade
                    };

                    question.Choices[0] = choice;
                }

                if (!string.IsNullOrWhiteSpace(BigPanel2.C2Text.Text))
                {
                    double grade = 0;
                    if (BigPanel2.C2Grade.Text != "None")
                    {
                        grade = Convert.ToDouble(BigPanel2.C2Grade.Text.TrimEnd('%')) / 100.0;
                    }
                    Choice choice = new Choice
                    {
                        Text = BigPanel2.C2Text.Text,
                        Grade = grade
                    };
                    question.Choices[1] = choice;
                }

                if (!string.IsNullOrWhiteSpace(BigPanel2.C3Text.Text))
                {
                    double grade = 0;
                    if (BigPanel2.C3Grade.Text != "None")
                    {
                        grade = Convert.ToDouble(BigPanel2.C3Grade.Text.TrimEnd('%')) / 100.0;
                    }
                    Choice choice = new Choice
                    {
                        Text = BigPanel2.C3Text.Text,
                        Grade = grade
                    };
                    question.Choices[2] = choice;
                }

                if (!string.IsNullOrWhiteSpace(BigPanel2.C4Text.Text))
                {
                    double grade = 0;
                    if (BigPanel2.C4Grade.Text != "None")
                    {
                        grade = Convert.ToDouble(BigPanel2.C4Grade.Text.TrimEnd('%')) / 100.0;
                    }
                    Choice choice = new Choice
                    {
                        Text = BigPanel2.C4Text.Text,
                        Grade = grade
                    };
                    question.Choices[3] = choice;
                }

                if (!string.IsNullOrWhiteSpace(BigPanel2.C5Text.Text))
                {
                    double grade = 0;
                    if (BigPanel2.C5Grade.Text != "None")
                    {
                        grade = Convert.ToDouble(BigPanel2.C5Grade.Text.TrimEnd('%')) / 100.0;
                    }
                    Choice choice = new Choice
                    {
                        Text = BigPanel2.C5Text.Text,
                        Grade = grade
                    };
                    question.Choices[4] = choice;
                }

                CurrentQuestion = question;
            }
            else
            {
                CurrentQuestion.Name = BigPanel2.QName.Text;
                CurrentQuestion.Description = BigPanel2.QText.Rtf;
                CurrentQuestion.Mark = mark;

                if (CurrentCategory != CurrentQuestion.Category)
                {
                    CurrentQuestion.Category.QuestionList.Remove(CurrentQuestion);
                    CurrentQuestion.Category = CurrentCategory;
                    CurrentCategory.AddQuestion(CurrentQuestion);
                    BP1_Category_ItemsChanged();
                }

                if (!string.IsNullOrWhiteSpace(BigPanel2.C1Text.Text))
                {
                    double grade = 0;

                    if (BigPanel2.C1Grade.Text != "None")
                    {
                        grade = Convert.ToDouble(BigPanel2.C1Grade.Text.TrimEnd('%')) / 100.0;
                    }

                    if (CurrentQuestion.Choices[0] != null)
                    {
                        CurrentQuestion.Choices[0].Text = BigPanel2.C1Text.Text;
                        CurrentQuestion.Choices[0].Grade = grade;
                    }
                    else
                    {
                        Choice choice = new Choice
                        {
                            Text = BigPanel2.C1Text.Text,
                            Grade = grade
                        };
                        CurrentQuestion.Choices[0] = choice;
                    }

                }
                else
                {
                    if (CurrentQuestion.Choices[0] != null)
                    {
                        CurrentQuestion.Choices[0] = null;
                    }
                }

                if (!string.IsNullOrWhiteSpace(BigPanel2.C2Text.Text))
                {
                    double grade = 0;

                    if (BigPanel2.C2Grade.Text != "None")
                    {
                        grade = Convert.ToDouble(BigPanel2.C2Grade.Text.TrimEnd('%')) / 100.0;
                    }

                    if (CurrentQuestion.Choices[1] != null)
                    {
                        CurrentQuestion.Choices[1].Text = BigPanel2.C2Text.Text;
                        CurrentQuestion.Choices[1].Grade = grade;
                    }
                    else
                    {
                        Choice choice = new Choice
                        {
                            Text = BigPanel2.C2Text.Text,
                            Grade = grade
                        };
                        CurrentQuestion.Choices[1] = choice;
                    }

                }
                else
                {
                    if (CurrentQuestion.Choices[1] != null)
                    {
                        CurrentQuestion.Choices[1] = null;
                    }
                }

                if (!string.IsNullOrWhiteSpace(BigPanel2.C3Text.Text))
                {
                    double grade = 0;

                    if (BigPanel2.C3Grade.Text != "None")
                    {
                        grade = Convert.ToDouble(BigPanel2.C3Grade.Text.TrimEnd('%')) / 100.0;
                    }

                    if (CurrentQuestion.Choices[2] != null)
                    {
                        CurrentQuestion.Choices[2].Text = BigPanel2.C3Text.Text;
                        CurrentQuestion.Choices[2].Grade = grade;
                    }
                    else
                    {
                        Choice choice = new Choice
                        {
                            Text = BigPanel2.C3Text.Text,
                            Grade = grade
                        };
                        CurrentQuestion.Choices[2] = choice;
                    }

                }
                else
                {
                    if (CurrentQuestion.Choices[2] != null)
                    {
                        CurrentQuestion.Choices[2] = null;
                    }
                }

                if (!string.IsNullOrWhiteSpace(BigPanel2.C4Text.Text))
                {
                    double grade = 0;

                    if (BigPanel2.C4Grade.Text != "None")
                    {
                        grade = Convert.ToDouble(BigPanel2.C4Grade.Text.TrimEnd('%')) / 100.0;
                    }

                    if (CurrentQuestion.Choices[3] != null)
                    {
                        CurrentQuestion.Choices[3].Text = BigPanel2.C4Text.Text;
                        CurrentQuestion.Choices[3].Grade = grade;
                    }
                    else
                    {
                        Choice choice = new Choice
                        {
                            Text = BigPanel2.C4Text.Text,
                            Grade = grade
                        };
                        CurrentQuestion.Choices[3] = choice;
                    }

                }
                else
                {
                    if (CurrentQuestion.Choices[3] != null)
                    {
                        CurrentQuestion.Choices[3] = null;
                    }
                }

                if (!string.IsNullOrWhiteSpace(BigPanel2.C5Text.Text))
                {
                    double grade = 0;

                    if (BigPanel2.C5Grade.Text != "None")
                    {
                        grade = Convert.ToDouble(BigPanel2.C5Grade.Text.TrimEnd('%')) / 100.0;
                    }

                    if (CurrentQuestion.Choices[4] != null)
                    {
                        CurrentQuestion.Choices[4].Text = BigPanel2.C5Text.Text;
                        CurrentQuestion.Choices[4].Grade = grade;
                    }
                    else
                    {
                        Choice choice = new Choice
                        {
                            Text = BigPanel2.C5Text.Text,
                            Grade = grade
                        };
                        CurrentQuestion.Choices[4] = choice;
                    }

                }
                else
                {
                    if (CurrentQuestion.Choices[4] != null)
                    {
                        CurrentQuestion.Choices[4] = null;
                    }
                }
            }
            MessageBox.Show("Saved!");
        }

        // cancel button in edit question form
        private void BP2_CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to discard the changes and quit to Question Bank?", "Cancel", MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
            {
                return;
            }

            BigPanel2.QName.ResetText();
            BigPanel2.QText.ResetText();
            BigPanel2.QMark.Text = "1";
            BigPanel2.C1Text.ResetText();
            BigPanel2.C2Text.ResetText();
            BigPanel2.C3Text.ResetText();
            BigPanel2.C4Text.ResetText();
            BigPanel2.C5Text.ResetText();
            BigPanel2.C1Grade.Text = "None";
            BigPanel2.C2Grade.Text = "None";
            BigPanel2.C3Grade.Text = "None";
            BigPanel2.C4Grade.Text = "None";
            BigPanel2.C5Grade.Text = "None";
            BigPanel2.Hide();
            slash2.Hide();
            direction3.Hide();
            direction2.Click -= new EventHandler(MP_SecondDirection_Click);
            direction2.ForeColor = Color.Black;
            direction2.Cursor = Cursors.Default;
            mainpanel.Controls.RemoveAt(0);
            mainpanel.Controls.Add(BigPanel1);
            mainpanel.Tag = BigPanel1;
            BigPanel1.Show();
        }

        //
/* Quiz Create Form */
/* BigPanel3 */

        // create quiz button event
        private void BP3_CreateButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BigPanel3.QuizName.Text))
            {
                MessageBox.Show("Please enter a valid name for the quiz");
                return;
            }

            DateTime open = DateTime.MinValue;
            DateTime close = DateTime.MaxValue;
            int coeff = 0;
            string unit = null;

            if (BigPanel3.CBTimeO.Checked)
            {
                open = BigPanel3.DTOpen.Value;
            }

            if (BigPanel3.CBTimeC.Checked)
            {
                close = BigPanel3.DTClose.Value;
            }

            if (BigPanel3.CBTimeL.Checked)
            {
                try
                {
                    coeff = Convert.ToInt32(BigPanel3.TimeCoeff.Text);
                }

                catch (FormatException)
                {
                    MessageBox.Show("Please input a valid value for Time Limit!");
                    return;
                }
                unit = BigPanel3.TimeUnit.Text;
            }

            Timing timing = new Timing
            {
                TimeOpen = open,
                TimeClose = close,
                TimeCoefficient = coeff,
                TimeUnit = unit
            };

            Quiz newquiz = new Quiz
            {
                Name = BigPanel3.QuizName.Text,
                Description = BigPanel3.QuizDescription.Text,
                QuestionList = new List<Question>(),
                TotalMark = 0,
                MaxGrade = 10.0,
                PendingList = new List<Question>(),
                Previews = new List<PreviewQuiz>(),
                Time = timing
            };

            Quizzes.Add(newquiz);

            BigPanel3.QuizName.ResetText();
            BigPanel3.QuizDescription.ResetText();
            BigPanel3.DTOpen.Value = DateTime.Now;
            BigPanel3.DTClose.Value = DateTime.Now;
            BigPanel3.TimeCoeff.Text = "0";
            BigPanel3.TimeUnit.SelectedIndex = 1;
            BigPanel3.CBTimeO.Checked = false;
            BigPanel3.CBTimeC.Checked = false;
            BigPanel3.CBTimeL.Checked = false;
            BigPanel3.MainPanel.AutoScrollPosition = new Point(0, 0);
            BigPanel3.Hide();
            //
            heading.Size = new Size(1114, 117);
            functionbutton1.Show();
            button1.Show();
            slash1.Hide();
            direction2.Hide();
            direction1.ForeColor = SystemColors.ControlText;
            direction1.Cursor = Cursors.Default;
            direction1.Click -= new EventHandler(MP_FirstDirection_Click);
            //
            mainpanel.Controls.RemoveAt(0);
            MP_UpdateQuizDisplay();
        }

        // cancel quiz button event
        private void BP3_CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to discard the changes and quit to Home?", "Cancel", MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
            {
                return;
            }

            BigPanel3.QuizName.ResetText();
            BigPanel3.QuizDescription.ResetText();
            BigPanel3.DTOpen.Value = DateTime.Now;
            BigPanel3.DTClose.Value = DateTime.Now;
            BigPanel3.TimeCoeff.Text = "0";
            BigPanel3.TimeUnit.SelectedIndex = 1;
            BigPanel3.CBTimeO.Checked = false;
            BigPanel3.CBTimeC.Checked = false;
            BigPanel3.CBTimeL.Checked = false;
            BigPanel3.MainPanel.AutoScrollPosition = new Point(0, 0);
            BigPanel3.Hide();
            //
            heading.Size = new Size(1114, 117);
            functionbutton1.Show();
            button1.Show();
            slash1.Hide();
            direction2.Hide();
            direction1.ForeColor = SystemColors.ControlText;
            direction1.Cursor = Cursors.Default;
            direction1.Click -= new EventHandler(MP_FirstDirection_Click);
            //
            mainpanel.Controls.RemoveAt(0);
            MP_UpdateQuizDisplay();
        }

        //
/* Quiz Attempt Form */
/* BigPanel4 */

        private void BP4_GearIconLabel_Click(object _1, EventArgs _2, Quiz quiz)
        {
            if (BigPanel4.Visible == false)
            {
                return;
            }

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (BigPanel5 == null)
            {
                OpenQuizEditPanel(new QuizEdit_Form());
            }
            else
            {
                mainpanel.Controls.Add(BigPanel5);
                mainpanel.Tag = BigPanel5;
                BigPanel5.Show();
            }

            BigPanel5.QuestionNameLabel.Text = quiz.Name;
            BigPanel5.QuestionNumberLabel.Text = quiz.QuestionList.Count.ToString();
            BigPanel5.TotalMarkLabel.Text = quiz.TotalMark.ToString();

            BigPanel4.Hide();
            slash1.Show();
            slash2.Show();
            direction1.Click -= new EventHandler(MP_FirstDirection_Click);
            direction1.Click += new EventHandler(MP_FirstDirectionAlter_Click);
            direction3.Text = "Edit quiz";
            direction3.Show();
            direction2.Cursor = Cursors.Hand;
            direction2.ForeColor = Color.IndianRed;
            direction2.Click += new EventHandler(MP_SecondDirectionAlter_Click);
        }

        private void BP4_Popup_StartQuizAttempt_Click(object sender, EventArgs e)
        {
            if (CurrentQuiz.QuestionList.Count == 0)
            {
                MessageBox.Show("Add some questions to this quiz first!");
                BigPanel4.PopupPanel.Hide();
                return;
            }

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (BigPanel6 == null)
            {
                OpenQuizPlayPanel(new QuizPlay_Form());
            }
            else
            {
                mainpanel.Controls.Add(BigPanel6);
                mainpanel.Tag = BigPanel6;
                BigPanel6.Show();
            }

            BigPanel4.PopupPanel.Hide();
            BigPanel6.AttemptInformationPanel.Hide();
            BigPanel6.Timer.Show();
            BP6_InitiateQuizPlayPanel();
        }

        private void BP4_Popup_ContinueQuizAttempt_Click(object sender, EventArgs e)
        {
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (BigPanel6 == null)
            {
                OpenQuizPlayPanel(new QuizPlay_Form());
            }
            else
            {
                mainpanel.Controls.Add(BigPanel6);
                mainpanel.Tag = BigPanel6;
                BigPanel6.Show();
            }

        }


        //
/* Quiz Play Form */
/* BigPanel6 */

        // initiate function
        private void BP6_InitiateQuizPlayPanel()
        {
            if (CurrentQuiz.Time.TimeLimit == 0)
            {
                BigPanel6.Timer.Text = "Time Left: None";
            }
            else
            {
                BigPanel6.StartTimer(CurrentQuiz.Time.TimeLimit);
            }

            int quizSize = CurrentQuiz.QuestionList.Count();
            CurrentQuiz.OngoingPreview = new PreviewQuiz
            {
                QAnswerList = new List<QAnswer>(),
                qLayouts = new List<QuestionDisplay>(),
                StartTime = DateTime.Now
            };

            for (int i = 0; i < quizSize; i++)
            {
                CurrentQuiz.OngoingPreview.qLayouts.Add(null);
            }

            if (CurrentQuiz.Shuffle)
            {
                ShuffleTool.Shuffle(quizSize);
            }
            else
            {
                ShuffleTool.NonShuffle(quizSize);
            }
            int index = 1;
            for (int i = 0; i < quizSize; i++)
            {
                QAnswer qAnswer = new QAnswer
                {
                    Question = CurrentQuiz.QuestionList[ShuffleTool.shuffleIndex[i]],
                    Answer = null,
                    Flag = false
                };
                CurrentQuiz.OngoingPreview.QAnswerList.Add(qAnswer);
                ButtonPlus temp = new ButtonPlus
                {
                    QAnswerAttached = qAnswer,
                    Text = index.ToString(),
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink
                };
                index++;
                temp.Click += new EventHandler(BP6_QuestionIndex_Click);
                temp.GotFocus += new EventHandler(BP6_QuestionIndex_GotFocus);
                temp.LostFocus += new EventHandler(BP6_QuestionIndex_LostFocus);
                BigPanel6.QuestionIndexFlowLayout.Controls.Add(temp);
            }

            BP6_ShowQuestion(0);
        }

        // question index click event
        private void BP6_QuestionIndex_Click(object sender, EventArgs e)
        {
            ButtonPlus button = sender as ButtonPlus;
            int index = Convert.ToInt32(button.Text) - 1;
            if (index == Convert.ToInt32(BigPanel6.QuestionIndexLabel.Text) - 1)
            {
                return;
            }
            BP6_ShowQuestion(index);
        }

        // checkbox checkchanged event for question
        private void BP6_QuestionCheckBox_CheckChanged(object sender, EventArgs e)
        {
            CheckBoxPlus temp = (CheckBoxPlus)sender;
            temp.CheckedChanged -= new EventHandler(BP6_QuestionCheckBox_CheckChanged);
            if (temp.Checked)
            {
                int index = Convert.ToInt32(BigPanel6.QuestionIndexLabel.Text) - 1;
                CurrentQuiz.OngoingPreview.QAnswerList[index].Answer = temp.ChoiceAttached;
                QuestionDisplay temp1 = (QuestionDisplay)((Panel)((Panel)temp.Parent).Parent).Parent;
                for (int i = 0; i < 5; i++)
                {
                    CheckBoxPlus temp2 = temp1.choiceCheckBox[i];
                    if (temp2 == null) break;
                    if (temp2 == temp) continue;
                    temp2.CheckedChanged -= new EventHandler(BP6_QuestionCheckBox_CheckChanged);
                    temp2.Checked = false;
                    temp2.CheckedChanged += new EventHandler(BP6_QuestionCheckBox_CheckChanged);
                }
            }
            else
            {
                temp.Checked = true;
            }
            temp.CheckedChanged += new EventHandler(BP6_QuestionCheckBox_CheckChanged);
        }

        // gotfocus event for questionindexbutton
        private void BP6_QuestionIndex_GotFocus(object sender, EventArgs e)
        {
            ButtonPlus temp = (ButtonPlus)sender;
            temp.BackColor = SystemColors.ControlDark;
        }

        // lostfocus event for questionindexbutton
        private void BP6_QuestionIndex_LostFocus(object sender, EventArgs e)
        {
            ButtonPlus temp = (ButtonPlus)sender;
            temp.BackColor = SystemColors.Window;
        }

        // method to show question
        private void BP6_ShowQuestion(int index)
        {
            QAnswer temp = CurrentQuiz.OngoingPreview.QAnswerList[index];
            if (CurrentQuiz.OngoingPreview.qLayouts[index] == null)
            {
                QuestionDisplay temp1 = new QuestionDisplay(temp.Question);
                for (int i = 0; i < 5; i++)
                {
                    CheckBoxPlus temp2 = temp1.choiceCheckBox[i];
                    if (temp2 == null) break;
                    temp2.CheckedChanged += new EventHandler(BP6_QuestionCheckBox_CheckChanged);
                }
                CurrentQuiz.OngoingPreview.qLayouts[index] = temp1;
            }

            if (index == 0)
            {
                BigPanel6.Back.Hide();
            }
            else
            {
                BigPanel6.Back.Show();
            }
            if (index == CurrentQuiz.QuestionList.Count - 1)
            {
                BigPanel6.Next.Hide();
            }
            else
            {
                BigPanel6.Next.Show();
            }
            BigPanel6.QuestionIndexLabel.Text = $"{index + 1}";
            BigPanel6.MarkLabel.Text = temp.Question.Mark.ToString();
            if (temp.Answer != null)
            {
                BigPanel6.AnswerStateLabel.Text = "Answered";
            }
            else
            {
                BigPanel6.AnswerStateLabel.Text = "Not yet answered";
            }
            if (temp.Flag)
            {
                BigPanel6.FlagLabel.Image = Properties.Resources.flagicon1;
            }
            else
            {
                BigPanel6.FlagLabel.Image = Properties.Resources.flagicon;
            }
            try
            {
                BigPanel6.QuestionPanel.Controls.RemoveAt(2);
            }
            catch { }
            BigPanel6.QuestionPanel.Controls.Add(CurrentQuiz.OngoingPreview.qLayouts[index]);
            BigPanel6.QuestionPanel.Controls.SetChildIndex(CurrentQuiz.OngoingPreview.qLayouts[index], 2);
            BigPanel6.QuestionIndexFlowLayout.Controls[index].Focus();
        }

        // flag click event
        private void BP6_FlagLabel_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(BigPanel6.QuestionIndexLabel.Text) - 1;
            QAnswer temp = CurrentQuiz.OngoingPreview.QAnswerList[index];
            if (temp.Flag)
            {
                temp.Flag = false;
                BigPanel6.FlagLabel.Image = Properties.Resources.flagicon;
            }
            else
            {
                temp.Flag = true;
                BigPanel6.FlagLabel.Image = Properties.Resources.flagicon1;
            }
        }

        // back click event
        private void BP6_BackLabel_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(BigPanel6.QuestionIndexLabel.Text) - 2;
            if(index < 0)
            {
                return;
            }
            BP6_ShowQuestion(index);
        }

        // next click event
        private void BP6_NextLabel_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(BigPanel6.QuestionIndexLabel.Text);
            if (index >= CurrentQuiz.QuestionList.Count)
            {
                return;
            }
            BP6_ShowQuestion(index);
        }

        // finish attempt label click event
        private void BP6_FinishAttemptLabel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to finish this attempt and review it?", "Finish Attempt", MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
            {
                return;
            }
            BigPanel6.StopTimer();

            // get this test information value
            CurrentQuiz.OngoingPreview.EndTime = DateTime.Now;
            double mark = 0;
            foreach (QAnswer temp in CurrentQuiz.OngoingPreview.QAnswerList)
            {
                if (temp.Answer != null)
                {
                    mark += temp.Answer.Grade * temp.Question.Mark;
                }
            }
            CurrentQuiz.OngoingPreview.Mark = mark;

            // set value to review information
            BigPanel6.AttemptInformation_StartedOn.Text = CurrentQuiz.OngoingPreview.StartTime.ToString("dddd, d MMMM yyyy, h:mm tt");
            BigPanel6.AttemptInformation_CompletedOn.Text = CurrentQuiz.OngoingPreview.EndTime.ToString("dddd, d MMMM yyyy, h:mm tt");
            BigPanel6.AttemptInformation_TimeTaken.Text = string.Format("{0:%m} min {0:%s} sec", CurrentQuiz.OngoingPreview.GetRemainingTime());
            BigPanel6.AttemptInformation_Marks.Text = $"{CurrentQuiz.OngoingPreview.Mark}/{CurrentQuiz.TotalMark}";
            BigPanel6.AttemptInformation_Grade.Text = $"{CurrentQuiz.OngoingPreview.Mark / CurrentQuiz.TotalMark}/{CurrentQuiz.MaxGrade}";

            // change others controls of bigpanel6 
            BigPanel6.FinishAttempt.Click -= new EventHandler(BP6_FinishAttemptLabel_Click);
            BigPanel6.FinishAttempt.Text = "Finish Review...";
            BigPanel6.Timer.Hide();
            BigPanel6.AttemptInformationPanel.Show();
            BigPanel6.CorrectAnswerTextBox.Show();
            foreach (ButtonPlus temp in BigPanel6.QuestionIndexFlowLayout.Controls)
            {
                temp.Click -= new EventHandler(BP6_QuestionIndex_Click);
                temp.Click += new EventHandler(BP6_QuestionIndexReview_Click);
            }
            BigPanel6.Back.Click -= new EventHandler(BP6_BackLabel_Click);
            BigPanel6.Back.Click += new EventHandler(BP6_BackLabelReview_Click);
            BigPanel6.Next.Click -= new EventHandler(BP6_NextLabel_Click);
            BigPanel6.Next.Click += new EventHandler(BP6_NextLabelReview_Click);
            BigPanel6.FlagLabel.Click -= new EventHandler(BP6_FlagLabel_Click);
            BP6_ShowQuestionReview(0);
            BigPanel6.FinishAttempt.Click += new EventHandler(BP6_FinishReviewLabel_Click);
        }

        // question index click event for review mode
        private void BP6_QuestionIndexReview_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(((ButtonPlus)sender).Text);
            if (index == Convert.ToInt32(BigPanel6.QuestionIndexLabel.Text))
            {
                return;
            }
            BP6_ShowQuestionReview(index - 1);
        }

        // back click event for review mode
        private void BP6_BackLabelReview_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(BigPanel6.QuestionIndexLabel.Text) - 2;
            if (index < 0)
            {
                return;
            }
            BP6_ShowQuestionReview(index);
        }

        // next click event for review mode
        private void BP6_NextLabelReview_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(BigPanel6.QuestionIndexLabel.Text);
            if (index >= CurrentQuiz.QuestionList.Count)
            {
                return;
            }
            BP6_ShowQuestionReview(index);
        }

        // show question index in the review mode
        private void BP6_ShowQuestionReview(int index)
        {
            QAnswer temp = CurrentQuiz.OngoingPreview.QAnswerList[index];
            if (CurrentQuiz.OngoingPreview.qLayouts[index] == null)
            {
                QuestionDisplay temp1 = new QuestionDisplay(temp.Question);
                for (int i = 0; i < 5; i++)
                {
                    CheckBoxPlus temp2 = temp1.choiceCheckBox[i];
                    if (temp2 == null) break;
                    temp2.Enabled = false;
                }
                CurrentQuiz.OngoingPreview.qLayouts[index] = temp1;
            }
            else
            {
                foreach (CheckBoxPlus temp1 in CurrentQuiz.OngoingPreview.qLayouts[index].choiceCheckBox)
                {
                    if (temp1 == null) break;
                    temp1.Enabled = false;
                }
            }

            if (index == 0)
            {
                BigPanel6.Back.Hide();
            }
            else
            {
                BigPanel6.Back.Show();
            }
            if (index == CurrentQuiz.QuestionList.Count - 1)
            {
                BigPanel6.Next.Hide();
            }
            else
            {
                BigPanel6.Next.Show();
            }
            BigPanel6.QuestionIndexLabel.Text = $"{index + 1}";
            BigPanel6.MarkLabel.Text = temp.Question.Mark.ToString();
            if (temp.Answer != null)
            {
                BigPanel6.AnswerStateLabel.Text = "Answered";
            }
            else
            {
                BigPanel6.AnswerStateLabel.Text = "Not yet answered";
            }
            if (temp.Flag)
            {
                BigPanel6.FlagLabel.Image = Properties.Resources.flagicon1;
            }
            else
            {
                BigPanel6.FlagLabel.Image = Properties.Resources.flagicon;
            }

            TextBox correctAnswer = BigPanel6.CorrectAnswerTextBox;
            correctAnswer.Text = "The correct answer is: ";
            int counter = 0;
            foreach (Choice choice in temp.Question.Choices)
            {
                if (choice == null)
                {
                    continue;
                }
                if (choice.Grade > 0)
                {
                    if (counter > 0)
                    {
                        correctAnswer.Text += ", ";
                    }
                    correctAnswer.Text += choice.Text;
                }
            }
            correctAnswer.Text += ".";
            try
            {
                BigPanel6.QuestionPanel.Controls.RemoveAt(2);
            }
            catch { }
            BigPanel6.QuestionPanel.Controls.Add(CurrentQuiz.OngoingPreview.qLayouts[index]);
            BigPanel6.QuestionPanel.Controls.SetChildIndex(CurrentQuiz.OngoingPreview.qLayouts[index], 2);
            BigPanel6.QuestionIndexFlowLayout.Controls[index].Focus();
        }

        // finish review click event
        private void BP6_FinishReviewLabel_Click(object sender, EventArgs e)
        {
            BigPanel6.FinishAttempt.Click -= new EventHandler(BP6_FinishReviewLabel_Click);
            BigPanel6.FinishAttempt.Text = "Finish Attempt...";
            BigPanel6.CorrectAnswerTextBox.Hide();
            CurrentQuiz.AddPreview(CurrentQuiz.OngoingPreview);
            CurrentQuiz.OngoingPreview = null;
            BigPanel6.QuestionIndexFlowLayout.Controls.Clear();
            BigPanel6.Back.Click -= new EventHandler(BP6_BackLabelReview_Click);
            BigPanel6.Back.Click += new EventHandler(BP6_BackLabel_Click);
            BigPanel6.Next.Click -= new EventHandler(BP6_NextLabelReview_Click);
            BigPanel6.Next.Click += new EventHandler(BP6_NextLabel_Click);
            BigPanel6.FlagLabel.Click += new EventHandler(BP6_FlagLabel_Click);
            BigPanel6.FinishAttempt.Click += new EventHandler(BP6_FinishAttemptLabel_Click);

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }
            BigPanel6.Hide();
            mainpanel.Controls.Add(BigPanel4);
            mainpanel.Tag = BigPanel6;
            BigPanel4.Show();
        }

        //
/* Quiz Edit Form */
/* BigPanel5 */

        // add question to quiz 2 ("from question bank")
        private void BP5_Popup_AddQuestionFromQuestionBank_Click(object _1, EventArgs _2)
        {
            if (BigPanel5.Visible == false)
            {
                return;
            }
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (BigPanel5_2 == null)
            {
                OpenAddQuestionToQuiz2Panel(new AddQuestionToQuiz2_Form());
            }
            else
            {
                mainpanel.Controls.Add(BigPanel5_2);
                mainpanel.Tag = BigPanel5_2;
                BigPanel5_2.Show();
            }
            try
            {
                BigPanel5_2.CategoryComboBox.SelectedIndexChanged -= new EventHandler(BP5_2_CategoryComboBox_SelectedIndexChanged);
            }
            catch
            {

            }
            BigPanel5_2.CategoryComboBox.DataSource = null;
            BigPanel5_2.CategoryComboBox.DataSource = Categories;
            BigPanel5_2.CategoryComboBox.DisplayMember = "NameAndGen";
            BigPanel5_2.CategoryComboBox.SelectedItem = null;
            BigPanel5_2.QuestionDisplay.Controls.Clear();
            BigPanel5_2.CategoryComboBox.SelectedIndexChanged += new EventHandler(BP5_2_CategoryComboBox_SelectedIndexChanged);

            BigPanel5.Hide();
            BigPanel5.Popup.Hide();
            slash3.Show();
            direction4.Show();
            direction3.Cursor = Cursors.Hand;
            direction3.ForeColor = Color.IndianRed;
            direction3.Click += new EventHandler(MP_ThirdDirection_Click);
        }

        // save button event on edit form
        private void BP5_SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentQuiz.MaxGrade = Convert.ToDouble(BigPanel5.GradeTextBox.Text);
            }
            catch
            {
                CurrentQuiz.MaxGrade = 10.0;
            }

            CurrentQuiz.TotalMark = Convert.ToDouble(BigPanel5.TotalMarkLabel.Text);
            if (BigPanel5.ShuffleBox.Checked)
            {
                CurrentQuiz.Shuffle = true;
            }

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            slash2.Hide();
            direction3.Hide();
            direction2.Cursor = Cursors.Default;
            direction2.ForeColor = Color.Black;
            direction2.Click -= new EventHandler(MP_SecondDirectionAlter_Click);
            BigPanel5.Hide();
            BigPanel4.Show();
            mainpanel.Controls.Add(BigPanel4);
        }

        // event for indexing page
        private void BP5_IndexingPage()
        {
            BigPanel5.PageFlowLayout.Controls.Clear();
            int pageNum = (CurrentQuiz.QuestionList.Count() - 1) / 10 + 1;
            for (int i = 1; i <= pageNum; i++)
            {
                Button temp = new Button
                {
                    Text = i.ToString(),
                    AutoSize = true
                };
                temp.Click += new EventHandler(BP5_PageIndexButton_Click);
                BigPanel5.PageFlowLayout.Controls.Add(temp);
            }
        }

        // click event for page button
        private void BP5_PageIndexButton_Click(object sender, EventArgs e)
        {
            Button temp = (Button)sender;
            BP5_ChangePage(Convert.ToInt32(temp.Text));
        }

        // event for changing page
        private void BP5_ChangePage(int page)
        {
            BigPanel5.PageNumberLabel.Text = $"Page {page}";
            int quizQuestionLen = CurrentQuiz.QuestionList.Count();
            int start = (page - 1) * 10, end = page * 10 - 1;

            if ((quizQuestionLen - 1) < end)
            {
                end = quizQuestionLen - 1;
            }

            BigPanel5.QuestionLayout.Controls.Clear();
            for (int i = end; i >= start; i--)
            {
                QuizQuestionBlockBP5 temp = new QuizQuestionBlockBP5(CurrentQuiz.QuestionList[i], i + 1);
                temp.Edit.Click += new EventHandler(BP5_QuestionTrashIcon_Click);
                BigPanel5.QuestionLayout.Controls.Add(temp);
            }
        }

        // click event for trash icon in question layout
        private void BP5_QuestionTrashIcon_Click(object sender, EventArgs e)
        {
            QuizQuestionBlockBP5 temp = (QuizQuestionBlockBP5)((Label)sender).Parent;
            Panel container = temp.Parent as Panel;
            int index = temp.Index;
            foreach (QuizQuestionBlockBP5 temp1 in container.Controls)
            {
                if (temp1.Index > index)
                {
                    temp1.SetIndex(temp1.Index - 1);
                }
            }
            BigPanel5.QuestionLayout.Controls.Remove(temp);
            CurrentQuiz.QuestionList.RemoveAt(index - 1);
            if (CurrentQuiz.QuestionList.Count() % 10 == 0)
            {
                BigPanel5.PageFlowLayout.Controls.RemoveAt(CurrentQuiz.QuestionList.Count() / 10);
            }
            int lastIndex = ((index - 1) / 10 + 1) * 10;
            if (lastIndex <= CurrentQuiz.QuestionList.Count())
            {
                QuizQuestionBlockBP5 temp2 = new QuizQuestionBlockBP5(CurrentQuiz.QuestionList[lastIndex - 1], lastIndex);
                temp2.Edit.Click += new EventHandler(BP5_QuestionTrashIcon_Click);
                BigPanel5.QuestionLayout.Controls.Add(temp2);
                BigPanel5.QuestionLayout.Controls.SetChildIndex(temp2, 0);
            }
        }

        //
/* Add Question To Quiz 2 Form*/
/* BigPanel5_2*/

        // click event for "add selected question to quiz"
        private void BP5_2_AddSelectedToQuizButton_Click(object sender, EventArgs e)
        {
            double totalMark = 0;
            if (BigPanel5_2.AddAll.Checked)
            {
                foreach (Question question in CurrentCategory.QuestionList)
                {
                    totalMark += question.Mark;
                    CurrentQuiz.AddQuestion(question);
                }

                if (BigPanel5_2.SubcategoryCheckBox.Checked)
                {
                    foreach (Category subcategory in CurrentCategory.Child)
                    {
                        foreach (Question question in subcategory.QuestionList)
                        {
                            totalMark += question.Mark;
                            CurrentQuiz.AddQuestion(question);
                        }
                    }
                    BigPanel5_2.SubcategoryCheckBox.Checked = false;
                }
                BigPanel5_2.AddAll.Checked = false;
            }
            else
            {
                foreach (Question question in CurrentQuiz.PendingList)
                {
                    totalMark += question.Mark;
                    CurrentQuiz.AddQuestion(question);
                }
            }

            CurrentQuiz.PendingList.Clear();
            BigPanel5.QuestionNumberLabel.Text = CurrentQuiz.QuestionList.Count().ToString();
            BigPanel5.TotalMarkLabel.Text = totalMark.ToString();
            BP5_IndexingPage();
            BP5_ChangePage(1);

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            slash3.Hide();
            direction4.Hide();
            direction3.Cursor = Cursors.Default;
            direction3.ForeColor = Color.Black;
            direction3.Click -= new EventHandler(MP_ThirdDirection_Click);
            BigPanel5_2.Hide();
            BigPanel5.Show();
            mainpanel.Controls.Add(BigPanel5);
        }

        // refresh question display in add2
        private void BP5_2_RefreshQuestionDisplay(bool mode)
        {
            if (CurrentCategory != null)
            {
                BigPanel5_2.QuestionDisplay.Controls.Clear();
                BigPanel5_2.AddedQuestionDisplay.Controls.Clear();
                foreach (Question question in CurrentCategory.QuestionList)
                {
                    if (!CurrentQuiz.PendingList.Contains(question))
                    {
                        QuizQuestionBlockBP5_2 temp = new QuizQuestionBlockBP5_2(question, false);
                        BigPanel5_2.QuestionDisplay.Controls.Add(temp);
                        temp.Edit.Click += delegate (object sender1, EventArgs e1) { BP5_2_PlusIconLabel_Click(sender1, e1, temp.Question); };
                    }
                }

                if (mode)
                {
                    foreach (Category subcategory in CurrentCategory.Child)
                    {
                        foreach (Question question in subcategory.QuestionList)
                        {
                            if (!CurrentQuiz.PendingList.Contains(question))
                            {
                                QuizQuestionBlockBP5_2 temp = new QuizQuestionBlockBP5_2(question, false);
                                BigPanel5_2.QuestionDisplay.Controls.Add(temp);
                                temp.Edit.Click += delegate (object sender1, EventArgs e1) { BP5_2_PlusIconLabel_Click(sender1, e1, temp.Question); };
                            }
                        }
                    }
                }

                foreach (Question question in CurrentQuiz.PendingList)
                {
                    QuizQuestionBlockBP5_2 temp = new QuizQuestionBlockBP5_2(question, true);
                    BigPanel5_2.AddedQuestionDisplay.Controls.Add(temp);
                }
            }
        }

        // click event for when clicking plus icon on the left of the question block in add2
        private void BP5_2_PlusIconLabel_Click(object sender, EventArgs _, Question question)
        {
            CurrentQuiz.PendingList.Add(question);
            QuizQuestionBlockBP5_2 temp = (QuizQuestionBlockBP5_2)((Label)sender).Parent;
            temp.Edit.Image = Properties.Resources.icon11;
            temp.Edit.Click -= delegate (object sender1, EventArgs e1) { BP5_2_PlusIconLabel_Click(sender1, e1, temp.Question); };
            temp.Edit.Click += delegate (object sender1, EventArgs e1) { BP5_2_MinusIconLabel_Click(sender1, e1, temp.Question); };
        }

        // click event for when clicking minus icon on the left of the question block in add2
        private void BP5_2_MinusIconLabel_Click(object sender, EventArgs _, Question question)
        {
            CurrentQuiz.PendingList.Remove(question);
            QuizQuestionBlockBP5_2 temp = (QuizQuestionBlockBP5_2)((Label)sender).Parent;
            temp.Edit.Image = Properties.Resources.icon10;
            temp.Edit.Click -= delegate (object sender1, EventArgs e1) { BP5_2_MinusIconLabel_Click(sender1, e1, temp.Question); };
            temp.Edit.Click += delegate (object sender1, EventArgs e1) { BP5_2_PlusIconLabel_Click(sender1, e1, temp.Question); };
        }

        // checkchanged event for "subcategory" checkbox in add2
        private void BP5_2_ShowSubcategoriesCheckBox_CheckChanged(object sender, EventArgs e)
        {
            BP5_2_RefreshQuestionDisplay(BigPanel5_2.SubcategoryCheckBox.Checked);
        }

        // category index change event in add2
        private void BP5_2_CategoryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BigPanel5_2.CategoryComboBox.SelectedItem != null)
            {
                CurrentCategory = BigPanel5_2.CategoryComboBox.SelectedItem as Category;
                BP5_2_RefreshQuestionDisplay(BigPanel5_2.SubcategoryCheckBox.Checked);
            }
        }

        //
/* Headings */
/* MainPanel */

        // direct to home when click the home label on top
        private void MP_FirstDirection_Click(object sender, EventArgs e)
        {
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            BigPanel1?.Hide();

            BigPanel2?.Hide();

            BigPanel3?.Hide();

            heading.Size = new Size(1114, 117);
            functionbutton1.Show();
            button1.Show();
            slash1.Hide();
            slash2.Hide();
            direction2.Hide();
            direction3.Hide();
            direction1.ForeColor = SystemColors.ControlText;
            direction1.Cursor = Cursors.Default;
            direction1.Click -= new EventHandler(MP_FirstDirection_Click);
            mainpanel.Controls.Add(quizFlowLayout);
        }

        // direct to home when click the home label on top (alter)
        private void MP_FirstDirectionAlter_Click(object sender, EventArgs e)
        {
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (BigPanel5 != null && BigPanel5.Visible)
            {
                BigPanel5.Hide();
            }

            if (BigPanel5_2 != null && BigPanel5_2.Visible)
            {
                DialogResult result = MessageBox.Show("Do you want to undo the changes?", "Directing to home page", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return;
                }

                CurrentQuiz.PendingList.Clear();
                BigPanel5.Hide();
            }

            heading.Size = new Size(1114, 117);
            functionbutton1.Show();
            button1.Show();
            slash1.Hide();
            slash2.Hide();
            slash3.Hide();
            direction2.Hide();
            direction3.Hide();
            direction4.Hide();
            direction1.ForeColor = SystemColors.ControlText;
            direction1.Cursor = Cursors.Default;
            direction1.Click -= new EventHandler(MP_FirstDirectionAlter_Click);
            direction2.Click -= new EventHandler(MP_SecondDirectionAlter_Click);
            direction3.Click -= new EventHandler(MP_ThirdDirection_Click);
            mainpanel.Controls.Add(quizFlowLayout);
        }

        // direct to edit form when click the question bank label on top (alter for direct from bigpanel 5* -> 4)
        private void MP_SecondDirection_Click(object sender, EventArgs e)
        {
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }
            //
            BigPanel2.Hide();
            //
            slash1.Hide();
            slash2.Hide();
            direction2.Hide();
            direction3.Hide();
            direction2.Click -= new EventHandler(MP_SecondDirection_Click);
            BP1_UpdateQuestionDisplay(BigPanel1.SubcategoriesQ.Checked);
            //
            mainpanel.Controls.Add(BigPanel1);
            mainpanel.Tag = BigPanel1;
            BigPanel1.Show();
        }

        private void MP_SecondDirectionAlter_Click(object sender, EventArgs e)
        {
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }
            //
            if (BigPanel5 != null && BigPanel5.Visible)
            {
                BigPanel5.Hide();
            }

            if (BigPanel5_2 != null && BigPanel5_2.Visible)
            {
                DialogResult result = MessageBox.Show("Do you want to undo the changes?", "Directing to quiz attempt page", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return;
                }

                CurrentQuiz.PendingList.Clear();
                BigPanel5_2.Hide();
            }
            //
            slash2.Hide();
            slash3.Hide();
            direction2.ForeColor = SystemColors.ControlText;
            direction2.Cursor = Cursors.Default;
            direction2.Click -= new EventHandler(MP_SecondDirectionAlter_Click);
            direction3.Click -= new EventHandler(MP_ThirdDirection_Click);
            direction3.Hide();
            direction4.Hide();
            //

            mainpanel.Controls.Add(BigPanel4);
            mainpanel.Tag = BigPanel4;
            BigPanel4.Show();
        }

        private void MP_ThirdDirection_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to undo the changes?", "Directing to edit quiz page", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                return;
            }

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }
            //
            CurrentQuiz.PendingList.Clear();
            BigPanel5_2.Hide();
            //
            slash3.Hide();
            direction3.ForeColor = SystemColors.ControlText;
            direction3.Cursor = Cursors.Default;
            direction4.Hide();
            direction3.Click -= new EventHandler(MP_ThirdDirection_Click);
            //
            mainpanel.Controls.Add(BigPanel5);
            mainpanel.Tag = BigPanel5;
            BigPanel5.Show();
        }


    }
}
