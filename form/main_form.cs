using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ExamApp
{
    public partial class ExamApp : System.Windows.Forms.Form
    {
        private edit_form bigpanel1;
        private editquestion_form bigpanel2;
        private quiz_form bigpanel3;
        private quiz_attempt_form bigpanel4;
        private Question currentquestion;
        private List<Category> categories;
        private Category currentcategory;
        private List<Quiz> quizs;
        private Quiz currentquiz;

        public ExamApp()
        {
            InitializeComponent();
            categories = new List<Category>
            {
                new Category(null, new List<Category>(), "Default", "Default category", -1, 0, new List<Question>())
            };
            quizs = new List<Quiz>();
        }

/* form opening */

        // function to open edit form
        private void openeditpanel(edit_form form, int index)
        {
            bigpanel1 = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            // Events
            form.DirectingButton1.Click += new System.EventHandler(this.createquestion_Click);
            form.SubcategoriesQ.CheckedChanged += new System.EventHandler(this.BP1checkbox1_CheckedChanged);
            form.addCategoryButton.Click += new System.EventHandler(this.BP1addcategory_Click);

            form.ImportPanel.DragEnter += this.importPanel_DragEnter;
            form.ImportPanel.DragDrop += this.importPanel_DragDrop;
            //
            // Category ComboBox in Questions tab
            //
            form.QCategory.SelectedIndexChanged += new System.EventHandler(this.qcategory1_SelectIndexChanged);
            form.QCategory.Items.Clear();
            form.QCategory.DataSource = categories;
            form.QCategory.DisplayMember = "NameAndGen";
            //
            // Category ComboBox in Categories tab
            //
            form.PCategory.SelectedIndexChanged += new System.EventHandler(this.pcategory1_SelectIndexChanged);
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
        private void openeditquestionpanel(editquestion_form form, Boolean show)
        {
            bigpanel2 = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            //
            // add events to buttons
            //
            form.FunctionButton1.Click += delegate (object sender, EventArgs e) { this.BP2savebutton1_Click(sender, e, this.currentquestion); };
            form.FunctionButton2.Click += new System.EventHandler(this.BP2cancelbutton_Click);
            form.FunctionButton3.Click += delegate (object sender, EventArgs e) { this.BP2savebutton2_Click(sender, e, this.currentquestion); };
            //
            // Category ComboBox
            //
            form.QCategory.SelectedIndexChanged += new System.EventHandler(this.qcategory2_SelectIndexChanged);
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
        private void openquizpanel(quiz_form form)
        {
            bigpanel3 = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            //
            form.CreateQuizButton.Click += new System.EventHandler(this.BP3createbutton_Click);
            form.CANCELbutton.Click += new System.EventHandler(this.BP3cancelbutton_Click);
            //
            mainpanel.Controls.Add(form);
            mainpanel.Tag = form;
            form.Show();
        }

        // function to open quiz attempt form
        private void openquizattemptpanel(quiz_attempt_form form)
        {
            bigpanel4 = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            //

            //
            mainpanel.Controls.Add(form);
            mainpanel.Tag = form;
            form.Show();
        }


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
            this.heading.Size = new System.Drawing.Size(1114, 86);

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if(bigpanel3 == null)
            {
                openquizpanel(new quiz_form());
            }
            else
            {
                mainpanel.Controls.Add(bigpanel3);
                mainpanel.Tag = bigpanel3;
                bigpanel3.Show();
            }
            direction1.Cursor = System.Windows.Forms.Cursors.Hand;
            direction1.ForeColor = Color.IndianRed;
            direction1.Click += new System.EventHandler(this.direction1_Click);
            slash1.Show();
            direction2.Text = "Adding a new quiz";
            direction2.ForeColor = System.Drawing.SystemColors.ControlText;
            direction2.Cursor = System.Windows.Forms.Cursors.Default;
            direction2.Show();
        }

        private void quizName_Click(object sender, EventArgs e, Quiz quiz)
        {
            button1.Hide();
            functionbutton1.Hide();
            popup.Hide();
            this.heading.Size = new System.Drawing.Size(1114, 86);

            currentquiz = quiz;

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (bigpanel4 == null)
            {
                openquizattemptpanel(new quiz_attempt_form());
            }
            else
            {
                mainpanel.Controls.Add(bigpanel4);
                mainpanel.Tag = bigpanel4;
                bigpanel4.Show();
            }

            direction1.Cursor = System.Windows.Forms.Cursors.Hand;
            direction1.ForeColor = Color.IndianRed;
            direction1.Click += new System.EventHandler(this.direction1_Click);
            direction2.ForeColor = System.Drawing.SystemColors.ControlText;
            direction2.Cursor = System.Windows.Forms.Cursors.Default;

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
        
        

/* edit form */

        // function to update questions display in questions tab in edit form
        private void updateQuestiondisplay(bool mode)
        {
            currentcategory = bigpanel1.QCategory.SelectedItem as Category;
            if (currentcategory != null)
            {
                if (mode)
                {
                    this.bigpanel1.QBox.Controls.Clear();
                    foreach (Question question in this.currentcategory.QuestionList)
                    {
                        QuestionBlock temp = new QuestionBlock(question);
                        this.bigpanel1.QBox.Controls.Add(temp);
                        temp.Edit.Click += delegate (object sender1, EventArgs e1) { this.BP1editlabel_Click(sender1, e1, temp.Question); };
                    }

                    foreach (Category subcategory in currentcategory.Child)
                    {
                        foreach (Question question in subcategory.QuestionList)
                        {
                            QuestionBlock temp = new QuestionBlock(question);
                            this.bigpanel1.QBox.Controls.Add(temp);
                            temp.Edit.Click += delegate (object sender1, EventArgs e1) { this.BP1editlabel_Click(sender1, e1, temp.Question); };
                        }
                    }
                }
                else
                {
                    if (currentcategory != null)
                    {
                        this.bigpanel1.QBox.Controls.Clear();
                        foreach (Question question in currentcategory.QuestionList)
                        {
                            QuestionBlock temp = new QuestionBlock(question);
                            this.bigpanel1.QBox.Controls.Add(temp);
                            temp.Edit.Click += delegate (object sender1, EventArgs e1) { this.BP1editlabel_Click(sender1, e1, temp.Question); };
                        }
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
                currentcategory = bigpanel1.QCategory.SelectedItem as Category;
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
            this.heading.Size = new System.Drawing.Size(1114, 86);

            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            if (bigpanel1 == null)
            {
                openeditpanel(new edit_form(), 0);
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
            direction1.Click += new System.EventHandler(this.direction1_Click);
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
                openeditpanel(new edit_form(), 1);
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
                openeditpanel(new edit_form(), 2);
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
                openeditquestionpanel(new editquestion_form(), true);
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
            updateQuestiondisplay(true);
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
                this.openeditquestionpanel(new editquestion_form(), false);
            }

            currentquestion = question;
            bigpanel2.QName.Text = question.Name;
            try
            {
                bigpanel2.QText.Rtf = question.Description;
            }
            catch (Exception ex)
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
                        bigpanel2.C1Text.Text = choice.Text;
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
                        bigpanel2.C2Text.Text = choice.Text;
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
                        bigpanel2.C3Text.Text = choice.Text;
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
                        bigpanel2.C4Text.Text = choice.Text;
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
                        bigpanel2.C5Text.Text = choice.Text;
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
            bigpanel2.ChoiceP.AutoScrollPosition = new Point(0, 0);
            mainpanel.Controls.Add(bigpanel2);
            mainpanel.Tag = bigpanel2;
            bigpanel2.Show();
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

        private void importPanel_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the dragged data contains file(s)
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get the array of dropped files
                string[] filepaths = (string[])e.Data.GetData(DataFormats.FileDrop);

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
                    if (fileSize > 10*1024*1024) // MAX 10MB
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

        private void importPanel_DragDrop(object sender, DragEventArgs e)
        {
            // Get the dropped data
            string[] filepaths = (string[])e.Data.GetData(DataFormats.FileDrop);

            if(filepaths.Length > 1)
            {
                MessageBox.Show("Please drop 1 file at once");
                return;
            }

            AikenFormat aikenFormat = new AikenFormat { FilePath = filepaths[0] };
            string extension = System.IO.Path.GetExtension(filepaths[0]);

            if(extension == ".txt")
            {
                aikenFormat.ReadTxt();
            }
            else
            {
                aikenFormat.ReadDocx();
            }

            if(aikenFormat == null)
            {
                MessageBox.Show("?Error!");
            }

            if(aikenFormat.ImportQuestion != null)
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
            else if(importedQuestionNum == 1)
            {
                MessageBox.Show("Successfully imported 1 question");
            }
            else
            {
                MessageBox.Show($"Successfully imported {importedQuestionNum} questions");
            }
        }

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
            this.heading.Size = new System.Drawing.Size(1114, 117);
            this.functionbutton1.Show();
            this.button1.Show();
            slash1.Hide();
            direction2.Hide();
            direction1.ForeColor = System.Drawing.SystemColors.ControlText;
            direction1.Cursor = System.Windows.Forms.Cursors.Default;
            direction1.Click -= new System.EventHandler(this.direction1_Click);
            //
            mainpanel.Controls.RemoveAt(0);
            updateQuizdisplay();
        }

        // cancel quiz button event
        private void BP3cancelbutton_Click(Object sender, EventArgs e)
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
            this.heading.Size = new System.Drawing.Size(1114, 117);
            this.functionbutton1.Show();
            this.button1.Show();
            slash1.Hide();
            direction2.Hide();
            direction1.ForeColor = System.Drawing.SystemColors.ControlText;
            direction1.Cursor = System.Windows.Forms.Cursors.Default;
            direction1.Click -= new System.EventHandler(this.direction1_Click);
            //
            mainpanel.Controls.RemoveAt(0);
        }

        

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

            this.heading.Size = new System.Drawing.Size(1114, 117);
            this.functionbutton1.Show();
            this.button1.Show();
            slash1.Hide();
            slash2.Hide();
            direction2.Hide();
            direction3.Hide();
            direction1.ForeColor = System.Drawing.SystemColors.ControlText;
            direction1.Cursor = System.Windows.Forms.Cursors.Default;
            direction1.Click -= new System.EventHandler(this.direction1_Click);
            mainpanel.Controls.Add(quizFlowLayout);
        }

        // direct to edit form when click the question bank label on top
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
            direction2.Click -= new System.EventHandler(this.direction2_Click);
            updateQuestiondisplay(bigpanel1.SubcategoriesQ.Checked);
            //
            mainpanel.Controls.Add(bigpanel1);
            mainpanel.Tag = bigpanel1;
            bigpanel1.Show();
        }


       
        
    }
}
