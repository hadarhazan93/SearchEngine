using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    class EntitiesSearcher
    {
        List<String> entities;

        public EntitiesSearcher()
        {
            Entities e = new Entities();
            entities = e.getEntities();
        }
        public String[] get5maxEntities(String docName)
        {
            String[] max5entities = new String[5];
            Dictionary<String, int> dicEntities = getEntities(docName);
            String[] allEntities = dicEntities.Keys.ToArray();
            for (int w=0; w<5; w++)
            {
                String key = "";
                int min = 0;
                for (int i = 0; i < dicEntities.Keys.Count(); i++)
                {
                    if (dicEntities[allEntities[i]] > min)
                    {
                        min = dicEntities[allEntities[i]];
                        key = allEntities[i];
                        dicEntities[allEntities[i]] = -1;
                    }
                }
                max5entities[w] = key;
            }
            return max5entities;
        }
        // FBIS3-1;19;321;NONE;percent;44;D:\corpus\FB396001\FB396001
        public Dictionary<String, int> getEntities(String docName)
        {
            bool flag = true;
            int i = 0;
            String path = "";
            Dictionary<String, int> toReturn = new Dictionary<String, int>();
            HashSet<String> entitesHash = new HashSet<string>(entities);
            String documentIndex_non_stemmed = File.ReadAllText(@"C:\a\results\postings\documentIndex_non_stemmed.txt");
            String[] documentIndex_non_stemmed_Splited = documentIndex_non_stemmed.Split('\n');
            while (flag)
            {
                String[] splitedLine = documentIndex_non_stemmed_Splited[i].Split(';');
                if (splitedLine[0].Equals(docName))
                {
                    flag = false;
                    path = splitedLine[splitedLine.Length-1];
                }
                i++;
            }
            Document relevantDoc = null;
            FileReader fileReader = new FileReader(null);
            String[] ptheSplited = path.Split(new string[] { "corpus" }, StringSplitOptions.RemoveEmptyEntries);
            Document[] documents = fileReader.GetNextDocuments(@"C:\a\corpus" + ptheSplited[1]);
            for (int a=0; a< documents.Length ; a++)
            {
                if (documents[a].id.Equals(docName))
                {
                    relevantDoc = documents[a];
                    break;
                }
            }
            Parser parser = new Parser(@"C:\a\stop_words.txt", false);
            Token[] tokens = parser.processDoc(relevantDoc);
            for (i=0; i < tokens.Length; i++)
            {
                if (entitesHash.Contains(tokens[i].Value))
                {
                    if (!toReturn.Keys.Contains(tokens[i].Value))
                    {
                        toReturn.Add(tokens[i].Value, 1);
                    }
                    else
                        toReturn[tokens[i].Value]++;
                }
            }
            return toReturn;

        }
        public static void Main()
        {
            EntitiesSearcher es = new EntitiesSearcher();
            es.get5maxEntities("FBIS3-49");
        }
    }
}
