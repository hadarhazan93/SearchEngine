using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/**
 * a class to locate a term in dictionary and fetch the relevant posting section
 */
namespace SearchEngine
{
    class IndexSearcher
    {
        private String root;
        private String suffix;
        private Dictionary<String, int> dictionary= new Dictionary<String, int>();
        private Dictionary<String, String> cityIndex = new Dictionary<string, string>();
        private Dictionary<String, List<String>> cityIndexByCity = new Dictionary<string, List<string>>();
        private Dictionary<String, int> docVecPWIndex = new Dictionary<string, int>();
        private Dictionary<String, int> docLenghtIndex = new Dictionary<string, int>();
        public IndexSearcher(String path,bool stemmed)
        {
            this.root = path + "\\postings";
            if (!stemmed)
                suffix = "_non_stemmed.txt";
            else
                suffix = "_stemmed.txt";
            this.dictionaryToMemory();
            this.documentIndexToMemory();
        }
        /**
         * uploads the dictionary file to memory, includes only "term":"line in index"
         */
        private void dictionaryToMemory()
        {
            String content = File.ReadAllText(root + "\\dictionary" + suffix);
            String[] lines = content.Split('\n');
            for(int i=0; i < lines.Length; i++)
            {
                String[] buffer = lines[i].Split(';');
                if (buffer.Length > 1)
                {
                    this.dictionary.Add(buffer[0].ToLower(), Int32.Parse(buffer[2]));
                }

            }
        }
        /**
         * uploads document index to memory keeps map DOCID:City
         */
        private void documentIndexToMemory()
        {
            String content = File.ReadAllText(root + "\\documentIndex" + suffix);
            String[] lines = content.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                String[] buffer = lines[i].Split(';');
                if (buffer.Length > 1)
                {
                    this.cityIndex.Add(buffer[0], buffer[3]);
                    this.docVecPWIndex.Add(buffer[0], Int32.Parse(buffer[5]));
                    this.docLenghtIndex.Add(buffer[0], Int32.Parse(buffer[2]));
                }
            }
            String[] docs = this.cityIndex.Keys.ToArray();
            for(int i = 0; i < docs.Length; i++)
            {
                if (!this.cityIndexByCity.Keys.Contains(this.cityIndex[docs[i]]))
                    this.cityIndexByCity.Add(this.cityIndex[docs[i]], new List<string>());
                this.cityIndexByCity[this.cityIndex[docs[i]]].Add(docs[i]);
            }

        }
        /**
         * return term position in relevant index file
         */
        private int locatePostingPosition(String term)
        {
            if (!this.dictionary.ContainsKey(term))
                return -1;
            return this.dictionary[term];
        }
        /**
         * returns string array of index lines for given term
         */
        private String[] getPostingSection(String term, int line)
        {
            line++;
            List<String> res = new List<string>();
            String content = File.ReadAllText(root + "\\" + suffix.Substring(1,suffix.Length-5)+"\\"+char.ToLower(term[0])+"\\"+term.Substring(0,2).ToLower()+".txt");
            String[] lines = content.Split('\n');
            String current = lines[line];
            while (current.Contains(":;"))
            {
                res.Add(current);
                line++;
                current = lines[line];
            }
            return res.ToArray();
        }
        /**
         * simpler argument funtion uses previous two functions
         */
        private String[] getPostingSection(String term)
        {
            return this.getPostingSection(term, this.locatePostingPosition(term));

        }
        /**
         * builds a relation vector term->frequency in document vector values mapped with dictionary
         */
        private Dictionary<String,int> getVectorValues(String[] postingsList)
        {
            Dictionary<String, int> result = new Dictionary<string, int>();
            for(int i = 0; i < postingsList.Length; i++)
            {
                String[] buffer = postingsList[i].Split(';');
                String docID = buffer[0].Substring(0, buffer[0].Length - 1);
                result.Add(docID, buffer.Length - 1);
            }
            return result;
        }
        /**
         * returns a single vector DocId->TF for a given term
         */
        public Dictionary<String,int> getTermVector(String term)
        {
            return this.getVectorValues(this.getPostingSection(term));
        }
        /**
         * returns true if our existing dictionary contains given term
         */
        public bool termExists(String term)
        {
            return this.dictionary.ContainsKey(term)|| this.dictionary.ContainsKey(char.ToUpper(term[0])+term.Substring(1));
        }
        public int getDocumentVectorPW(String docID)
        {
            if (this.docVecPWIndex.ContainsKey(docID))
                return this.docVecPWIndex[docID];
            else
                return -1;
        }
        public int getDocCount()
        {
            return this.cityIndex.Count();
        }
        public int getDocLen(String docID)
        {
            return this.docLenghtIndex[docID];
        }
        public List<string> getCityByTag(String city)
        {
            if (this.cityIndexByCity.Keys.Contains(city))
                return this.cityIndexByCity[city];
            else
                return new List<string>();
        }
    }
}
