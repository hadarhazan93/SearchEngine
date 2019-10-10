using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    class Entities
    {
        List<String> entities = new List<string>();
        public Entities()
        {
            List<String> entiti = readDicAndTakeEntities();
            String[] entitiArray = entiti.ToArray();
            for (int i = 0; i < entitiArray.Length ; i++)
            {
                entities.Add(entitiArray[i]);
            }
        }
        public List<String> getEntities()
        {
            return entities;
        }
        public List<String> readDicAndTakeEntities()
        {
            List<String> entities = new List<String>();
            String[] recordSplited;
            String term;
            String dictionary = File.ReadAllText(@"C:\a\results\postings\dictionary_non_stemmed.txt");
            String[] dictionarySplited = dictionary.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i=0; i < dictionarySplited.Length; i++)
            {
                recordSplited = dictionarySplited[i].Split(';');
                term = recordSplited[0];
                if (char.IsUpper(term[0]))
                {
                    entities.Add(term);
                }
            }
            return entities;
        }
        public static void Main()
        {
            Entities e = new Entities();
        }
    }
}
