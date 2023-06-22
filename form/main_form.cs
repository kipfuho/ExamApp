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
        private edit_form bigpanel1;
        private editquestion_form bigpanel2;
        private quiz_form bigpanel3;
        private quiz_attempt_form bigpanel4;
        private quiz_edit_form bigpanel5;
        private addquestiontoquiz2_form bigpanel5_2;
        private quiz_play bigpanel6;
        private Question currentquestion;
        private List<Category> categories;
        private Category currentcategory;
        private List<Quiz> quizs;
        private Quiz currentquiz;
        private string importFilePath;

        public ExamApp()
        {
            InitializeComponent();
            categories = new List<Category>
            {
                new Category(null, new List<Category>(), "Default", "Default category", -1, 0, new List<Question>())
            };
            quizs = new List<Quiz>();
        }

        //
/* form opening */

        // function to open edit form
        private void openEditPanel(edit_form form, int index)
        {
            bigpanel1 = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            // Events
            form.DirectingButton1.Click += new EventHandler(this.createquestion_Click);
            form.SubcategoriesQ.CheckedChanged += new EventHandler(this.BP1checkbox1_CheckedChanged);
            form.addCategoryButton.Click += new EventHandler(this.BP1addcategory_Click);

            form.ImportPanel.DragEnter += this.importPanel_DragEnter;
            form.ImportPanel.DragDrop += this.importPanel_DragDrop;
            form.ImportButton.Click += new EventHandler(this.BP1importbutton_Click);
            //
            // Category ComboBox in Questions tab
            //
            form.QCategory.SelectedIndexChanged += new EventHandler(this.qcategory1_SelectIndexChanged);
            form.QCategory.Items.Clear();
            form.QCategory.DataSource = categories;
            form.QCategory.DisplayMember = "NameAndGen";
            //
            // Category ComboBox in Categories tab
            //
            form.PCategory.SelectedIndexChanged += new EventHandler(this.pcategory1_SelectIndexChanged);
            form.PCategory.Items.Clear();
            form.PCategory.DataSource = categories;
            form.PCategory.DisplayMember = "NameAndGen";
            //
            form.DirectTab.SelectedIndex = index;
            mainpanel.Controls.Add(form);
            mainpanel.Tag = form;
            form.Show();
        }

        // function to open edit question form
        private void openEditQuestionPanel(editquestion_form form, Boolean show)
        {
            bigpanel2 = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            //
            // add events to buttons
            //
            form.FunctionButton1.Click += delegate (object sender, EventArgs e) { this.BP2savebutton1_Click(sender, e, this.currentquestion); };
            form.FunctionButton2.Click += new EventHandler(this.BP2cancelbutton_Click);
            form.FunctionButton3.Click += delegate (object sender, EventArgs e) { this.BP2savebutton2_Click(sender, e, this.currentquestion); };
            //
            // Category ComboBox
            //
            form.QCategory.SelectedIndexChanged += new EventHandler(this.qcategory2_SelectIndexChanged);
            form.QCategory.Items.Clear();
            form.QCategory.DataSource = categories;
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
        private void openQuizPanel(quiz_form form)
        {
            bigpanel3 = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            //
            form.CreateQuizButton.Click += new EventHandler(this.BP3createbutton_Click);
            form.CANCELbutton.Click += new EventHandler(this.BP3cancelbutton_Click);
            //
            mainpanel.Controls.Add(form);
            mainpanel.Tag = form;
            form.Show();
        }

        // function to open quiz attempt form
        private void openQuizAttemptPanel(quiz_attempt_form form)
        {
            bigpanel4 = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            //
            form.GearSetting.Click += delegate (object sender, EventArgs e) { this.gearSettingLabel_Click(sender, e, this.currentquiz); };
            form.StartAttemptButton.Click += new EventHandler(this.startQuizAttempt_Click);
            //
            mainpanel.Controls.Add(form);
            mainpanel.Tag = form;
            form.Show();
        }

        // function to open quiz attempt form
        private void openQuizEditPanel(quiz_edit_form form)
        {
            bigpanel5 = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            //
            bigpanel5.AddFromQuestionBankLabel.Click += delegate (object sender, EventArgs e) { this.addQ2Q2_Click(sender, e, this.currentquiz); };
            bigpanel5.SaveQuizBtn.Click += new EventHandler(this.saveBP5_Click);
            //
            mainpanel.Controls.Add(form);
            mainpanel.Tag = form;
            form.Show();
        }

        // function to open add question to quiz 2 form ("from question bank")
        private void openAddQuestionToQuiz2(addquestiontoquiz2_form form)
        {
            bigpanel5_2 = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            //
            form.SubcategoryCheckBox.CheckedChanged += new EventHandler(this.Add2_Subcategory_CheckedChanged);
            form.AddToQuiz.Click += new EventHandler(this.Add2_AddSelectedToQuiz);
            //
            mainpanel.Controls.Add(form);
            mainpanel.Tag = form;
            form.Show();
        }

        // function to open quiz_play form
        private void openQuizPlay(quiz_play form)
        {
            bigpanel6 = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            //
            //
            mainpanel.Controls.Add(form);
            mainpanel.Tag = form;
            form.Show();
        }
        
        //
/* main form */

        private void updateQuizdisplay()
        {
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            this.quizFlowLayout.Controls.Clear();
            foreach (Quiz quiz in this.quizs)
            {
                QuizBlock temp = new QuizBlock(quiz);
                temp.QName.Click += delegate (object sender1, EventArgs e1) { this.quizName_Click(sender1, e1, temp.Quiz); };
                this.quizFlowLayout.Controls.Add(temp);
            }

            this.mainpanel.Controls.Add(this.quizFlowLayout);
        }

        // "setting" label (gear icon)
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Visible == true)
            {
                if (popup.Visible == false)
                {
                    popup.Location = new Point(this.heading.Right - popup.Width, this.heading.Bottom - 60);
                    popup.Show();
                }
                else
                {
                    popup.Hide();
                }
            }
        }

        // "turn edit on" button
        private void functionbutton1_Click(object sender, EventArgs e)
        {
            button1.Hide();
            functionbutton1.Hide();
            popup.Hide();
            this.heading.Size = new Size(1114, 86);

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if(bigpanel3 == null)
            {
                openQuizPanel(new quiz_form());
            }
            else
            {
                mainpanel.Controls.Add(bigpanel3);
                mainpanel.Tag = bigpanel3;
                bigpanel3.Show();
            }
            direction1.Cursor = Cursors.Hand;
            direction1.ForeColor = Color.IndianRed;
            direction1.Click += new EventHandler(this.direction1_Click);
            slash1.Show();
            direction2.Text = "Adding a new quiz";
            direction2.ForeColor = SystemColors.ControlText;
            direction2.Cursor = Cursors.Default;
            direction2.Show();
        }

        // click event for clicking a quiz name on home page, directing to quiz attempt form
        private void quizName_Click(object sender, EventArgs e, Quiz quiz)
        {
            button1.Hide();
            functionbutton1.Hide();
            popup.Hide();
            this.heading.Size = new Size(1114, 86);

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }
            if (bigpanel4 == null)
            {
                openQuizAttemptPanel(new quiz_attempt_form());
            }
            else
            {
                mainpanel.Controls.Add(bigpanel4);
                mainpanel.Tag = bigpanel4;
                bigpanel4.Show();
            }
            //
            currentquiz = quiz;
            bigpanel4.QuizName.Text = quiz.Name;
            if (quiz.Time.TimeCoefficient == 0)
            {
                bigpanel4.TimeLimitLabel.Text = "None";
            }
            else
            {
                bigpanel4.TimeLimitLabel.Text = $"{quiz.Time.TimeCoefficient} {quiz.Time.TimeUnit}";
            }
            //
            direction1.Cursor = Cursors.Hand;
            direction1.ForeColor = Color.IndianRed;
            direction1.Click += new EventHandler(this.direction1_Click);
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
        

        
        //
/* edit form */

        // function to update questions display in questions tab in edit form
        private void updateQuestiondisplay(bool mode)
        {
            currentcategory = bigpanel1.QCategory.SelectedItem as Category;
            if (currentcategory != null)
            {
                this.bigpanel1.QBox.Controls.Clear();
                if (mode)
                {
                    foreach (Question question in this.currentcategory.QuestionList)
                    {
                        QuestionBlock temp = new QuestionBlock(question);
                        this.bigpanel1.QBox.Controls.Add(temp);
                        temp.Edit.Click += delegate (object sender1, EventArgs e1) { this.BP1editlabel_Click(sender1, e1, temp.Question); };
                        temp.Delete.Click += delegate (object sender1, EventArgs e1) { this.BP1deletelabel_Click(sender1, e1, temp.Question); };
                    }

                    foreach (Category subcategory in currentcategory.Child)
                    {
                        foreach (Question question in subcategory.QuestionList)
                        {
                            QuestionBlock temp = new QuestionBlock(question);
                            this.bigpanel1.QBox.Controls.Add(temp);
                            temp.Edit.Click += delegate (object sender1, EventArgs e1) { this.BP1editlabel_Click(sender1, e1, temp.Question); };
                            temp.Delete.Click += delegate (object sender1, EventArgs e1) { this.BP1deletelabel_Click(sender1, e1, temp.Question); };
                        }
                    }
                }
                else
                {
                    foreach (Question question in currentcategory.QuestionList)
                    {
                        QuestionBlock temp = new QuestionBlock(question);
                        this.bigpanel1.QBox.Controls.Add(temp);
                        temp.Edit.Click += delegate (object sender1, EventArgs e1) { this.BP1editlabel_Click(sender1, e1, temp.Question); };
                        temp.Delete.Click += delegate (object sender1, EventArgs e1) { this.BP1deletelabel_Click(sender1, e1, temp.Question); };
                    }
                }
            }
        }

        // event when category items changed, it will update the category combobox
        private void Category_ItemsChanged()
        {
            if(bigpanel1 != null)
            {
                bigpanel1.QCategory.DataSource = null;
                bigpanel1.QCategory.DataSource = categories;
                bigpanel1.QCategory.DisplayMember = "NameAndGen";
                //
                bigpanel1.PCategory.DataSource = null;
                bigpanel1.PCategory.DataSource = categories;
                bigpanel1.PCategory.DisplayMember = "NameAndGen";
                //
            }
            if (bigpanel2 != null)
            {
                bigpanel2.QCategory.DataSource = null;
                bigpanel2.QCategory.DataSource = categories;
                bigpanel2.QCategory.DisplayMember = "NameAndGen";
            }
        }

        // set currentcategory as the selected items in category combobox in questions tab
        private void qcategory1_SelectIndexChanged(object sender, EventArgs e)
        {
            if(bigpanel1.QCategory.SelectedItem != null)
            {
                updateQuestiondisplay(bigpanel1.SubcategoriesQ.Checked);
            }
        }

        // set currentcategory as the selected items in category combobox in edit question
        private void qcategory2_SelectIndexChanged(object sender, EventArgs e)
        {
            if (bigpanel2.QCategory.SelectedItem != null)
            {
                currentcategory = bigpanel2.QCategory.SelectedItem as Category;
            }
        }

        // set currentcategory as the selected items in category combobox in categories tab
        private void pcategory1_SelectIndexChanged(object sender, EventArgs e)
        {
            if (bigpanel1.PCategory.SelectedItem != null)
            {
                currentcategory = bigpanel1.PCategory.SelectedItem as Category;
            }
        }

        // direct to questions tab in edit form
        private void questions1_Click(object sender, EventArgs e)
        {
            this.popup.Hide();
            this.functionbutton1.Hide();
            this.button1.Hide();
            this.heading.Size = new Size(1114, 86);

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (bigpanel1 == null)
            {
                openEditPanel(new edit_form(), 0);
            }
            else
            {
                mainpanel.Controls.Add(bigpanel1);
                mainpanel.Tag = bigpanel1;
                bigpanel1.DirectTab.SelectedIndex = 0;
                bigpanel1.Show();
            }

            direction1.Cursor = System.Windows.Forms.Cursors.Hand;
            direction1.ForeColor = Color.IndianRed;
            direction1.Click += new EventHandler(this.direction1_Click);
        }

        // direct to categories tab in edit form
        private void categories1_Click(object sender, EventArgs e)
        {
            this.popup.Hide();
            this.functionbutton1.Hide();
            this.button1.Hide();
            this.heading.Size = new System.Drawing.Size(1114, 86);

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (bigpanel1 == null)
            {
                openEditPanel(new edit_form(), 1);
            }
            else
            {
                mainpanel.Controls.Add(bigpanel1);
                mainpanel.Tag = bigpanel1;
                bigpanel1.DirectTab.SelectedIndex = 1;
                bigpanel1.Show();
            }

            direction1.Cursor = System.Windows.Forms.Cursors.Hand;
            direction1.ForeColor = Color.IndianRed;
            direction1.Click += new System.EventHandler(this.direction1_Click);
        }

        // direct to import tab in edit form
        private void import1_Click(object sender, EventArgs e)
        {
            this.popup.Hide();
            this.functionbutton1.Hide();
            this.button1.Hide();
            this.heading.Size = new System.Drawing.Size(1114, 86);

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (bigpanel1 == null)
            {
                openEditPanel(new edit_form(), 2);
            }
            else
            {
                mainpanel.Controls.Add(bigpanel1);
                mainpanel.Tag = bigpanel1;
                bigpanel1.DirectTab.SelectedIndex = 2;
                bigpanel1.Show();
            }

            direction1.Cursor = System.Windows.Forms.Cursors.Hand;
            direction1.ForeColor = Color.IndianRed;
            direction1.Click += new System.EventHandler(this.direction1_Click);
        }

        // direct to edit question form when click the create question button
        private void createquestion_Click(object sender, EventArgs e)
        {
            if (bigpanel1.Visible == false)
            {
                return ;
            }

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (bigpanel2 == null)
            {
                openEditQuestionPanel(new editquestion_form(), true);
            }
            else
            {
                bigpanel2.MainPanel.AutoScrollPosition = new Point(0, 0);
                bigpanel2.SubMainPanel.AutoScrollPosition = new Point(0, 0);
                bigpanel2.ChoiceP.AutoScrollPosition = new Point(0, 0);
                bigpanel2.MoreChoiceP.Hide();
                mainpanel.Controls.Add(bigpanel2);
                mainpanel.Tag = bigpanel2;
                bigpanel2.Show();
            }

            bigpanel2.BigLabel.Text = "Adding a Multiple choices question";
            bigpanel1.Hide();
            slash1.Show();
            slash2.Show();
            direction2.Text = "Question Bank";
            direction2.Show();
            direction3.Show();
            direction2.Cursor = System.Windows.Forms.Cursors.Hand;
            direction2.ForeColor = Color.IndianRed;
            direction2.Click += new System.EventHandler(this.direction2_Click);
        }

        // checkbox to also show questions from subcategories
        private void BP1checkbox1_CheckedChanged(object sender, EventArgs e)
        {
            updateQuestiondisplay(bigpanel1.SubcategoriesQ.Checked);
        }

        // event for edit label beside each question in questions, edit form to edit the selected question
        private void BP1editlabel_Click(object sender, EventArgs e, Question question)
        {
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (bigpanel2 == null)
            {
                this.openEditQuestionPanel(new editquestion_form(), false);
            }

            currentquestion = question;
            bigpanel2.QName.Text = question.Name;
            try
            {
                bigpanel2.QText.Rtf = question.Description;
            }
            catch
            {
                bigpanel2.QText.Text = question.Description;
            }
            bigpanel2.QMark.Text = Convert.ToString(question.Mark);

            if (question.Choices[2] != null || question.Choices[3] != null || question.Choices[4] != null)
            {
                bigpanel2.MoreChoiceP.Show();
            }

            for (int i = 0; i <= 4; i++)
            {
                Choice choice = currentquestion.Choices[i];
                if (choice != null)
                {
                    if (i == 0)
                    {
                        try
                        {
                            bigpanel2.C1Text.Rtf = choice.Text;
                        }
                        catch
                        {
                            bigpanel2.C1Text.Text = choice.Text;
                        }
                        if (choice.Grade != 0)
                        {
                            bigpanel2.C1Grade.Text = Convert.ToString(choice.Grade * 100.0) + "%";
                        }
                        else
                        {
                            bigpanel2.C1Grade.Text = "None";
                        }
                    }
                    else if (i == 1)
                    {
                        try
                        {
                            bigpanel2.C2Text.Rtf = choice.Text;
                        }
                        catch
                        {
                            bigpanel2.C2Text.Text = choice.Text;
                        }
                        if (choice.Grade != 0)
                        {
                            bigpanel2.C2Grade.Text = Convert.ToString(choice.Grade * 100.0) + "%";
                        }
                        else
                        {
                            bigpanel2.C2Grade.Text = "None";
                        }
                    }
                    else if (i == 2)
                    {
                        try
                        {
                            bigpanel2.C3Text.Rtf = choice.Text;
                        }
                        catch
                        {
                            bigpanel2.C3Text.Text = choice.Text;
                        }
                        if (choice.Grade != 0)
                        {
                            bigpanel2.C3Grade.Text = Convert.ToString(choice.Grade * 100.0) + "%";
                        }
                        else
                        {
                            bigpanel2.C3Grade.Text = "None";
                        }
                    }
                    else if (i == 3)
                    {
                        try
                        {
                            bigpanel2.C4Text.Rtf = choice.Text;
                        }
                        catch
                        {
                            bigpanel2.C4Text.Text = choice.Text;
                        }
                        if (choice.Grade != 0)
                        {
                            bigpanel2.C4Grade.Text = Convert.ToString(choice.Grade * 100.0) + "%";
                        }
                        else
                        {
                            bigpanel2.C4Grade.Text = "None";
                        }
                    }
                    else if (i == 4)
                    {
                        try
                        {
                            bigpanel2.C5Text.Rtf = choice.Text;
                        }
                        catch
                        {
                            bigpanel2.C5Text.Text = choice.Text;
                        }
                        if (choice.Grade != 0)
                        {
                            bigpanel2.C5Grade.Text = Convert.ToString(choice.Grade * 100.0) + "%";
                        }
                        else
                        {
                            bigpanel2.C5Grade.Text = "None";
                        }
                    }
                }
            }

            // Change the heading
            slash2.Show();
            direction3.Show();
            direction2.Cursor = System.Windows.Forms.Cursors.Hand;
            direction2.ForeColor = Color.IndianRed;
            direction2.Click += new System.EventHandler(this.direction2_Click);

            // Switch big panel
            bigpanel1.Hide();
            bigpanel2.BigLabel.Text = "Editing a Multiple choices question";
            bigpanel2.MainPanel.AutoScrollPosition = new Point(0, 0);
            bigpanel2.SubMainPanel.AutoScrollPosition = new Point(0, 0);
            bigpanel2.ChoiceP.AutoScrollPosition = new Point(0, 0);
            mainpanel.Controls.Add(bigpanel2);
            mainpanel.Tag = bigpanel2;
            bigpanel2.Show();
        }

        // event for delete label beside each question in questions, delete the selected question
        private void BP1deletelabel_Click(object sender, EventArgs e, Question question)
        {
            if(question != null)
            {
                question.Category.QuestionList.Remove(question);
                Category_ItemsChanged();
                updateQuestiondisplay(bigpanel1.SubcategoriesQ.Checked);
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        // add category event for button
        private void BP1addcategory_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(bigpanel1.CName.Text))
            {
                MessageBox.Show("Category name is invalid");
                return;
            }

            int cid = -1;
            if (!String.IsNullOrWhiteSpace(bigpanel1.CId.Text))
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

            Category Cparent = bigpanel1.PCategory.SelectedItem as Category;
            Category category = new Category(Cparent, new List<Category>(), bigpanel1.CName.Text, bigpanel1.CInfo.Text, cid, Cparent.Gen + 1, new List<Question>());
            Cparent.addChild(category);
            int index = categories.FindIndex(cat => cat == Cparent);

            if (index != -1)
            {
                categories.Insert(index + 1, category);
            }
            else
            {
                categories.Add(category);
            }

            // reset textbox
            bigpanel1.CName.ResetText();
            bigpanel1.CInfo.ResetText();
            bigpanel1.CId.ResetText();
            bigpanel1.PCategory.SelectedIndex = 0;
            //
            Category_ItemsChanged();
            bigpanel1.DirectTab.SelectedIndex = 0;
            MessageBox.Show("Successfully created a new category!");
        }

        // drag enter event when drag files on import panel
        private void importPanel_DragEnter(object sender, DragEventArgs e)
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
                    if (fileSize > 50*1024*1024) // MAX 50MB
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
        private void importPanel_DragDrop(object sender, DragEventArgs e)
        {
            // Get the dropped data
            string[] filepaths = (string[])e.Data.GetData(DataFormats.FileDrop);
            importFilePath = filepaths[0];
            string extension = System.IO.Path.GetExtension(importFilePath);

            bigpanel1.closeImportBtn.Show();
            bigpanel1.closeImportBtn.Click += new System.EventHandler(this.closeImportFileButton_Click);
            bigpanel1.importLabel.Text = importFilePath;
            if (extension == ".txt")
            {
                bigpanel1.importImageLabel.Image = global::ExamApp.Properties.Resources.txtIcon;
            }
            else
            {
                bigpanel1.importImageLabel.Image = global::ExamApp.Properties.Resources.docxIcon;
            }
        }

        // click event for "x" button appeared when drag a file successfully
        private void closeImportFileButton_Click(object sender, EventArgs e)
        {
            bigpanel1.importImageLabel.Image = global::ExamApp.Properties.Resources.icon4;
            bigpanel1.importLabel.Text = "You can drag and drop files here to add them.";
            importFilePath = "";
            bigpanel1.closeImportBtn.Hide();
            bigpanel1.closeImportBtn.Click -= new System.EventHandler(this.closeImportFileButton_Click);
        }

        // click event for IMPORT button
        private void BP1importbutton_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(importFilePath))
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
                        question.Category = categories[0];
                        categories[0].addQuestion(question);
                    }
                    updateQuestiondisplay(false);
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

            bigpanel1.importImageLabel.Image = global::ExamApp.Properties.Resources.icon4;
            bigpanel1.importLabel.Text = "You can drag and drop files here to add them.";
            importFilePath = "";
            bigpanel1.closeImportBtn.Hide();
            bigpanel1.closeImportBtn.Click -= new System.EventHandler(this.closeImportFileButton_Click);
        }

        //
/* edit question form*/

        // save and quit button in edit question form
        private void BP2savebutton1_Click(object sender, EventArgs e, Question question)
        {
            if (String.IsNullOrWhiteSpace(this.bigpanel2.QName.Text))
            {
                MessageBox.Show("Question Name is empty");
                return;
            }

            if (String.IsNullOrWhiteSpace(this.bigpanel2.QText.Text))
            {
                MessageBox.Show("Question Text is empty");
                return;
            }

            if (String.IsNullOrWhiteSpace(this.bigpanel2.QMark.Text))
            {
                MessageBox.Show("Default Mark is empty");
                return;
            }

            double mark;

            try
            {
                mark = Convert.ToDouble(this.bigpanel2.QMark.Text);
            }

            catch (FormatException)
            {
                MessageBox.Show("Please input a valid value for Default Mark!");
                return;
            }


            Question tempQuestion = new Question
            {
                Name = this.bigpanel2.QName.Text,
                Description = this.bigpanel2.QText.Rtf,
                Mark = mark,
                Choices = new List<Choice>(5) { null, null, null, null, null }
            };

            int choiceNum = 0;
            if (!String.IsNullOrWhiteSpace(this.bigpanel2.C1Text.Text))
            {
                double grade = 0;

                if (this.bigpanel2.C1Grade.Text != "None")
                {
                    grade = Convert.ToDouble(this.bigpanel2.C1Grade.Text.TrimEnd('%')) / 100.0;
                }

                Choice choice = new Choice
                {
                    Text = this.bigpanel2.C1Text.Rtf,
                    Grade = grade
                };

                tempQuestion.Choices[0] = choice;
                choiceNum++;
            }

            if (!String.IsNullOrWhiteSpace(this.bigpanel2.C2Text.Text))
            {
                double grade = 0;
                if (this.bigpanel2.C2Grade.Text != "None")
                {
                    grade = Convert.ToDouble(this.bigpanel2.C2Grade.Text.TrimEnd('%')) / 100.0;
                }
                Choice choice = new Choice
                {
                    Text = this.bigpanel2.C2Text.Rtf,
                    Grade = grade
                };
                tempQuestion.Choices[1] = choice;
                choiceNum++;
            }

            if (!String.IsNullOrWhiteSpace(this.bigpanel2.C3Text.Text))
            {
                double grade = 0;
                if (this.bigpanel2.C3Grade.Text != "None")
                {
                    grade = Convert.ToDouble(this.bigpanel2.C3Grade.Text.TrimEnd('%')) / 100.0;
                }
                Choice choice = new Choice
                {
                    Text = this.bigpanel2.C3Text.Rtf,
                    Grade = grade
                };
                tempQuestion.Choices[2] = choice;
                choiceNum++;
            }

            if (!String.IsNullOrWhiteSpace(this.bigpanel2.C4Text.Text))
            {
                double grade = 0;
                if (this.bigpanel2.C4Grade.Text != "None")
                {
                    grade = Convert.ToDouble(this.bigpanel2.C4Grade.Text.TrimEnd('%')) / 100.0;
                }
                Choice choice = new Choice
                {
                    Text = this.bigpanel2.C4Text.Rtf,
                    Grade = grade
                };
                tempQuestion.Choices[3] = choice;
                choiceNum++;
            }

            if (!String.IsNullOrWhiteSpace(this.bigpanel2.C5Text.Text))
            {
                double grade = 0;
                if (this.bigpanel2.C5Grade.Text != "None")
                {
                    grade = Convert.ToDouble(this.bigpanel2.C5Grade.Text.TrimEnd('%')) / 100.0;
                }
                Choice choice = new Choice
                {
                    Text = this.bigpanel2.C5Text.Rtf,
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

            if(question == null)
            {
                tempQuestion.Category = currentcategory;
                this.currentcategory.addQuestion(tempQuestion);
                Category_ItemsChanged();
            }
            else
            {
                question.Name = tempQuestion.Name;
                question.Description = tempQuestion.Description;
                question.Mark = tempQuestion.Mark;
                question.Choices = tempQuestion.Choices;
                if (currentcategory != question.Category)
                {
                    question.Category.QuestionList.Remove(question);
                    question.Category = currentcategory;
                    currentcategory.addQuestion(question);
                    Category_ItemsChanged();
                }
                currentquestion = null;
            }
            
            //
            bigpanel2.QName.ResetText();
            bigpanel2.QText.ResetText();
            bigpanel2.QMark.Text = "1";
            bigpanel2.C1Text.ResetText();
            bigpanel2.C2Text.ResetText();
            bigpanel2.C3Text.ResetText();
            bigpanel2.C4Text.ResetText();
            bigpanel2.C5Text.ResetText();
            bigpanel2.C1Grade.Text = "None";
            bigpanel2.C2Grade.Text = "None";
            bigpanel2.C3Grade.Text = "None";
            bigpanel2.C4Grade.Text = "None";
            bigpanel2.C5Grade.Text = "None";
            //
            slash2.Hide();
            direction3.Hide();
            direction2.Click -= new System.EventHandler(this.direction2_Click);
            direction2.ForeColor = System.Drawing.Color.Black;
            direction2.Cursor = System.Windows.Forms.Cursors.Default;
            updateQuestiondisplay(bigpanel1.SubcategoriesQ.Checked);
            //
            mainpanel.Controls.RemoveAt(0);
            mainpanel.Controls.Add(bigpanel1);
            mainpanel.Tag = bigpanel1;
            bigpanel1.Show();
        }

        // save and continue button in edit question form
        private void BP2savebutton2_Click(object sender, EventArgs e, Question question)
        {
            if (String.IsNullOrWhiteSpace(this.bigpanel2.QName.Text))
            {
                MessageBox.Show("Question Name is empty");
                return;
            }

            if (String.IsNullOrWhiteSpace(this.bigpanel2.QText.Text))
            {
                MessageBox.Show("Question Text is empty");
                return;
            }

            if (String.IsNullOrWhiteSpace(this.bigpanel2.QMark.Text))
            {
                MessageBox.Show("Default Mark is empty");
                return;
            }

            double mark;

            try
            {
                mark = Convert.ToDouble(this.bigpanel2.QMark.Text);
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
                    Name = this.bigpanel2.QName.Text,
                    Description = this.bigpanel2.QText.Rtf,
                    Mark = mark,
                    Choices = new List<Choice>(5) { null, null, null, null, null }
                };

                question.Category = currentcategory;
                this.currentcategory.addQuestion(question);
                Category_ItemsChanged();

                if (!String.IsNullOrWhiteSpace(this.bigpanel2.C1Text.Text))
                {
                    double grade = 0;

                    if (this.bigpanel2.C1Grade.Text != "None")
                    {
                        grade = Convert.ToDouble(this.bigpanel2.C1Grade.Text.TrimEnd('%')) / 100.0;
                    }

                    Choice choice = new Choice
                    {
                        Text = this.bigpanel2.C1Text.Text,
                        Grade = grade
                    };

                    question.Choices[0] = choice;
                }

                if (!String.IsNullOrWhiteSpace(this.bigpanel2.C2Text.Text))
                {
                    double grade = 0;
                    if (this.bigpanel2.C2Grade.Text != "None")
                    {
                        grade = Convert.ToDouble(this.bigpanel2.C2Grade.Text.TrimEnd('%')) / 100.0;
                    }
                    Choice choice = new Choice
                    {
                        Text = this.bigpanel2.C2Text.Text,
                        Grade = grade
                    };
                    question.Choices[1] = choice;
                }

                if (!String.IsNullOrWhiteSpace(this.bigpanel2.C3Text.Text))
                {
                    double grade = 0;
                    if (this.bigpanel2.C3Grade.Text != "None")
                    {
                        grade = Convert.ToDouble(this.bigpanel2.C3Grade.Text.TrimEnd('%')) / 100.0;
                    }
                    Choice choice = new Choice
                    {
                        Text = this.bigpanel2.C3Text.Text,
                        Grade = grade
                    };
                    question.Choices[2] = choice;
                }

                if (!String.IsNullOrWhiteSpace(this.bigpanel2.C4Text.Text))
                {
                    double grade = 0;
                    if (this.bigpanel2.C4Grade.Text != "None")
                    {
                        grade = Convert.ToDouble(this.bigpanel2.C4Grade.Text.TrimEnd('%')) / 100.0;
                    }
                    Choice choice = new Choice
                    {
                        Text = this.bigpanel2.C4Text.Text,
                        Grade = grade
                    };
                    question.Choices[3] = choice;
                }

                if (!String.IsNullOrWhiteSpace(this.bigpanel2.C5Text.Text))
                {
                    double grade = 0;
                    if (this.bigpanel2.C5Grade.Text != "None")
                    {
                        grade = Convert.ToDouble(this.bigpanel2.C5Grade.Text.TrimEnd('%')) / 100.0;
                    }
                    Choice choice = new Choice
                    {
                        Text = this.bigpanel2.C5Text.Text,
                        Grade = grade
                    };
                    question.Choices[4] = choice;
                }

                currentquestion = question;
            }
            else
            {
                this.currentquestion.Name = this.bigpanel2.QName.Text;
                this.currentquestion.Description = this.bigpanel2.QText.Rtf;
                this.currentquestion.Mark = mark;

                if (currentcategory != currentquestion.Category)
                {
                    currentquestion.Category.QuestionList.Remove(currentquestion);
                    currentquestion.Category = currentcategory;
                    currentcategory.addQuestion(currentquestion);
                    Category_ItemsChanged();
                }

                if (!String.IsNullOrWhiteSpace(this.bigpanel2.C1Text.Text))
                {
                    double grade = 0;

                    if (this.bigpanel2.C1Grade.Text != "None")
                    {
                        grade = Convert.ToDouble(this.bigpanel2.C1Grade.Text.TrimEnd('%')) / 100.0;
                    }

                    if (currentquestion.Choices[0] != null)
                    {
                        currentquestion.Choices[0].Text = this.bigpanel2.C1Text.Text;
                        currentquestion.Choices[0].Grade = grade;
                    }
                    else
                    {
                        Choice choice = new Choice
                        {
                            Text = this.bigpanel2.C1Text.Text,
                            Grade = grade
                        };
                        currentquestion.Choices[0] = choice;
                    }

                }
                else
                {
                    if (currentquestion.Choices[0] != null)
                    {
                        currentquestion.Choices[0] = null;
                    }
                }

                if (!String.IsNullOrWhiteSpace(this.bigpanel2.C2Text.Text))
                {
                    double grade = 0;

                    if (this.bigpanel2.C2Grade.Text != "None")
                    {
                        grade = Convert.ToDouble(this.bigpanel2.C2Grade.Text.TrimEnd('%')) / 100.0;
                    }

                    if (currentquestion.Choices[1] != null)
                    {
                        currentquestion.Choices[1].Text = this.bigpanel2.C2Text.Text;
                        currentquestion.Choices[1].Grade = grade;
                    }
                    else
                    {
                        Choice choice = new Choice
                        {
                            Text = this.bigpanel2.C2Text.Text,
                            Grade = grade
                        };
                        currentquestion.Choices[1] = choice;
                    }

                }
                else
                {
                    if (currentquestion.Choices[1] != null)
                    {
                        currentquestion.Choices[1] = null;
                    }
                }

                if (!String.IsNullOrWhiteSpace(this.bigpanel2.C3Text.Text))
                {
                    double grade = 0;

                    if (this.bigpanel2.C3Grade.Text != "None")
                    {
                        grade = Convert.ToDouble(this.bigpanel2.C3Grade.Text.TrimEnd('%')) / 100.0;
                    }

                    if (currentquestion.Choices[2] != null)
                    {
                        currentquestion.Choices[2].Text = this.bigpanel2.C3Text.Text;
                        currentquestion.Choices[2].Grade = grade;
                    }
                    else
                    {
                        Choice choice = new Choice
                        {
                            Text = this.bigpanel2.C3Text.Text,
                            Grade = grade
                        };
                        currentquestion.Choices[2] = choice;
                    }

                }
                else
                {
                    if (currentquestion.Choices[2] != null)
                    {
                        currentquestion.Choices[2] = null;
                    }
                }

                if (!String.IsNullOrWhiteSpace(this.bigpanel2.C4Text.Text))
                {
                    double grade = 0;

                    if (this.bigpanel2.C4Grade.Text != "None")
                    {
                        grade = Convert.ToDouble(this.bigpanel2.C4Grade.Text.TrimEnd('%')) / 100.0;
                    }

                    if (currentquestion.Choices[3] != null)
                    {
                        currentquestion.Choices[3].Text = this.bigpanel2.C4Text.Text;
                        currentquestion.Choices[3].Grade = grade;
                    }
                    else
                    {
                        Choice choice = new Choice
                        {
                            Text = this.bigpanel2.C4Text.Text,
                            Grade = grade
                        };
                        currentquestion.Choices[3] = choice;
                    }

                }
                else
                {
                    if (currentquestion.Choices[3] != null)
                    {
                        currentquestion.Choices[3] = null;
                    }
                }

                if (!String.IsNullOrWhiteSpace(this.bigpanel2.C5Text.Text))
                {
                    double grade = 0;

                    if (this.bigpanel2.C5Grade.Text != "None")
                    {
                        grade = Convert.ToDouble(this.bigpanel2.C5Grade.Text.TrimEnd('%')) / 100.0;
                    }

                    if (currentquestion.Choices[4] != null)
                    {
                        currentquestion.Choices[4].Text = this.bigpanel2.C5Text.Text;
                        currentquestion.Choices[4].Grade = grade;
                    }
                    else
                    {
                        Choice choice = new Choice
                        {
                            Text = this.bigpanel2.C5Text.Text,
                            Grade = grade
                        };
                        currentquestion.Choices[4] = choice;
                    }

                }
                else
                {
                    if (currentquestion.Choices[4] != null)
                    {
                        currentquestion.Choices[4] = null;
                    }
                }
            }
            MessageBox.Show("Saved!");
        }

        // cancel button in edit question form
        private void BP2cancelbutton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to discard the changes and quit to Question Bank?", "Cancel", MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
            {
                return;
            }

            bigpanel2.QName.ResetText();
            bigpanel2.QText.ResetText();
            bigpanel2.QMark.Text = "1";
            bigpanel2.C1Text.ResetText();
            bigpanel2.C2Text.ResetText();
            bigpanel2.C3Text.ResetText();
            bigpanel2.C4Text.ResetText();
            bigpanel2.C5Text.ResetText();
            bigpanel2.C1Grade.Text = "None";
            bigpanel2.C2Grade.Text = "None";
            bigpanel2.C3Grade.Text = "None";
            bigpanel2.C4Grade.Text = "None";
            bigpanel2.C5Grade.Text = "None";
            bigpanel2.Hide();
            slash2.Hide();
            direction3.Hide();
            direction2.Click -= new System.EventHandler(this.direction2_Click);
            direction2.ForeColor = System.Drawing.Color.Black;
            direction2.Cursor = System.Windows.Forms.Cursors.Default;
            mainpanel.Controls.RemoveAt(0);
            mainpanel.Controls.Add(bigpanel1);
            mainpanel.Tag = bigpanel1;
            bigpanel1.Show();
        }

        //
/* quiz form*/

        // create quiz button event
        private void BP3createbutton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(bigpanel3.QuizName.Text))
            {
                MessageBox.Show("Please enter a valid name for the quiz");
                return;
            }

            DateTime open = DateTime.MinValue;
            DateTime close = DateTime.MaxValue;
            int coeff = 0;
            string unit = null;

            if (bigpanel3.CBTimeO.Checked)
            {
                open = bigpanel3.DTOpen.Value;
            }

            if (bigpanel3.CBTimeC.Checked)
            {
                close = bigpanel3.DTClose.Value;
            }

            if (bigpanel3.CBTimeL.Checked)
            {
                try
                {
                    coeff = Convert.ToInt32(bigpanel3.TimeCoeff.Text);
                }

                catch (FormatException)
                {
                    MessageBox.Show("Please input a valid value for Time Limit!");
                    return;
                }
                unit = bigpanel3.TimeUnit.Text;
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
                Name = bigpanel3.QuizName.Text,
                Description = bigpanel3.QuizDescription.Text,
                QuestionList = new List<Question>(),
                TotalMark = 0,
                MaxGrade = 10.0,
                PendingList = new List<Question>(),
                Previews = new List<PreviewQuiz>(),
                Time = timing
            };

            quizs.Add(newquiz);

            bigpanel3.QuizName.ResetText();
            bigpanel3.QuizDescription.ResetText();
            bigpanel3.DTOpen.Value = DateTime.Now;
            bigpanel3.DTClose.Value = DateTime.Now;
            bigpanel3.TimeCoeff.Text = "0";
            bigpanel3.TimeUnit.SelectedIndex = 1;
            bigpanel3.CBTimeO.Checked = false;
            bigpanel3.CBTimeC.Checked = false;
            bigpanel3.CBTimeL.Checked = false;
            bigpanel3.MainPanel.AutoScrollPosition = new Point(0, 0);
            bigpanel3.Hide();
            //
            this.heading.Size = new Size(1114, 117);
            this.functionbutton1.Show();
            this.button1.Show();
            slash1.Hide();
            direction2.Hide();
            direction1.ForeColor = SystemColors.ControlText;
            direction1.Cursor = Cursors.Default;
            direction1.Click -= new EventHandler(this.direction1_Click);
            //
            mainpanel.Controls.RemoveAt(0);
            updateQuizdisplay();
        }

        // cancel quiz button event
        private void BP3cancelbutton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to discard the changes and quit to Home?", "Cancel", MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
            {
                return;
            }

            bigpanel3.QuizName.ResetText();
            bigpanel3.QuizDescription.ResetText();
            bigpanel3.DTOpen.Value = DateTime.Now;
            bigpanel3.DTClose.Value = DateTime.Now;
            bigpanel3.TimeCoeff.Text = "0";
            bigpanel3.TimeUnit.SelectedIndex = 1;
            bigpanel3.CBTimeO.Checked = false;
            bigpanel3.CBTimeC.Checked = false;
            bigpanel3.CBTimeL.Checked = false;
            bigpanel3.MainPanel.AutoScrollPosition = new Point(0, 0);
            bigpanel3.Hide();
            //
            this.heading.Size = new Size(1114, 117);
            this.functionbutton1.Show();
            this.button1.Show();
            slash1.Hide();
            direction2.Hide();
            direction1.ForeColor = SystemColors.ControlText;
            direction1.Cursor = Cursors.Default;
            direction1.Click -= new EventHandler(this.direction1_Click);
            //
            mainpanel.Controls.RemoveAt(0);
            updateQuizdisplay();
        }

        //
/* quiz attempt form*/

        private void gearSettingLabel_Click(object sender, EventArgs e, Quiz quiz)
        {
            if (bigpanel4.Visible == false)
            {
                return;
            }

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (bigpanel5 == null)
            {
                openQuizEditPanel(new quiz_edit_form());
            }
            else
            {
                mainpanel.Controls.Add(bigpanel5);
                mainpanel.Tag = bigpanel5;
                bigpanel5.Show();
            }

            bigpanel5.QuestionNameLabel.Text = quiz.Name;
            bigpanel5.QuestionNumberLabel.Text = quiz.QuestionList.Count.ToString();
            bigpanel5.TotalMarkLabel.Text = quiz.TotalMark.ToString();

            bigpanel4.Hide();
            slash1.Show();
            slash2.Show();
            direction1.Click -= new EventHandler(this.direction1_Click);
            direction1.Click += new EventHandler(this.direction1_1_Click);
            direction3.Text = "Edit quiz";
            direction3.Show();
            direction2.Cursor = Cursors.Hand;
            direction2.ForeColor = Color.IndianRed;
            direction2.Click += new EventHandler(this.direction2_1_Click);
        }

        private void startQuizAttempt_Click(object sender, EventArgs e)
        {
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (bigpanel6 == null)
            {
                openQuizPlay(new quiz_play());
            }
            else
            {
                mainpanel.Controls.Add(bigpanel6);
                mainpanel.Tag = bigpanel6;
                bigpanel6.Show();
            }

            initiateQuizPlay();
        }

        private void continueQuizAttempt_Click(object sender, EventArgs e)
        {
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (bigpanel6 == null)
            {
                openQuizPlay(new quiz_play());
            }
            else
            {
                mainpanel.Controls.Add(bigpanel6);
                mainpanel.Tag = bigpanel6;
                bigpanel6.Show();
            }

        }


        //
/* quiz play form*/

        // initiate function
        private void initiateQuizPlay()
        {
            if (currentquiz.Time.TimeLimit == 0)
            {
                bigpanel6.Timer.Text = "Time Left: None";
            }
            else
            {
                bigpanel6.StartTimer(currentquiz.Time.TimeLimit);
            }

            int quizSize = currentquiz.QuestionList.Count();
            currentquiz.OngoingPreview = new PreviewQuiz
            {
                AnswerList = new List<QAnswer>(),
                qLayouts = new List<QuestionDisplay>()
            };

            for (int i = 0; i < quizSize; i++)
            {
                currentquiz.OngoingPreview.qLayouts.Add(null);
            }
            
            if (currentquiz.Shuffle)
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
                Question question = currentquiz.QuestionList[ShuffleTool.shuffleIndex[i]];
                ButtonPlus temp = new ButtonPlus
                {
                    QuestionAttached = question,
                    Text = index.ToString(),
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink
                };
                index++;
                temp.Click += new EventHandler(this.BP6_QuestionIndex_Click);
                bigpanel6.QuestionIndexFlowLayout.Controls.Add(temp);
            }

            Question question_1 = currentquiz.QuestionList[ShuffleTool.shuffleIndex[0]];
            QuestionDisplay temp1 = new QuestionDisplay(question_1);
            bigpanel6.QuestionPanel.Controls.Add(temp1);
            bigpanel6.QuestionPanel.Controls.SetChildIndex(temp1, 1);
            bigpanel6.QuestionIndexLabel.Text = "1";
            currentquiz.OngoingPreview.qLayouts[0] = temp1;
        }

        // question index click event
        private void BP6_QuestionIndex_Click(object sender, EventArgs e)
        {
            ButtonPlus button = sender as ButtonPlus;
            int index = Convert.ToInt32(button.Text) - 1;
            if (currentquiz.OngoingPreview.qLayouts[index] != null)
            {
                bigpanel6.QuestionPanel.Controls.RemoveAt(1);
                bigpanel6.QuestionPanel.Controls.Add(currentquiz.OngoingPreview.qLayouts[index]);
                bigpanel6.QuestionPanel.Controls.SetChildIndex(currentquiz.OngoingPreview.qLayouts[index], 1);
            }
            else
            {
                BP6_ShowQuestion(button.QuestionAttached, index);
            }
        }

        // method to show question
        private void BP6_ShowQuestion(Question question, int index)
        {
            QuestionDisplay temp = new QuestionDisplay(question);
            bigpanel6.QuestionPanel.Controls.RemoveAt(1);
            bigpanel6.QuestionPanel.Controls.Add(temp);
            bigpanel6.QuestionPanel.Controls.SetChildIndex(temp, 1);
            bigpanel6.QuestionIndexLabel.Text = $"{index + 1}";
            currentquiz.OngoingPreview.qLayouts[index] = temp;
        }

        //
/* quiz edit form*/

        // add question to quiz 2 ("from question bank")
        private void addQ2Q2_Click(object sender, EventArgs e, Quiz quiz)
        {
            if (bigpanel5.Visible == false)
            {
                return;
            }
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (bigpanel5_2 == null)
            {
                openAddQuestionToQuiz2(new addquestiontoquiz2_form());
            }
            else
            {
                mainpanel.Controls.Add(bigpanel5_2);
                mainpanel.Tag = bigpanel5_2;
                bigpanel5_2.Show();
            }
            try
            {
                bigpanel5_2.CategoryComboBox.SelectedIndexChanged -= new EventHandler(this.Add2Category_SelectIndexChanged);
            }
            catch
            {

            }
            bigpanel5_2.CategoryComboBox.DataSource = null;
            bigpanel5_2.CategoryComboBox.DataSource = categories;
            bigpanel5_2.CategoryComboBox.DisplayMember = "NameAndGen";
            bigpanel5_2.CategoryComboBox.SelectedItem = null;
            bigpanel5_2.QuestionDisplay.Controls.Clear();
            bigpanel5_2.CategoryComboBox.SelectedIndexChanged += new EventHandler(this.Add2Category_SelectIndexChanged);

            bigpanel5.Hide();
            bigpanel5.Popup.Hide();
            slash3.Show();
            direction4.Show();
            direction3.Cursor = Cursors.Hand;
            direction3.ForeColor = Color.IndianRed;
            direction3.Click += new EventHandler(this.direction3_Click);
        }

        // save button event on edit form
        private void saveBP5_Click(object sender, EventArgs e)
        {
            try
            {
                currentquiz.MaxGrade = Convert.ToDouble(bigpanel5.GradeTextBox.Text);
            }
            catch
            {
                currentquiz.MaxGrade = 10.0;
            }

            currentquiz.TotalMark = Convert.ToDouble(bigpanel5.TotalMarkLabel.Text);
            if (bigpanel5.ShuffleBox.Checked)
            {
                currentquiz.Shuffle = true;
            }

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            slash2.Hide();
            direction3.Hide();
            direction2.Cursor = Cursors.Default;
            direction2.ForeColor = Color.Black;
            direction2.Click -= new EventHandler(this.direction2_1_Click);
            bigpanel5.Hide();
            bigpanel4.Show();
            mainpanel.Controls.Add(bigpanel4);
        }

        //
/* add question to quiz forms*/

        // event for indexing page
        private void IndexingPage_BP5()
        {
            bigpanel5.PageFlowLayout.Controls.Clear();
            int pageNum = (currentquiz.QuestionList.Count() - 1) / 10 + 1;
            for(int i = 1; i <= pageNum; i++)
            {
                Button temp = new Button();
                temp.Text = i.ToString();
                temp.AutoSize = true;
                temp.Click += new EventHandler(this.BP5_PageBtn_Click);
                bigpanel5.PageFlowLayout.Controls.Add(temp);
            }
        }

        // click event for page button
        private void BP5_PageBtn_Click(object sender, EventArgs e)
        {
            Button temp = (Button)sender;
            ChangePage_BP5(Convert.ToInt32(temp.Text));
        }

        // event for changing page
        private void ChangePage_BP5(int page)
        {
            bigpanel5.PageNumberLabel.Text = $"Page {page}";
            int quizQuestionLen = currentquiz.QuestionList.Count();
            int start = (page - 1) * 10, end = page * 10 - 1;

            if((quizQuestionLen - 1) < end)
            {
                end = quizQuestionLen - 1;
            }

            bigpanel5.QuestionLayout.Controls.Clear();
            for(int i = end; i >= start; i--)
            {
                QuizQuestionBlockBP5 temp = new QuizQuestionBlockBP5(currentquiz.QuestionList[i], i + 1);
                temp.Edit.Click += new EventHandler(BP5_QuestionTrashIcon_Click);
                bigpanel5.QuestionLayout.Controls.Add(temp);
            }
        }

        // click event for trash icon in question layout
        private void BP5_QuestionTrashIcon_Click(object sender, EventArgs e)
        {
            QuizQuestionBlockBP5 temp = (QuizQuestionBlockBP5)((Label)sender).Parent;
            Panel container = temp.Parent as Panel;
            int index = temp.Index;
            foreach(QuizQuestionBlockBP5 temp1 in container.Controls)
            {
                if(temp1.Index > index)
                {
                    temp1.setIndex(temp1.Index - 1);
                }
            }
            bigpanel5.QuestionLayout.Controls.Remove(temp);
            currentquiz.QuestionList.RemoveAt(index - 1);
            if (currentquiz.QuestionList.Count() % 10 == 0)
            {
                bigpanel5.PageFlowLayout.Controls.RemoveAt(currentquiz.QuestionList.Count() / 10);
            }
            int lastIndex = ((index - 1) / 10 + 1) * 10;
            if(lastIndex <= currentquiz.QuestionList.Count())
            {
                QuizQuestionBlockBP5 temp2 = new QuizQuestionBlockBP5(currentquiz.QuestionList[lastIndex - 1], lastIndex);
                temp2.Edit.Click += new EventHandler(BP5_QuestionTrashIcon_Click);
                bigpanel5.QuestionLayout.Controls.Add(temp2);
                bigpanel5.QuestionLayout.Controls.SetChildIndex(temp2, 0);
            }
        }

        // click event for "add selected question to quiz"
        private void Add2_AddSelectedToQuiz(object sender, EventArgs e)
        {
            double totalMark = 0;
            if (bigpanel5_2.AddAll.Checked)
            {
                foreach (Question question in this.currentcategory.QuestionList)
                {
                    totalMark += question.Mark;
                    currentquiz.addquestion(question);
                }

                if (bigpanel5_2.SubcategoryCheckBox.Checked)
                {
                    foreach (Category subcategory in currentcategory.Child)
                    {
                        foreach (Question question in subcategory.QuestionList)
                        {
                            totalMark += question.Mark;
                            currentquiz.addquestion(question);
                        }
                    }
                    bigpanel5_2.SubcategoryCheckBox.Checked = false;
                }
                bigpanel5_2.AddAll.Checked = false;
            }
            else
            {
                foreach (Question question in currentquiz.PendingList)
                {
                    totalMark += question.Mark;
                    currentquiz.addquestion(question);
                }
            }

            currentquiz.PendingList.Clear();
            bigpanel5.QuestionNumberLabel.Text = currentquiz.QuestionList.Count().ToString();
            bigpanel5.TotalMarkLabel.Text = totalMark.ToString();
            IndexingPage_BP5();
            ChangePage_BP5(1);

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            slash3.Hide();
            direction4.Hide();
            direction3.Cursor = Cursors.Default;
            direction3.ForeColor = Color.Black;
            direction3.Click -= new EventHandler(this.direction3_Click);
            bigpanel5_2.Hide();
            bigpanel5.Show();
            mainpanel.Controls.Add(bigpanel5);
        }

        // refresh question display in add2
        private void Add2_RefreshQuestionDisplay(bool mode)
        {
            if (currentcategory != null)
            {
                this.bigpanel5_2.QuestionDisplay.Controls.Clear();
                this.bigpanel5_2.AddedQuestionDisplay.Controls.Clear();
                foreach (Question question in this.currentcategory.QuestionList)
                {
                    if (!currentquiz.PendingList.Contains(question))
                    {
                        QuizQuestionBlockBP5_2 temp = new QuizQuestionBlockBP5_2(question, false);
                        this.bigpanel5_2.QuestionDisplay.Controls.Add(temp);
                        temp.Edit.Click += delegate (object sender1, EventArgs e1) { this.Add2_addToPending_Click(sender1, e1, temp.Question); };
                    }
                }

                if (mode)
                {
                    foreach (Category subcategory in currentcategory.Child)
                    {
                        foreach (Question question in subcategory.QuestionList)
                        {
                            if (!currentquiz.PendingList.Contains(question))
                            {
                                QuizQuestionBlockBP5_2 temp = new QuizQuestionBlockBP5_2(question, false);
                                this.bigpanel5_2.QuestionDisplay.Controls.Add(temp);
                                temp.Edit.Click += delegate (object sender1, EventArgs e1) { this.Add2_addToPending_Click(sender1, e1, temp.Question); };
                            }
                        }
                    }
                }

                foreach (Question question in currentquiz.PendingList)
                {
                    QuizQuestionBlockBP5_2 temp = new QuizQuestionBlockBP5_2(question, true);
                    this.bigpanel5_2.AddedQuestionDisplay.Controls.Add(temp);
                }
            }
        }

        // click event for when clicking plus icon on the left of the question block in add2
        private void Add2_addToPending_Click(object sender, EventArgs e, Question question)
        {
            currentquiz.PendingList.Add(question);
            QuizQuestionBlockBP5_2 temp = (QuizQuestionBlockBP5_2)((Label)sender).Parent;
            temp.Edit.Image = Properties.Resources.icon11;
            temp.Edit.Click -= delegate (object sender1, EventArgs e1) { this.Add2_addToPending_Click(sender1, e1, temp.Question); };
            temp.Edit.Click += delegate (object sender1, EventArgs e1) { this.Add2_minusFromPending_Click(sender1, e1, temp.Question); };
        }

        // click event for when clicking minus icon on the left of the question block in add2
        private void Add2_minusFromPending_Click(object sender, EventArgs e, Question question)
        {
            currentquiz.PendingList.Remove(question);
            QuizQuestionBlockBP5_2 temp = (QuizQuestionBlockBP5_2)((Label)sender).Parent;
            temp.Edit.Image = Properties.Resources.icon10;
            temp.Edit.Click -= delegate (object sender1, EventArgs e1) { this.Add2_minusFromPending_Click(sender1, e1, temp.Question); };
            temp.Edit.Click += delegate (object sender1, EventArgs e1) { this.Add2_addToPending_Click(sender1, e1, temp.Question); };
        }

        // checkchanged event for "subcategory" checkbox in add2
        private void Add2_Subcategory_CheckedChanged(object sender, EventArgs e)
        {
            Add2_RefreshQuestionDisplay(bigpanel5_2.SubcategoryCheckBox.Checked);
        }

        // category index change event in add2
        private void Add2Category_SelectIndexChanged(object sender, EventArgs e)
        {
            if(bigpanel5_2.CategoryComboBox.SelectedItem != null)
            {
                currentcategory = bigpanel5_2.CategoryComboBox.SelectedItem as Category;
                Add2_RefreshQuestionDisplay(bigpanel5_2.SubcategoryCheckBox.Checked);
            }
        }

        //
/* headings */

        // direct to home when click the home label on top
        private void direction1_Click(object sender, EventArgs e)
        {
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (bigpanel1 != null)
            {
                this.bigpanel1.Hide();
            }

            if (bigpanel2 != null)
            {
                this.bigpanel2.Hide();
            }

            if(bigpanel3 != null)
            {
                this.bigpanel3.Hide();
            }

            this.heading.Size = new Size(1114, 117);
            this.functionbutton1.Show();
            this.button1.Show();
            slash1.Hide();
            slash2.Hide();
            direction2.Hide();
            direction3.Hide();
            direction1.ForeColor = SystemColors.ControlText;
            direction1.Cursor = Cursors.Default;
            direction1.Click -= new EventHandler(this.direction1_Click);
            mainpanel.Controls.Add(quizFlowLayout);
        }

        // direct to home when click the home label on top (alter)
        private void direction1_1_Click(object sender, EventArgs e)
        {
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (bigpanel5 != null && bigpanel5.Visible)
            {
                this.bigpanel5.Hide();
            }

            if (bigpanel5_2 != null && bigpanel5_2.Visible)
            {
                DialogResult result = MessageBox.Show("Do you want to undo the changes?", "Directing to home page", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return;
                }

                currentquiz.PendingList.Clear();
                this.bigpanel5.Hide();
            }

            this.heading.Size = new Size(1114, 117);
            this.functionbutton1.Show();
            this.button1.Show();
            slash1.Hide();
            slash2.Hide();
            slash3.Hide();
            direction2.Hide();
            direction3.Hide();
            direction4.Hide();
            direction1.ForeColor = SystemColors.ControlText;
            direction1.Cursor = Cursors.Default;
            direction1.Click -= new EventHandler(this.direction1_1_Click);
            direction2.Click -= new EventHandler(this.direction2_1_Click);
            direction3.Click -= new EventHandler(this.direction3_Click);
            mainpanel.Controls.Add(quizFlowLayout);
        }

        // direct to edit form when click the question bank label on top (alter for direct from bigpanel 5* -> 4)
        private void direction2_Click(object sender, EventArgs e)
        {
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }
            //
            bigpanel2.Hide();
            //
            slash1.Hide();
            slash2.Hide();
            direction2.Hide();
            direction3.Hide();
            direction2.Click -= new EventHandler(this.direction2_Click);
            updateQuestiondisplay(bigpanel1.SubcategoriesQ.Checked);
            //
            mainpanel.Controls.Add(bigpanel1);
            mainpanel.Tag = bigpanel1;
            bigpanel1.Show();
        }

        private void direction2_1_Click(object sender, EventArgs e)
        {
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }
            //
            if(bigpanel5 != null && bigpanel5.Visible)
            {
                bigpanel5.Hide();
            }

            if (bigpanel5_2 != null && bigpanel5_2.Visible)
            {
                DialogResult result = MessageBox.Show("Do you want to undo the changes?", "Directing to quiz attempt page", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return;
                }

                currentquiz.PendingList.Clear();
                bigpanel5_2.Hide();
            }
            //
            slash2.Hide();
            slash3.Hide();
            direction2.ForeColor = SystemColors.ControlText;
            direction2.Cursor = Cursors.Default;
            direction2.Click -= new EventHandler(this.direction2_1_Click);
            direction3.Click -= new EventHandler(this.direction3_Click);
            direction3.Hide();
            direction4.Hide();
            //

            mainpanel.Controls.Add(bigpanel4);
            mainpanel.Tag = bigpanel4;
            bigpanel4.Show();
        }

        private void direction3_Click(object sender, EventArgs e)
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
            currentquiz.PendingList.Clear();
            bigpanel5_2.Hide();
            //
            slash3.Hide();
            direction3.ForeColor = SystemColors.ControlText;
            direction3.Cursor = Cursors.Default;
            direction4.Hide();
            direction3.Click -= new EventHandler(this.direction3_Click);
            //
            mainpanel.Controls.Add(bigpanel5);
            mainpanel.Tag = bigpanel5;
            bigpanel5.Show();
        }


    }
}
