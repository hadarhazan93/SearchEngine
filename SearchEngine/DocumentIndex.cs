using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
/**
 * class maintains the documetn index, keeps all data relevant to a single document
 */
namespace SearchEngine
{
    class DocumentIndex
    {
        private String path;
        private Dictionary<String,int[]> list= new Dictionary<String,int[]>();//int[0] - max TF; int[1] - unique words count
        private Dictionary<String, String> docToMXTFWord = new Dictionary<string, string>();
        public static Dictionary<String, String> countryList = new Dictionary<string, string>();
        public static Mutex coutryList_prtc = new Mutex();
        public DocumentIndex(String path)
        {
            this.path = path;
        }
        public void addToList(String docID,int maxTF,int maxApp,int vectorPW ,String MXTFWORD)
        {
            this.list.Add(docID, new int[] { maxTF, maxApp,vectorPW });
            this.docToMXTFWord.Add(docID, MXTFWORD);
        }
        public void endSession()
        {
            StringBuilder file = new StringBuilder();
            String[] keys = this.list.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                if (countryList[keys[i]] == null)
                    countryList[keys[i]] = "N/A";
                file.Append(keys[i] + ';' + this.list[keys[i]][0] + ';' + this.list[keys[i]][1] + ';' + countryList[keys[i]] + ';' + docToMXTFWord[keys[i]] + ';' + this.list[keys[i]][2] + ';' + FileReader.docPaths[keys[i]] + '\n');
            }
            String stemmed;
            if (StartDialog.stemming)
                stemmed = "_stemmed";
            else
                stemmed = "_non_stemmed";
            File.WriteAllText(this.path+"\\documentIndex"+stemmed+".txt", file.ToString());

        }
    }
}
