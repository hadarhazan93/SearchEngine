namespace SearchEngine
{
    partial class StartDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.start_button = new System.Windows.Forms.Button();
            this.set_path_words = new System.Windows.Forms.Button();
            this.set_path_corpus = new System.Windows.Forms.Button();
            this.set_path_index = new System.Windows.Forms.Button();
            this.path_to_stopfile_show = new System.Windows.Forms.Label();
            this.path_to_corpus_show = new System.Windows.Forms.Label();
            this.path_to_index_show = new System.Windows.Forms.Label();
            this.doStemming = new System.Windows.Forms.CheckBox();
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.empty_Index = new System.Windows.Forms.Button();
            this.language_selector = new System.Windows.Forms.ComboBox();
            this.time = new System.Windows.Forms.Label();
            this.view_dictionary = new System.Windows.Forms.Button();
            this.exit = new System.Windows.Forms.Button();
            this.run_query = new System.Windows.Forms.Button();
            this.queryText = new System.Windows.Forms.TextBox();
            this.pathQueries = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBoxSemantic = new System.Windows.Forms.CheckBox();
            this.textBox_InputDocToSearchEntities = new System.Windows.Forms.TextBox();
            this.searchEntities = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // start_button
            // 
            this.start_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(61)))));
            this.start_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.start_button.Font = new System.Drawing.Font("Century Gothic", 12.25F);
            this.start_button.ForeColor = System.Drawing.SystemColors.GrayText;
            this.start_button.Location = new System.Drawing.Point(753, 245);
            this.start_button.Name = "start_button";
            this.start_button.Size = new System.Drawing.Size(93, 41);
            this.start_button.TabIndex = 1;
            this.start_button.Text = "Start";
            this.start_button.UseVisualStyleBackColor = false;
            this.start_button.Click += new System.EventHandler(this.start_button_Click);
            // 
            // set_path_words
            // 
            this.set_path_words.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(61)))));
            this.set_path_words.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.set_path_words.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.set_path_words.ForeColor = System.Drawing.SystemColors.GrayText;
            this.set_path_words.Location = new System.Drawing.Point(703, 47);
            this.set_path_words.Name = "set_path_words";
            this.set_path_words.Size = new System.Drawing.Size(206, 31);
            this.set_path_words.TabIndex = 2;
            this.set_path_words.Text = "Path to Stop Words";
            this.set_path_words.UseVisualStyleBackColor = false;
            this.set_path_words.Click += new System.EventHandler(this.set_path_words_Click);
            // 
            // set_path_corpus
            // 
            this.set_path_corpus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(61)))));
            this.set_path_corpus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.set_path_corpus.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.set_path_corpus.ForeColor = System.Drawing.SystemColors.GrayText;
            this.set_path_corpus.Location = new System.Drawing.Point(703, 100);
            this.set_path_corpus.Name = "set_path_corpus";
            this.set_path_corpus.Size = new System.Drawing.Size(206, 33);
            this.set_path_corpus.TabIndex = 3;
            this.set_path_corpus.Text = "Path to Corpus Folder";
            this.set_path_corpus.UseVisualStyleBackColor = false;
            this.set_path_corpus.Click += new System.EventHandler(this.set_path_corpus_Click);
            // 
            // set_path_index
            // 
            this.set_path_index.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(61)))));
            this.set_path_index.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.set_path_index.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.set_path_index.ForeColor = System.Drawing.SystemColors.GrayText;
            this.set_path_index.Location = new System.Drawing.Point(703, 154);
            this.set_path_index.Name = "set_path_index";
            this.set_path_index.Size = new System.Drawing.Size(206, 33);
            this.set_path_index.TabIndex = 4;
            this.set_path_index.Text = "Path to Index Location";
            this.set_path_index.UseVisualStyleBackColor = false;
            this.set_path_index.Click += new System.EventHandler(this.set_path_index_Click);
            // 
            // path_to_stopfile_show
            // 
            this.path_to_stopfile_show.AutoSize = true;
            this.path_to_stopfile_show.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.path_to_stopfile_show.ForeColor = System.Drawing.SystemColors.GrayText;
            this.path_to_stopfile_show.Location = new System.Drawing.Point(305, 56);
            this.path_to_stopfile_show.Name = "path_to_stopfile_show";
            this.path_to_stopfile_show.Size = new System.Drawing.Size(0, 17);
            this.path_to_stopfile_show.TabIndex = 5;
            // 
            // path_to_corpus_show
            // 
            this.path_to_corpus_show.AutoSize = true;
            this.path_to_corpus_show.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.path_to_corpus_show.ForeColor = System.Drawing.SystemColors.GrayText;
            this.path_to_corpus_show.Location = new System.Drawing.Point(305, 110);
            this.path_to_corpus_show.Name = "path_to_corpus_show";
            this.path_to_corpus_show.Size = new System.Drawing.Size(0, 17);
            this.path_to_corpus_show.TabIndex = 6;
            // 
            // path_to_index_show
            // 
            this.path_to_index_show.AutoSize = true;
            this.path_to_index_show.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.path_to_index_show.ForeColor = System.Drawing.SystemColors.GrayText;
            this.path_to_index_show.Location = new System.Drawing.Point(305, 170);
            this.path_to_index_show.Name = "path_to_index_show";
            this.path_to_index_show.Size = new System.Drawing.Size(0, 17);
            this.path_to_index_show.TabIndex = 7;
            // 
            // doStemming
            // 
            this.doStemming.AutoSize = true;
            this.doStemming.Font = new System.Drawing.Font("Century Gothic", 10.25F);
            this.doStemming.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.doStemming.Location = new System.Drawing.Point(42, 100);
            this.doStemming.Name = "doStemming";
            this.doStemming.Size = new System.Drawing.Size(151, 23);
            this.doStemming.TabIndex = 8;
            this.doStemming.Text = "Perform Stemming";
            this.doStemming.UseVisualStyleBackColor = true;
            this.doStemming.CheckedChanged += new System.EventHandler(this.doStemming_CheckedChanged);
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.AutoSize = true;
            this.ProgressLabel.Font = new System.Drawing.Font("Century Gothic", 10.25F);
            this.ProgressLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ProgressLabel.Location = new System.Drawing.Point(714, 217);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(176, 19);
            this.ProgressLabel.TabIndex = 9;
            this.ProgressLabel.Text = "Please Choose File Paths";
            this.ProgressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ProgressLabel.Click += new System.EventHandler(this.ProgressLabel_Click);
            // 
            // empty_Index
            // 
            this.empty_Index.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(61)))));
            this.empty_Index.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.empty_Index.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.empty_Index.ForeColor = System.Drawing.SystemColors.GrayText;
            this.empty_Index.Location = new System.Drawing.Point(42, 217);
            this.empty_Index.Name = "empty_Index";
            this.empty_Index.Size = new System.Drawing.Size(224, 36);
            this.empty_Index.TabIndex = 11;
            this.empty_Index.Text = "Erase Index";
            this.empty_Index.UseVisualStyleBackColor = false;
            this.empty_Index.Click += new System.EventHandler(this.empty_Index_Click);
            // 
            // language_selector
            // 
            this.language_selector.BackColor = System.Drawing.SystemColors.GrayText;
            this.language_selector.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.language_selector.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.language_selector.FormattingEnabled = true;
            this.language_selector.Location = new System.Drawing.Point(41, 43);
            this.language_selector.Name = "language_selector";
            this.language_selector.Size = new System.Drawing.Size(224, 30);
            this.language_selector.TabIndex = 12;
            this.language_selector.SelectedIndexChanged += new System.EventHandler(this.language_selector_SelectedIndexChanged);
            // 
            // time
            // 
            this.time.AutoSize = true;
            this.time.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.time.Location = new System.Drawing.Point(569, 254);
            this.time.Name = "time";
            this.time.Size = new System.Drawing.Size(0, 22);
            this.time.TabIndex = 13;
            // 
            // view_dictionary
            // 
            this.view_dictionary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(61)))));
            this.view_dictionary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.view_dictionary.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.view_dictionary.ForeColor = System.Drawing.SystemColors.GrayText;
            this.view_dictionary.Location = new System.Drawing.Point(41, 167);
            this.view_dictionary.Name = "view_dictionary";
            this.view_dictionary.Size = new System.Drawing.Size(225, 36);
            this.view_dictionary.TabIndex = 14;
            this.view_dictionary.Text = "View Dictionary";
            this.view_dictionary.UseVisualStyleBackColor = false;
            this.view_dictionary.Click += new System.EventHandler(this.view_dictionary_Click);
            // 
            // exit
            // 
            this.exit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(61)))));
            this.exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exit.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.exit.ForeColor = System.Drawing.SystemColors.GrayText;
            this.exit.Location = new System.Drawing.Point(1028, 12);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(41, 45);
            this.exit.TabIndex = 15;
            this.exit.Text = "X";
            this.exit.UseVisualStyleBackColor = false;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // run_query
            // 
            this.run_query.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(61)))));
            this.run_query.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.run_query.Font = new System.Drawing.Font("Century Gothic", 12.25F);
            this.run_query.ForeColor = System.Drawing.SystemColors.GrayText;
            this.run_query.Location = new System.Drawing.Point(762, 366);
            this.run_query.Name = "run_query";
            this.run_query.Size = new System.Drawing.Size(154, 31);
            this.run_query.TabIndex = 16;
            this.run_query.Text = "Run query";
            this.run_query.UseVisualStyleBackColor = false;
            this.run_query.Click += new System.EventHandler(this.button1_Click);
            // 
            // queryText
            // 
            this.queryText.Location = new System.Drawing.Point(41, 366);
            this.queryText.Name = "queryText";
            this.queryText.Size = new System.Drawing.Size(715, 31);
            this.queryText.TabIndex = 17;
            this.queryText.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // pathQueries
            // 
            this.pathQueries.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(61)))));
            this.pathQueries.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pathQueries.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.pathQueries.ForeColor = System.Drawing.SystemColors.GrayText;
            this.pathQueries.Location = new System.Drawing.Point(41, 429);
            this.pathQueries.Name = "pathQueries";
            this.pathQueries.Size = new System.Drawing.Size(715, 45);
            this.pathQueries.TabIndex = 18;
            this.pathQueries.Text = "Path to browse queries file";
            this.pathQueries.UseVisualStyleBackColor = false;
            this.pathQueries.Click += new System.EventHandler(this.pathQueries_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(61)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Century Gothic", 12.25F);
            this.button1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.button1.Location = new System.Drawing.Point(762, 429);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(165, 45);
            this.button1.TabIndex = 19;
            this.button1.Text = "Run queries";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // checkBoxSemantic
            // 
            this.checkBoxSemantic.AutoSize = true;
            this.checkBoxSemantic.Font = new System.Drawing.Font("Century Gothic", 10.25F);
            this.checkBoxSemantic.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkBoxSemantic.Location = new System.Drawing.Point(41, 275);
            this.checkBoxSemantic.Name = "checkBoxSemantic";
            this.checkBoxSemantic.Size = new System.Drawing.Size(145, 23);
            this.checkBoxSemantic.TabIndex = 20;
            this.checkBoxSemantic.Text = "Semantic Search";
            this.checkBoxSemantic.UseVisualStyleBackColor = true;
            // 
            // textBox_InputDocToSearchEntities
            // 
            this.textBox_InputDocToSearchEntities.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.textBox_InputDocToSearchEntities.Location = new System.Drawing.Point(41, 493);
            this.textBox_InputDocToSearchEntities.Name = "textBox_InputDocToSearchEntities";
            this.textBox_InputDocToSearchEntities.Size = new System.Drawing.Size(303, 31);
            this.textBox_InputDocToSearchEntities.TabIndex = 21;
            this.textBox_InputDocToSearchEntities.TabStop = false;
            this.textBox_InputDocToSearchEntities.TextChanged += new System.EventHandler(this.textBox_InputDocToSearchEntities_TextChanged);
            // 
            // searchEntities
            // 
            this.searchEntities.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(61)))));
            this.searchEntities.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchEntities.Font = new System.Drawing.Font("Century Gothic", 12.25F);
            this.searchEntities.ForeColor = System.Drawing.SystemColors.GrayText;
            this.searchEntities.Location = new System.Drawing.Point(357, 493);
            this.searchEntities.Name = "searchEntities";
            this.searchEntities.Size = new System.Drawing.Size(533, 31);
            this.searchEntities.TabIndex = 22;
            this.searchEntities.Text = "Search Entities In Document";
            this.searchEntities.UseVisualStyleBackColor = false;
            this.searchEntities.Click += new System.EventHandler(this.searchEntities_Click);
            // 
            // StartDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(44)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(967, 552);
            this.Controls.Add(this.searchEntities);
            this.Controls.Add(this.textBox_InputDocToSearchEntities);
            this.Controls.Add(this.checkBoxSemantic);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pathQueries);
            this.Controls.Add(this.queryText);
            this.Controls.Add(this.run_query);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.view_dictionary);
            this.Controls.Add(this.time);
            this.Controls.Add(this.language_selector);
            this.Controls.Add(this.empty_Index);
            this.Controls.Add(this.ProgressLabel);
            this.Controls.Add(this.doStemming);
            this.Controls.Add(this.path_to_index_show);
            this.Controls.Add(this.path_to_corpus_show);
            this.Controls.Add(this.path_to_stopfile_show);
            this.Controls.Add(this.set_path_index);
            this.Controls.Add(this.set_path_corpus);
            this.Controls.Add(this.set_path_words);
            this.Controls.Add(this.start_button);
            this.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "StartDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "hafdfej";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button start_button;
        private System.Windows.Forms.Button set_path_words;
        private System.Windows.Forms.Button set_path_corpus;
        private System.Windows.Forms.Button set_path_index;
        private System.Windows.Forms.Label path_to_stopfile_show;
        private System.Windows.Forms.Label path_to_corpus_show;
        private System.Windows.Forms.Label path_to_index_show;
        private System.Windows.Forms.CheckBox doStemming;
        private System.Windows.Forms.Label ProgressLabel;
        private System.Windows.Forms.Button empty_Index;
        private System.Windows.Forms.ComboBox language_selector;
        private System.Windows.Forms.Label time;
        private System.Windows.Forms.Button view_dictionary;
        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.Button run_query;
        private System.Windows.Forms.TextBox queryText;
        private System.Windows.Forms.Button pathQueries;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBoxSemantic;
        private System.Windows.Forms.TextBox textBox_InputDocToSearchEntities;
        private System.Windows.Forms.Button searchEntities;
    }
}

