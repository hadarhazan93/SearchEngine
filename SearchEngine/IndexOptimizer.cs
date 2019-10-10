using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
/**
 * class used for optimizing index after a run
 */
namespace SearchEngine
{
    class IndexOptimizer
    {
        public Dictionary<string,int[]> dict;
        private String rootPath;
        public IndexOptimizer(Dictionary d)
        {
            this.rootPath = d.rootPath;
            this.dict = d.termList.ToDictionary(h => h, h => new int[2]);
        }

        public void RestructureFiles(String[] paths)
        {
            for(int i = 0; i < paths.Length; i++)
            {
                this.restructureFile(paths[i]);
            }
            //message to indexer that thread is complete
            Indexer.opt_cnt++;
        }
        /**
         * rewrites a single file to comply with format:
         * [term1]:
         * [document number];[term location];...;[term location]
         * ...
         * ...
         * [term2]:
         * ...
         */
        public void restructureFile(String path)
        {
            String content = File.ReadAllText(path);
            char[] separator = { '|' };
            List<string> entries = content.Split(new string[] { "|" }, StringSplitOptions.None).ToList();
            entries.Sort();
            StringBuilder file = new StringBuilder();
            String currentWord = "";
            String currentDoc = "";
            int lineCounter = 0;
            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].Length == 0)
                    continue;
                String[] occurance = entries[i].Split(new string[] { ";", ":" }, StringSplitOptions.None);
                if (occurance[0].All(char.IsPunctuation))
                    continue;
                if (!occurance[0].Equals(currentWord))
                {
                    currentWord = occurance[0];
                    file.Append('\n' + currentWord);
                    this.dict[currentWord][1] = lineCounter;
                    lineCounter++;
                }
                if (!currentDoc.Equals(occurance[1])){
                    currentDoc = occurance[1];
                    file.Append('\n'+ currentDoc+':');
                    this.dict[currentWord][0]++;
                    lineCounter++;
                }
                file.Append(';' + occurance[2]);
            }
            File.WriteAllText(path, file.ToString().Substring(1));
        }


    }
}
