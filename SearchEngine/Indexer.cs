using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
/**
 * Indexer class takes care of all index creation tasks
 * also responsible for index optimization after main run complete
 */
namespace SearchEngine
{
    class Indexer
    {
        private Dictionary dictionary;
        private Posting post;
        private String rootPath;
        public static int opt_cnt;
        public Indexer(String path)
        {
            this.dictionary = new Dictionary(path);
            this.post = new Posting(path);
            this.rootPath = path;
            Indexer.opt_cnt = 0;
        }
        public void ProcessBatch(Token[] tokens)
        {
            this.dictionary.ProcessTokens(tokens);
            this.post.ProcessTokens(tokens);
        }
        /**
         * file reorganizing procedure writes dictionary to disk and optimizes inverse index files
         * uses multithreading for optimization task
         */
        public void endSession()
        {
            this.post.flushBuffer(post.termBuffer);
            IndexOptimizer i1 = new IndexOptimizer(this.dictionary);
            IndexOptimizer i2 = new IndexOptimizer(this.dictionary);
            IndexOptimizer i3 = new IndexOptimizer(this.dictionary);
            IndexOptimizer i4 = new IndexOptimizer(this.dictionary);
            if (!Directory.Exists(rootPath + "\\postings"))
                System.IO.Directory.CreateDirectory(rootPath + "\\postings");
            this.rootPath = this.rootPath + "\\postings";
            String tempPath = "";
            if (StartDialog.stemming)
            {
                if (!Directory.Exists(this.rootPath + "\\stemmed"))
                    System.IO.Directory.CreateDirectory(rootPath + "\\stemmed");
                tempPath = this.rootPath + "\\stemmed";
            }
            else
            {
                if (!Directory.Exists(this.rootPath + "\\non_stemmed"))
                    System.IO.Directory.CreateDirectory(rootPath + "\\non_stemmed");
                tempPath = this.rootPath + "\\non_stemmed";
            }
            FileReader f = new FileReader(tempPath);
            //to prevent simultaneous reads/write every thread gets only a portion of files to take care of
            String[] paths = f.paths.ToArray();
            int partsize = paths.Length / 4;
            String[] pa1 = new String[partsize];
            Array.Copy(paths, 0, pa1, 0, partsize);
            String[] pa2 = new String[partsize];
            Array.Copy(paths, partsize, pa2, 0, partsize);
            String[] pa3 = new String[partsize];
            Array.Copy(paths, partsize*2, pa3, 0, partsize);
            String[] pa4 = new String[paths.Length-partsize*3];
            Array.Copy(paths, partsize*3, pa4, 0, paths.Length - partsize * 3);

            int i = 0;
            Thread t1 = new Thread(new ThreadStart(() =>
            {
                Task.Run(() => i1.RestructureFiles(pa1));
            }));
            t1.Start();
            Thread t2 = new Thread(new ThreadStart(() =>
            {
                Task.Run(() => i1.RestructureFiles(pa2));
            }));
            t2.Start();
            Thread t3 = new Thread(new ThreadStart(() =>
            {
                Task.Run(() => i1.RestructureFiles(pa3));
            }));
            t3.Start();
            Thread t4 = new Thread(new ThreadStart(() =>
            {
                Task.Run(() => i1.RestructureFiles(pa4));
            }));
            t4.Start();
            while (Indexer.opt_cnt!=4)
            {
                Thread.Sleep(250);
            }
            Dictionary<string, int[]>[] temp = { i1.dict, i2.dict, i3.dict, i4.dict };
            Dictionary<string, int[]> res = this.uniteDicts(temp);
            this.writeDictionary(res);
            this.dictionary.endSession();
        }
        /**
         * after index optimization in multithreaded mode dictionaries of all thread need to be united
         */
        private Dictionary<string,int[]> uniteDicts(Dictionary<string,int[]>[] dicts)
        {
            String[] keys = dicts[0].Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                dicts[0][keys[i]][0] += dicts[1][keys[i]][0];
                if (dicts[0][keys[i]][1] == 0)
                    dicts[0][keys[i]][1] = dicts[1][keys[i]][1];
                dicts[0][keys[i]][0] += dicts[2][keys[i]][0];
                if (dicts[0][keys[i]][1] == 0)
                    dicts[0][keys[i]][1] = dicts[2][keys[i]][1];
                dicts[0][keys[i]][0] += dicts[3][keys[i]][0];
                if (dicts[0][keys[i]][1] == 0)
                    dicts[0][keys[i]][1] = dicts[3][keys[i]][1];
            }
            return dicts[0];
        }
        /**
         * writes resulting dictionary to disk
         * format is [term];[document frequency];[first line in index file]
         */
        public void writeDictionary(Dictionary<string, int[]> res)
        {
            String[] resultingDictionary = res.Keys.OrderBy(q => q).ToArray();
            StringBuilder fileContents = new StringBuilder();
            for (int i = 0; i < resultingDictionary.Length; i++)
            {
                if (res[resultingDictionary[i]][0] == 0)
                    continue;
                if(this.dictionary.capitalized.Contains(resultingDictionary[i]))
                    fileContents.Append(resultingDictionary[i] + ";" + res[resultingDictionary[i]][0] + ";" + res[resultingDictionary[i]][1] + '\n');
                else
                    fileContents.Append(resultingDictionary[i].First().ToString().ToUpper() + resultingDictionary[i].Substring(1) + ";" + res[resultingDictionary[i]][0] + ";" + res[resultingDictionary[i]][1] + '\n');
            }
            string stemmed;
            if (StartDialog.stemming)
                stemmed = "_stemmed";
            else
                stemmed = "_non_stemmed";
            File.WriteAllText(rootPath + "\\dictionary" + stemmed + ".txt", fileContents.ToString());


        }
    }
}
