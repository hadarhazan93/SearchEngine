using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    class Dictionary
    {
        public static int numbercount = 00;
        public String rootPath;
        public HashSet<String> termList;
        public HashSet<String> capitalized;
        private DocumentIndex dci;
        public Dictionary(String path)
        {
            this.rootPath = path;
            this.termList = new HashSet<string>();
            this.capitalized = new HashSet<string>();
            this.dci = new DocumentIndex(path + "\\postings");
        }
        //adds terms to dictionary if they not exist
        public void ProcessTokens(Token[] tokens)
        {
            if (tokens.Length == 0)
                return;
            String docID = tokens[0].docID;
            for(int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i].Value == null)
                    continue;
                if (!this.capitalized.Contains(tokens[i].Value))
                    this.capitalized.Add(tokens[i].Value);
                if (!this.termList.Contains(tokens[i].Value))
                    this.termList.Add(tokens[i].Value.ToLower());
            }
            string maxTerm = "";
            int[] docData = this.countDictinctAndMax(tokens, out maxTerm);
            this.dci.addToList(docID, docData[0], docData[1],docData[2],maxTerm);
        }
        public void endSession()
        {
            this.dci.endSession();
            FileReader.docPaths = new Dictionary<string, string>();

        }
        private int[] countDictinctAndMax(Token[] arr,out String term)
        {
            int[] res = new int[3];
            Dictionary<Token, int> occuranceCount = new Dictionary<Token, int>();
            HashSet<Token> occured = new HashSet<Token>();
            int maxTF = 1;
            term = "";
            for (int i = 0; i < arr.Length; i++)
            {
                if (!occured.Contains(arr[i]))
                {
                    occured.Add(arr[i]);
                    occuranceCount.Add(arr[i], 1);
                }
                else
                {
                    occuranceCount[arr[i]] += 1;
                    if (occuranceCount[arr[i]] > maxTF)
                    {
                        maxTF = occuranceCount[arr[i]];
                        term = arr[i].Value;
                    }
                }
            }
            res[0] = maxTF;
            res[1] = occured.Count();
            int[] vectorValues = occuranceCount.Values.ToArray();
            int sum = 0;
            for(int i = 0; i < vectorValues.Length; i++)
            {
                sum = sum + (vectorValues[i] * vectorValues[i]);
            }
            res[2] = (int)Math.Sqrt(sum);
            return res;
        }

    }
}
