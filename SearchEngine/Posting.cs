using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
/**
 * class for preliminary storage of inverse index files saves all term in files names by first two symbols of the term
 */
namespace SearchEngine
{
    class Posting
    {
        private String rootPath;
        private int termCount = 0;
        public Dictionary<String, List<Token>> termBuffer;
        public Posting(string rootFolder)
        {
            //fixes adress so no collisions will occur if stemmed and non stem index built sequentially
            this.termBuffer = new Dictionary<string, List<Token>>();
            if (!Directory.Exists(rootPath + "\\postings"))
                System.IO.Directory.CreateDirectory(rootPath + "\\postings");
            this.rootPath = rootFolder+"\\postings";
            if (StartDialog.stemming)
            {
                if (!Directory.Exists(this.rootPath + "\\stemmed"))
                    System.IO.Directory.CreateDirectory(rootPath + "\\stemmed");
                this.rootPath = this.rootPath + "\\stemmed";
            }
            else
            {
                if (!Directory.Exists(this.rootPath + "\\non_stemmed"))
                    System.IO.Directory.CreateDirectory(rootPath + "\\non_stemmed");
                this.rootPath = this.rootPath + "\\non_stemmed";
            }

        }
        /**
         * wrapper function to process whole batch
         */
        public void ProcessTokens(Token[] tokens)
        {
            for(int i = 0; i < tokens.Length; i++)
            {
                this.ProcessToken(tokens[i]);
            }
        }
        /**
         * stores the term in memory if term amount didn`t achieve specified threshold
         */
        public void ProcessToken(Token token)
        {
            if (token.Value == null)
                return;
            if (token.Value.Length == 1)
                return;
            if (!this.termBuffer.ContainsKey(token.Value.Substring(0, 2)))
            {
                List<Token> list = new List<Token>();
                this.termBuffer.Add(token.Value.Substring(0, 2), list);
            }
            this.termBuffer[token.Value.Substring(0,2)].Add(token);
            this.termCount++;
            if (this.termCount == 10000000)
            {
                this.flushBuffer(this.termBuffer);
                this.termBuffer = new Dictionary<string, List<Token>>();
                this.termCount = 0;
            }
        }
        /**
         * if threshold amount of storage acheved all terms stored in memory flushed to disk
         */
        public void flushBuffer(Dictionary<String, List<Token>> tf)
        {
            Console.WriteLine("____________________________flush running_______________________________________");
            String[] keys = tf.Keys.ToArray();
            for(int i = 0; i < keys.Length; i++)
            {
                List<Token> tokens = tf[keys[i]];
                keys[i] = keys[i].ToLower();
                if (!System.IO.Directory.Exists(rootPath+ "\\"+keys[i][0]))
                    System.IO.Directory.CreateDirectory(rootPath + "\\" + keys[i][0]);
                StringBuilder fileContents = new StringBuilder();
                for (int j = 0; j < tokens.Count; j++)
                {
                    fileContents.Append(tokens[j].Value.ToLower() + ':' + tokens[j].docID + ';' + tokens[j].location+'|');
                }
                StreamWriter writer = File.AppendText(rootPath + "\\" + keys[i][0] + "\\" + keys[i].Substring(0, 2) + ".txt");
                writer.Write(fileContents.ToString());
                writer.Close();
            }
            Console.WriteLine("____________________________flush complete_______________________________________");

        }
    }
}
