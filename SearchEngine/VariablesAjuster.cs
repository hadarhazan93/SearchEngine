using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    class VariablesAjuster
    {
        Dictionary<int, HashSet<String>> optimalResults;
        public VariablesAjuster(String resultsPath)
        {
            optimalResults = new Dictionary<int, HashSet<String>>();
            String content = File.ReadAllText(resultsPath);
            String[] lines = content.Split('\n');
            int currentID = 351;
            optimalResults.Add(351, new HashSet<String>());
            int currentOrder = 1;
            for(int i = 0; i < lines.Length; i++)
            {
                String[] buffer = lines[i].Split(' ');
                if (Int32.Parse(buffer[0]) != currentID)
                {
                    currentID = Int32.Parse(buffer[0]);
                    optimalResults.Add(currentID, new HashSet<String>());
                    currentOrder = 1;
                }
                if (buffer[3].Equals("1\r"))
                {
                    optimalResults[currentID].Add(buffer[2]);
                    currentOrder++;
                }
            }
        }
        public int compareResults(Query query,Dictionary<string,double> qres)
        {
            List<KeyValuePair<string, double>> sorted = (from kv in qres orderby kv.Value select kv).ToList();
            sorted.Reverse();
            List<String> docRes = new List<string>();
            int result = 0;
            for(int i=0; i < sorted.Count; i++)
            {
                docRes.Add(sorted[i].Key);
            }
            Console.WriteLine("Query: " + query.getNum(true));
            HashSet<String> qOptimal = this.optimalResults[Int32.Parse(query.getNum(true))];
            for(int i = 0; i < 50; i++)
            {
                //Console.WriteLine(Int32.Parse(query.getNum()) + " 0 " + docRes[i] + " 1 0 r");
                if (qOptimal.Contains(docRes[i]))
                {
                    result++;
                    Console.Write(i + ",");
                }
            }
            Console.WriteLine(" ");
            return result;
        }
        /***
         * function for performing set manipulations such as adding of subtracting values of vec2 from all values that exist in vec1
         */
        public Dictionary<string,double> manipulateResults(Dictionary<string,double> vec1,Dictionary<string,double> vec2,string operation)
        {
            String[] vec1Keys = vec1.Keys.ToArray();
            for(int i = 0; i < vec1Keys.Length; i++)
            {
                if (vec2.Keys.Contains(vec1Keys[i]))
                {
                    switch (operation)
                    {
                        case "add":
                            vec1[vec1Keys[i]] += vec2[vec1Keys[i]];
                            break;
                        case "substract":
                            vec1[vec1Keys[i]] -= vec2[vec1Keys[i]];
                            break;
                        default:
                            continue;
                    }
                }
            }
            return vec1;
        }
        public static void Main()
        {
            Parser parser = new Parser(@"C:\Users\Hadar\Desktop\לימודים\סמסטר ז\אחזור\מנוע חלק ב\SearchEngine.v3.1\SearchEngine.v2.1\stop_words.txt", false);
            VariablesAjuster va = new VariablesAjuster(@"C:\Users\Hadar\Desktop\לימודים\סמסטר ז\אחזור\מנוע חלק ב\SearchEngine.v3.1\SearchEngine.v2.1\qrels.txt");
            //QueryMutator qm = new QueryMutator(@"X:\Junk\glove.6B.100dc.vec", 1);
            Ranker ranker = new Ranker(@"C:\a\results", false, @"C:\a\glove.6B.100dc.vec");
            double cosSimVal = 0.01;
            double bm25Val = 0.01;
            double maxCosSim = 0;
            double maxBM25 = 0;
            int max = 0;
            String queries = File.ReadAllText(@"C:\Users\Hadar\Desktop\לימודים\סמסטר ז\אחזור\מנוע חלק ב\SearchEngine.v3.1\SearchEngine.v2.1\queries.txt");
            String[] q = queries.Split(new string[] { "\r\n\r\n\r\n" },StringSplitOptions.RemoveEmptyEntries);
            Token[][] arr = new Token[15][];
            Token[][] relevant = new Token[15][];
            //Token[][] irrelevant = new Token[15][];
            Query[] col = new Query[15];
            for(int i = 0; i < 15; i++)
            {
                col[i] = new Query(q[i]);
                arr[i] = parser.processDoc(new Document(null, null, null, null, col[i].getQuery(), null));
                relevant[i] = parser.processDoc(new Document(null, null, null, null, col[i].getRelevant(), null));
                //irrelevant[i] = parser.processDoc(new Document(null, null, null, null, col[i].getNonRelevant(), null));
            }

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    //ranker.bm25Mult = bm25Val;
                    //ranker.cosSimMult = cosSimVal;
                    int score = 0;
                    //calculate query and compare
                    for (int k = 0; k < 15; k++)
                    {
                        //Token[][] mutated = qm.getPermutations(arr[k]);
                        //Dictionary<string,double> queryResult= ranker.processQuery(arr[k],relevant[k]);
                        Dictionary<string, double> queryResult = ranker.processQuerySemantically(arr[k], relevant[k]);

                        //Dictionary<string, double> irrelevantResults = ranker.processQuery(irrelevant[k]);
                        //queryResult = va.manipulateResults(queryResult, irrelevantResults, "substract");
                        score += va.compareResults(col[k], queryResult);
                    }
                    Console.WriteLine("BM=" + Math.Round(bm25Val, 2) + " CosSim=" + Math.Round(cosSimVal, 2) + " Score: " + score);
                    //compare with max if larger - update
                    if (score > max)
                    {
                        max = score;
                        maxBM25 = bm25Val;
                        maxCosSim = cosSimVal;
                    }
                    bm25Val += 0.05;
                }
                cosSimVal += 0.05;
                bm25Val = 0.01;
            }
            Console.WriteLine("MAX: BM=" + Math.Round(maxBM25, 2) + " CosSim=" + Math.Round(maxCosSim, 2) + " Score: " + max);
        }
    }
}
