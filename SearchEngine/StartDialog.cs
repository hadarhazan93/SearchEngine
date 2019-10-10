using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

/**
 * main window controller - defines all UI functions and used to start and controll flow of threads in the calculation process
 */
namespace SearchEngine
{
    public partial class StartDialog : Form
    {
        //static variables for inter-thread messaging
        public static int progrogress;
        public static int maxProgress;
        public static bool calculationsFinished;
        public static bool readFinished;
        public static bool writeFinished;
        public static string[] cityCheckedByUser;
        public static bool stemming;
        public static String indexPath;
        public static String stopWordsPath;
        public static String corpusFolder;
        public static Mutex qLok = new Mutex();
        private bool ready = false;
        private bool readyQueries = false;
        private bool complete = false;
        private String path_to_queries_file = "";
        //language collection
        private String[] languages = { "English", "Mandarin", "Spanish", "Hindi", "Arabic", "Portuguese", "Bengali", "Russian", "Japanese", "Punjabi", "German", "Javanese", "Wu", "Malay", "Telugu", "Vietnamese", "Korean", "French", "Marathi", "Tamil", "Urdu", "Turkish", "Italian", "Yue", "Thai", "Gujarati", "Jin", "Southern Min", "Persian", "Polish", "Pashto", "Kannada", "Xiang", "Malayalam", "Sundanese", "Hausa", "Odia", "Burmese", "Hakka", "Ukrainian", "Bhojpuri", "Tagalog", "Yoruba", "Maithili", "Uzbek", "Sindhi", "Amharic", "Fula", "Romanian", "Oromo", "Igbo", "Azerbaijani", "Awadhi", "Gan", "Cebuano", "Dutch", "Kurdish", "Serbo-Croatian", "Malagasy", "Saraiki", "Nepali", "Sinhalese", "Chittagonian", "Zhuang", "Khmer", "Turkmen", "Assamese", "Madurese", "Somali", "Marwari", "Magahi", "Haryanvi", "Hungarian", "Chhattisgarhi", "Greek", "Chewa", "Deccan", "Akan", "Kazakh", "Northern Min", "Sylheti", "Zulu", "Czech", "Kinyarwanda", "Dhundhari", "Haitian Creole", "Eastern Min", "Ilocano", "Quechua", "Kirundi", "Swedish", "Hmong", "Shona", "Uyghur", "Hiligaynon/Ilonggo", "Mossi", "Xhosa", "Belarusian", "Balochi", "Konkani" };
        private Stopwatch runtime;

        public StartDialog()
        {
            city c = new city();
            String[] city = c.returnCities();
            InitializeComponent();
            this.complete = false;
            StartDialog.calculationsFinished = false;
            StartDialog.readFinished = false;
            StartDialog.writeFinished = false;
            for(int i = 0; i < this.languages.Length; i++)
            {
                this.language_selector.Items.Insert(i, this.languages[i]);
            }
            this.language_selector.SelectedIndex = 0;

            runtime = new Stopwatch();
        }
        /**
         * setter of path to stop words file
         */
        private void set_path_words_Click(object sender, EventArgs e)
        {
            OpenFileDialog choofdlog = new OpenFileDialog();
            choofdlog.Filter = "Text Files (*.txt*)|*.*";
            choofdlog.FilterIndex = 1;

            choofdlog.Multiselect = true;
            DialogResult result = choofdlog.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(choofdlog.FileName))
            {
                String path = "";
                if (choofdlog.FileName.Length > 60)
                    path = choofdlog.FileName.Substring(0, 16) + "..." + choofdlog.FileName.Substring(choofdlog.FileName.Length - 35);
                else
                    path = choofdlog.FileName;
                this.path_to_stopfile_show.Text = path;
                StartDialog.stopWordsPath = path;
            }
            if (this.path_to_stopfile_show.Text != "" && this.path_to_corpus_show.Text != "" && this.path_to_index_show.Text != "")
            {
                this.ProgressLabel.Text = "Ready";
                this.ready = true;
            }
        }
        /**
        * setter of path to corpus root folder
        */
        private void set_path_corpus_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    this.path_to_corpus_show.Text = fbd.SelectedPath;
                StartDialog.corpusFolder = fbd.SelectedPath;

            }
            if (this.path_to_stopfile_show.Text != "" && this.path_to_corpus_show.Text != "" && this.path_to_index_show.Text != "")
            {
                this.ProgressLabel.Text = "Ready";
                this.ready = true;
            }
        }
        /**
        * setter of path to index root folder
        * !!!root folder should be empty otherwise erase button will delete all contents of the file
        */
        private void set_path_index_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    this.path_to_index_show.Text = fbd.SelectedPath;
                StartDialog.indexPath = fbd.SelectedPath;

            }
            if (this.path_to_stopfile_show.Text != "" && this.path_to_corpus_show.Text != "" && this.path_to_index_show.Text != "")
            {
                this.ProgressLabel.Text = "Ready";
                this.ready = true;
            }
        }
        /**
         * unused function for testing puposes
         */
        private void RunSingleThread()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            FileReader fr = new FileReader(this.path_to_corpus_show.Text);
            Parser pr = new Parser(this.path_to_stopfile_show.Text, true);
            StemmingSequence stemmer = new StemmingSequence();
            Indexer ind = new Indexer(this.path_to_index_show.Text);
            while (fr.hasNext())
            {
                Document[] docAr = fr.GetNextDocuments();
                for (int i = 0; i < docAr.Length; i++)
                {
                    Token[] parseResult = pr.processDoc(docAr[i]);
                    if (this.doStemming.Checked)
                        parseResult = stemmer.StemTokens(parseResult);
                    ind.ProcessBatch(parseResult);
                    Console.WriteLine(docAr[i].id);
                }
            }
            watch.Stop();
            Console.WriteLine("Total time: " + watch.ElapsedMilliseconds);
        }
        /**
         * main indexing process starting button - creates and runs all threads
         */
        private async void start_button_Click(object sender, EventArgs e)
        {
            this.complete = false;
            StartDialog.calculationsFinished = false;
            StartDialog.readFinished = false;
            StartDialog.writeFinished = false;
            DocumentIndex.countryList = new Dictionary<string, string>();
            this.runtime.Stop();
            this.runtime.Reset();
            StartDialog.stemming = this.doStemming.Checked;
            if (!this.ready)
            {
                this.ProgressLabel.Text = "Please insert all paths!";
                return;
            }
            ConcurrentQueue<Token[]> tokenQ = new ConcurrentQueue<Token[]>();
            ConcurrentQueue<Document[]> docQ = new ConcurrentQueue<Document[]>();
            int progr = 0;
            this.ProgressLabel.Text = "Performing task...";
            Thread t1 = new Thread(new ThreadStart(() =>
            {
                Task.Run(() => this.ReadTask(this.path_to_corpus_show.Text, docQ));
            }));
            t1.Start();
            Thread t2 = new Thread(new ThreadStart(() =>
            {
                Task.Run(() => this.CalculationTask(this.path_to_stopfile_show.Text, docQ, tokenQ, this.doStemming.Checked));
            }));
            t2.Start();
            Thread t3 = new Thread(new ThreadStart(() =>
            {
                Task.Run(() => this.CalculationTask(this.path_to_stopfile_show.Text, docQ, tokenQ, this.doStemming.Checked));
            }));
            t3.Start();
            Thread t5 = new Thread(new ThreadStart(() =>
            {
                Task.Run(() => this.WriteTask(this.path_to_index_show.Text, tokenQ));
            }));
            t5.Start();
            this.runtime.Start();
            /*new Thread(new ThreadStart(() =>
            {
                while (!StartDialog.calculationsFinished || !StartDialog.readFinished || !StartDialog.writeFinished)
                {
                    this.BeginInvoke(new InvokeDelegate(updateTimer));
                    Thread.Sleep(500);
                }
            })).Start();*/

        }
        /**
         * function used to create calculation threads
         * executes all IO intencive tasks such as parsing and stemming
         * mid pipeline thread
         */
        private void CalculationTask(String stopWordsPath, ConcurrentQueue<Document[]> docQ, ConcurrentQueue<Token[]> tokenQ, Boolean doStemming)
        {
            Parser parser = new Parser(stopWordsPath, true);
            StemmingSequence stemmer = new StemmingSequence();
            while (!docQ.IsEmpty || !StartDialog.readFinished)
            {
                if (docQ.Count > 1000)
                    Thread.Sleep(500);
                Document[] docAr;
                StartDialog.qLok.WaitOne();
                bool ok = docQ.TryDequeue(out docAr);
                StartDialog.qLok.ReleaseMutex();
                if (!ok)
                    continue;
                for (int i = 0; i < docAr.Length; i++)
                {
                    Token[] parseResult = parser.processDoc(docAr[i]);
                    if (doStemming)
                        parseResult = stemmer.StemTokens(parseResult);
                    tokenQ.Enqueue(parseResult);
                    Console.WriteLine("Doc done :" + docAr[i].id);
                }
            }
            Console.WriteLine("Processing Complete");
            StartDialog.calculationsFinished = true;

        }
        /**
         * function to create read task - that reads from corpus and creates Document type array from each file
         * function is IO bound
         * pipeline start thread
         */
        private void ReadTask(String corpusPath, ConcurrentQueue<Document[]> docQ)
        {
            FileReader fr = new FileReader(corpusPath);
            while (fr.hasNext())
            {
                Document[] docs = fr.GetNextDocuments();
                docQ.Enqueue(docs);
                StartDialog.progrogress++;
                Thread.Sleep(50);
            }
            StartDialog.readFinished = true;
            Console.WriteLine("Read complete!");
        }
        /**
         * function to delegate tasks i.e. execute UI commands from other threads
         */
        private delegate void InvokeDelegate();
        /**
         * function to create a write thread which writes all posting files, in end of indexing process executes index optimization
         * function is IO bound
         * pipeline end
         */
        private void WriteTask(String indexPath, ConcurrentQueue<Token[]> tokenQ)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Indexer ind = new Indexer(indexPath);

            while (!tokenQ.IsEmpty || !StartDialog.calculationsFinished || !StartDialog.readFinished)
            {
                Token[] tokAr;
                bool ok = tokenQ.TryDequeue(out tokAr);
                if (!ok)
                    continue;
                ind.ProcessBatch(tokAr);
            }
            Console.WriteLine("Write complete!");
            this.BeginInvoke(new InvokeDelegate(optimizing));
            ind.endSession();
            this.BeginInvoke(new InvokeDelegate(updateProgress));
            StartDialog.writeFinished = true;
        }
        /**
         * function so other threads could update UI status label to "finished"
         */
        private void updateProgress()
        {
            this.ProgressLabel.Text = "Finished";
        }
        /**
         * function so other threads could update timer label
         */
        private void updateTimer()
        {
            TimeSpan current = this.runtime.Elapsed;
            this.time.Text = ""+(int)current.TotalMinutes +":"+ (int)current.TotalSeconds%60;
        }
        /**
         * index erasing button function
         * path should be choosen beforehand
         */
        private void empty_Index_Click(object sender, EventArgs e)
        {
            if (this.path_to_index_show.Text == "")
            {
                this.ProgressLabel.Text = "No path to index!";
                return;
            }
            EmptyFolder(new DirectoryInfo(this.path_to_index_show.Text));
            this.ProgressLabel.Text = "Erasing Complete";
        }
        /**
        * function to empty the contents of given index folder
        */
        private void EmptyFolder(DirectoryInfo directoryInfo)
        {
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
            {
                Directory.Delete(subfolder.FullName, true);
            }
        }
        /**
         *function so other threads could update UI status label to "optimizing"
         */
        private void optimizing()
        {
            this.ProgressLabel.Text = "Optimizing...";
        }
        /**
         * button to open dictionary viewer dialog
         */
        private void view_dictionary_Click(object sender, EventArgs e)
        {
            if (this.path_to_index_show.Text == "")
            {
                this.ProgressLabel.Text = "No path to index!";
                return;
            }
            String[] expectedDirectionaryLoc = { path_to_index_show.Text + "\\postings\\dictionary_non_stemmed.txt", path_to_index_show.Text + "\\postings\\dictionary_stemmed.txt" };
            DictionaryDialog form = new DictionaryDialog(expectedDirectionaryLoc);
            form.Show();
        }
        /**
        * button to close the application
        */
        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void language_selector_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ProgressLabel_Click(object sender, EventArgs e)
        {

        }
        // run query
        private void button1_Click(object sender, EventArgs e)
        {
            if (!this.checkVariableIntegrity())
                return;
            Searcher search = new Searcher(this.doStemming.Checked);
            List<String> resultsFromSearch;
            //public List<String> semanticSearch(String query)
            if (this.checkBoxSemantic.Checked)
            {
                resultsFromSearch = search.semanticSearch(queryText.Text,"", cityCheckedByUser);

            }
            //public List<String> regularSearch(String query)
            else
            {
                resultsFromSearch = search.regularSearch(queryText.Text, "", cityCheckedByUser);
                String toPrint = "";
                for (int i = 0; i < resultsFromSearch.Count(); i++)
                {

                    toPrint = toPrint + "000 " + resultsFromSearch[i] + " 1 0 r" + "\n";
                }
                SearchResults form = new SearchResults(toPrint);
                form.Show();
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!this.checkVariableIntegrity())
                return;
            if (readyQueries)
            {
                String toPrint = "";
                Searcher search = new Searcher(this.doStemming.Checked);
                Dictionary<String, List<String>> dicOfMultipleSearch = search.multipleSearch(path_to_queries_file, this.checkBoxSemantic.Checked, cityCheckedByUser);
                string[] s = dicOfMultipleSearch.Keys.ToArray();
                for (int i =0;i< s.Length; i++)
                {
                    for (int j = 0; j < dicOfMultipleSearch[s[i]].Count; j++)
                        toPrint = toPrint + Int32.Parse(s[i]) + " 0 " + dicOfMultipleSearch[s[i]][j] + " 1 0 r" + "\n";
                }

                SearchResults form = new SearchResults(toPrint);
                form.Show();
            }

        }
        // choose path to get queries
        private void pathQueries_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Text Files (*.txt*)|*.*";
            fileDialog.FilterIndex = 1;

            fileDialog.Multiselect = true;
            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fileDialog.FileName))
            {
                if (fileDialog.FileName.Length > 60)
                    this.path_to_queries_file = fileDialog.FileName/*.Substring(0, 16) + "..." + fileDialog.FileName.Substring(fileDialog.FileName.Length - 35)*/;
                else
                    this.path_to_queries_file = fileDialog.FileName;
            }
            if (this.path_to_queries_file != "")
            {
                this.readyQueries = true;
            }
        }

        private void textBox_InputDocToSearchEntities_TextChanged(object sender, EventArgs e)
        {

        }

        private void searchEntities_Click(object sender, EventArgs e)
        {
            if (!this.checkVariableIntegrity())
                return;
            if (textBox_InputDocToSearchEntities.Text == null || textBox_InputDocToSearchEntities.Text == "") { }
            else
            {
                EntitiesSearcher entitiesSearcher = new EntitiesSearcher();
                String[] max5Entities = entitiesSearcher.get5maxEntities(textBox_InputDocToSearchEntities.Text);
                ViewEntities form = new ViewEntities(max5Entities);
                form.Show();
            }
        }

        private void saveResultsInFile_Click(object sender, EventArgs e)
        {

        }

        private void doStemming_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            CountrySelectDialog form = new CountrySelectDialog();
            form.Show();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool checkVariableIntegrity()
        {
            if (this.path_to_stopfile_show.Text != "" && this.path_to_corpus_show.Text != "" && this.path_to_index_show.Text != "")
            {
                this.ProgressLabel.Text = "Ready";
                return true;
            }
            else
            {
                this.ProgressLabel.Text = "Please provide path";
                return false;
            }
        }
    }
}
