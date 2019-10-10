using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
/**
 * Starting point of pipeline hierarchy - reads all files in a given folder and transforms them into Document type
*/
namespace SearchEngine
{
    class FileReader
    {
        public static Dictionary<String, String> docPaths = new Dictionary<string, string>();
        private String rootFolderPath;
        public int fileCounter=0;
        public List<String> paths = new List<string>();
        private bool flag = false;
        /**
         * initializer - path is root folder of file coprus collection
        */
        public FileReader(String path){
            if (path != null)
            {
                this.rootFolderPath = path;
                this.getAllFilesPath();
                StartDialog.maxProgress = paths.Count;
            }
            else
                this.flag = true;
        }
        /*
        * helper function finds all files in the folder 
        */
        private void getAllFilesPath()
        {
            String[] temp;
            temp = Directory.GetFileSystemEntries(this.rootFolderPath);
            for(int i = 0; i < temp.Length; i++)
            {
                FileAttributes attr = File.GetAttributes(temp[i]);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    String[] buffer = Directory.GetFileSystemEntries(temp[i]);
                    for (int j = 0; j < buffer.Length; j++)
                    {
                        this.paths.Add(buffer[j]);
                    }
                }
            }
        }
        /**
         * read completion indicator returns false if no files left to read
         */
        public bool hasNext()
        {
            return fileCounter < this.paths.Count;
        }
        /**
         * fetches next batch of Documents - a batch is content of a single file
         */
        public Document[] GetNextDocuments()
        {
            if (fileCounter < this.paths.Count)
            {
                String content = File.ReadAllText(this.paths[this.fileCounter]);
                String[] docs = content.Split(new string[] { "\n\n</DOC>\n\n<DOC>\n","<DOC>\n" }, StringSplitOptions.None);
                List<Document> result = new List<Document>();
                for(int i=0; i < docs.Length; i++)
                {
                    if (docs[i].Length<7||!String.Equals(docs[i].Substring(0, 7), "<DOCNO>"))
                        continue;
                    result.Add(this.formatDocument(docs[i],this.paths[this.fileCounter]));
                }
                this.fileCounter++;
                return result.ToArray();
            }
            else
                return null;

        }
        public Document[] GetNextDocuments(String path)
        {
                String content = File.ReadAllText(path);
                String[] docs = content.Split(new string[] { "\n\n</DOC>\n\n<DOC>\n", "<DOC>\n" }, StringSplitOptions.None);
                List<Document> result = new List<Document>();
                for (int i = 0; i < docs.Length; i++)
                {
                    if (docs[i].Length < 7 || !String.Equals(docs[i].Substring(0, 7), "<DOCNO>"))
                        continue;
                    if(flag)
                        result.Add(this.formatDocument(docs[i], null));
                    else
                    result.Add(this.formatDocument(docs[i], this.paths[this.fileCounter]));
                }
                this.fileCounter++;
                return result.ToArray();
        }
        /**
         * helper function - creates a Document from a string using precoded tags set
         */
        private Document formatDocument(String doc,String path)
        {
            doc = doc.Replace('\n', ' ');
            String docno;
            Match match = Regex.Match(doc, "<DOCNO>(.*?)</DOCNO>");
            docno = match.Groups[1].Value.Replace(" ","");
            String date;
            match = Regex.Match(doc, "<DATE1>(.*?)</DATE1>");
            if (!match.Success)
                match = Regex.Match(doc, "<DATE>(.*?)</DATE>");
            date = match.Groups[1].Value;
            String title;
            match = Regex.Match(doc, "<TI>(.*?)</TI>");
            title = match.Groups[1].Value;
            String text;
            match = Regex.Match(doc, "<TEXT>(.*?)</TEXT>");
            text = match.Groups[1].Value;
            String country = "NONE";
            match = Regex.Match(doc, "<F P=104>(.*?)</F>");
            if (match.Success)
            {
                String[] arr = match.Groups[1].Value.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if(arr.Length>0)
                    country =arr[0];
            }
            docPaths.Add(docno, path);
;            return new Document(path, docno, date, title, text, country);
        }

    }
}
