using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ExamApp
{
    public partial class ExamApp : System.Windows.Forms.Form
    {
        private Question currentquestion;
        private edit_form bigpanel1;
        private editquestion_form bigpanel2;
        private List<Question> questions;
        private Category defaultCategory;
        public ExamApp()
        {
            InitializeComponent();
            questions = new List<Question>();
            defaultCategory = new Category
            {
                Name = "Default",
                Info = "Default Category",
                Id = 0,
                QuestionList = questions,
                Child = new List<Category> { }
            };
        }

        private void closepopup_Click(object sender, EventArgs e)
        {
            this.popup.Hide();
        }

        private void functionbutton1_Click(object sender, EventArgs e)
        {
            if(functionbutton1.Visible == true)
            {
                Point point = headingElement.PointToScreen(Point.Empty);
                popup.Location = new System.Drawing.Point(point.X + headingElement.Width - popup.Width - 50, point.Y);
                popup.Show();
            }
        }

        private void openeditpanel(edit_form form)
        {
            bigpanel1 = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.DirectingButton1.Click += new System.EventHandler(this.createquestion_Click);
            form.SubcategoriesQ.CheckedChanged += new System.EventHandler(this.BP1checkbox1_CheckedChanged);
            mainpanel.Controls.Add(form);
            mainpanel.Tag = form;
            form.Show();
        }
        
        private void openeditquestionpanel(editquestion_form form)
        {
            bigpanel2 = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.FunctionButton1.Click += delegate (object sender, EventArgs e) { this.BP2savebutton1_Click(sender, e, this.currentquestion); };
            form.FunctionButton2.Click += new System.EventHandler(this.BP2cancelbutton_Click);
            form.FunctionButton3.Click += delegate (object sender, EventArgs e) { this.BP2savebutton2_Click(sender, e, this.currentquestion); };
            mainpanel.Controls.Add(form);
            mainpanel.Tag = form;
            form.Show();
        }

        private void questions1_Click(object sender, EventArgs e)
        {
            this.popup.Hide();
            this.functionbutton1.Hide();
            this.heading.Size = new System.Drawing.Size(1114, 86);
            if(mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }
            if(bigpanel1 == null)
            {
                openeditpanel(new edit_form());
            }
            else
            {
                mainpanel.Controls.Add(bigpanel1);
                mainpanel.Tag = bigpanel1;
                bigpanel1.Show();
            }
            slash1.Show();
            direction2.Show();
            direction1.Cursor = System.Windows.Forms.Cursors.Hand;
            direction1.ForeColor = Color.IndianRed;
            direction1.Click += new System.EventHandler(this.direction1_Click);
        }
        
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
                openeditquestionpanel(new editquestion_form());
            }
            else
            {
                bigpanel2.MainPanel.AutoScrollPosition = new Point(0, 0);
                bigpanel2.ChoiceP.AutoScrollPosition = new Point(0, 0);
                bigpanel2.MoreChoiceP.Hide();
                mainpanel.Controls.Add(bigpanel2);
                mainpanel.Tag = bigpanel2;
                bigpanel2.Show();
            }
            bigpanel2.BigLabel.Text = "Adding a Multiple choices question";
            bigpanel1.Hide();
            slash2.Show();
            direction3.Show();
            direction2.Cursor = System.Windows.Forms.Cursors.Hand;
            direction2.ForeColor = Color.IndianRed;
            direction2.Click += new System.EventHandler(this.direction2_Click);
        }

        private void direction1_Click(object sender, EventArgs e)
        {
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }
            this.bigpanel1.Hide();
            if (bigpanel2 != null)
            {
                this.bigpanel2.Hide();
            }
            this.heading.Size = new System.Drawing.Size(1114, 117);
            this.functionbutton1.Show();
            slash1.Hide();
            slash2.Hide();
            direction2.Hide();
            direction3.Hide();
            direction1.ForeColor = System.Drawing.Color.Black;
            direction1.Cursor = System.Windows.Forms.Cursors.Default;
        }
        
        private void direction2_Click(object sender, EventArgs e)
        {
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }
            bigpanel2.Hide();
            slash2.Hide();
            direction3.Hide();
            direction2.Click -= new System.EventHandler(this.direction2_Click);
            direction2.ForeColor = System.Drawing.Color.Black;
            direction2.Cursor = System.Windows.Forms.Cursors.Default;
            mainpanel.Controls.Add(bigpanel1);
            mainpanel.Tag = bigpanel1;
            bigpanel1.Show();
        }
        
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
                    Description = this.bigpanel2.QText.Text,
                    Mark = mark,
                    Choices = new List<Choice>(5) {null, null, null, null, null }
                };

                this.questions.Add(question);

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
                this.currentquestion.Description = this.bigpanel2.QText.Text;
                this.currentquestion.Mark = mark;

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
            slash2.Hide();
            direction3.Hide();
            direction2.Click -= new System.EventHandler(this.direction2_Click);
            direction2.ForeColor = System.Drawing.Color.Black;
            direction2.Cursor = System.Windows.Forms.Cursors.Default;
            mainpanel.Controls.RemoveAt(0);
            mainpanel.Controls.Add(bigpanel1);
            bigpanel1.Show();
        }

        private void BP2savebutton2_Click(object sender, EventArgs e, Question question)
        {
            if (String.IsNullOrWhiteSpace(this.bigpanel2.QName.Text))
            {
                MessageBox.Show("Question Name is empty");
                return ;
            }

            if (String.IsNullOrWhiteSpace(this.bigpanel2.QText.Text))
            {
                MessageBox.Show("Question Text is empty");
                return ;
            }

            if (String.IsNullOrWhiteSpace(this.bigpanel2.QMark.Text))
            {
                MessageBox.Show("Default Mark is empty");
                return ;
            }

            double mark;

            try
            {
                mark = Convert.ToDouble(this.bigpanel2.QMark.Text);
            }

            catch (FormatException)
            {
                MessageBox.Show("Please input a valid value for Default Mark!");
                return ;
            }

            if (question == null)
            {
                question = new Question
                {
                    Name = this.bigpanel2.QName.Text,
                    Description = this.bigpanel2.QText.Text,
                    Mark = mark,
                    Choices = new List<Choice>(5) { null, null, null, null, null }
                };

                this.questions.Add(question);

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
                this.currentquestion.Description = this.bigpanel2.QText.Text;
                this.currentquestion.Mark = mark;

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

        private void BP2cancelbutton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("You will lose what you've typed. Are you sure?", "Cancel", MessageBoxButtons.YesNo);
            if(result == DialogResult.No)
            {
                return ;
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
            bigpanel1.Show();
        }

        private void BP1checkbox1_CheckedChanged(object sender, EventArgs e)
        {
            if(this.bigpanel1.QBox.Visible == false)
            {
                this.bigpanel1.QBox.Controls.Clear();
                foreach (Question question in this.questions)
                {
                    QuestionBlock temp = new QuestionBlock(question);
                    this.bigpanel1.QBox.Controls.Add(temp);
                    temp.Edit.Click += delegate (object sender1, EventArgs e1) { this.BP1editlabel_Click(sender1, e1, temp.Question); };
                }
                this.bigpanel1.QBox.Visible = true;
            }
            else
            {
                this.bigpanel1.QBox.Visible = false;
            }
        }

        private void BP1editlabel_Click(object sender, EventArgs e, Question question)
        {
            if(mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            currentquestion = question;
            bigpanel2.QName.Text = question.Name;
            bigpanel2.QText.Text = question.Description;
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
                        if(choice.Grade != 0)
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

            slash2.Show();
            direction3.Show();
            direction2.Cursor = System.Windows.Forms.Cursors.Hand;
            direction2.ForeColor = Color.IndianRed;
            direction2.Click += new System.EventHandler(this.direction2_Click);
            bigpanel1.Hide();
            bigpanel2.BigLabel.Text = "Editing a Multiple choices question";
            bigpanel2.MainPanel.AutoScrollPosition = new Point(0, 0);
            bigpanel2.ChoiceP.AutoScrollPosition = new Point(0, 0);
            mainpanel.Controls.Add(bigpanel2);
            mainpanel.Controls.Add(bigpanel2);
            bigpanel2.Show();
        }
    }
}
