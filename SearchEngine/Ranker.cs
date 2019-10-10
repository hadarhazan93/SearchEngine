using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    class Ranker
    {
        private String path;
        private IndexSearcher iS;
        private double k = 1.5;
        private double b = 0.75;
        private double avgLen = 224.93;//calculated average value (not includes stop words)
        private QueryMutator qm;
        public double cosSimMult;
        public double bm25Mult;
        public Ranker(String path, bool stemmed,String vectorPath)
        {
            this.path = path;
            this.iS = new IndexSearcher(path, stemmed);
            this.qm = new QueryMutator(vectorPath, 2);
            this.cosSimMult = 1.04;
            this.bm25Mult = 0.04;
        }
        /**
         * main query processing function
         * @input - query after Tokenizing
         * @output - map docId->relevancy value
         */
        private Dictionary<String, double> regularSearch(Token[] query)
        {
            //building query vector.
            Dictionary<String, int> queryVector = new Dictionary<string, int>();
            for (int i = 0; i < query.Length; i++)
            {
                if (queryVector.ContainsKey(query[i].Value.ToLower()))
                    queryVector[query[i].Value.ToLower()]++;
                else
                    queryVector.Add(query[i].Value.ToLower(), 1);
            }
            Dictionary<String, Dictionary<String, int>> termVectors = new Dictionary<String, Dictionary<String, int>>();
            String[] termList = queryVector.Keys.ToArray();
            //building document vector for each term and mapping it to specific document vector.
            for (int i = 0; i < termList.Length; i++)
            {
                /*if (!iS.termExists(termList[i]))
                {
                    termVectors.Add(termList[i], null);
                    continue;
                }*/
                termVectors.Add(termList[i], iS.getTermVector(termList[i]));
            }
            //now we have mapped a vector of documents and their TF for each unique term.
            List<Dictionary<String, double>> algResults = new List<Dictionary<string, double>>();
            algResults.Add(this.calculateByCosSim(termVectors, queryVector));
            algResults.Add(this.calculateByBM25(termVectors, queryVector));
            return this.produceResult(algResults.ToArray());
        }
        public Dictionary<String, double> processQueryWithCities(Token[] query, Token[] related, string[] list)
        {
            Dictionary<string, double> queryRes = this.processQuery(query, related);
            List<Dictionary<string, double>> resluts = new List<Dictionary<string, double>>();
            for (int i = 0; i < list.Length; i++)
            {
                List<string> tagLimit = iS.getCityByTag(list[i]);
                List<string> wordsFound = this.processQuery(new Token[] { new Token(TokenType.Word, list[i], null, 0) }, new Token[] { }).Keys.ToList();
                resluts.Add(this.intersect(queryRes, tagLimit));
                resluts.Add(this.intersect(queryRes, wordsFound));
            }

            for (int i = 0; i < resluts.Count; i++)
            {
                queryRes = this.unite(queryRes, resluts[i]);
            }
            return queryRes;
        }
        public Dictionary<String, double> processQuery(Token[] query, Token[] related)
        {
            Dictionary<string, double> qres = this.regularSearch(query);
            Dictionary<string, double> rres = this.regularSearch(related);
            Dictionary<string, double> gres = this.manipulateResults(qres, rres, "add");
            return gres;
        }
        public Dictionary<String, double> processQuerySemanticallyWithCities(Token[] query, Token[] related,string[] list)
        {
            Dictionary<string, double> queryRes = this.processQuerySemantically(query, related);
            List<Dictionary<string, double>> resluts = new List<Dictionary<string, double>>();
            for(int i = 0; i < list.Length; i++)
            {
                List<string> tagLimit = iS.getCityByTag(list[i]);
                List<string> wordsFound = this.processQuery(new Token[] { new Token(TokenType.Word, list[i], null, 0) }, new Token[] { }).Keys.ToList();
                resluts.Add(this.intersect(queryRes, tagLimit));
                resluts.Add(this.intersect(queryRes, wordsFound));
            }

            for(int i = 0; i < resluts.Count; i++)
            {
                queryRes = this.unite(queryRes, resluts[i]);
            }
            return queryRes;
        }
        public Dictionary<String, double> processQuerySemantically(Token[] query, Token[] related)
        {
            Dictionary<string, double> regRes = processQuery(query, related);
            Dictionary<string, double> semRes = processQuery(this.getMutation(query), this.getMutation(related));
            Dictionary<string, double> gres = this.manipulateResults(regRes, semRes, "add");
            return gres;
        }
        private Token[] getMutation(Token[] query)
        {
            Token[][] mutations = qm.getPermutations(query);
            for(int i = 0; i < query.Length; i++)
            {
                if (iS.termExists(mutations[0][i].Value))
                    query[i].Value = mutations[0][i].Value;
                /*else if (iS.termExists(mutations[1][i].Value))
                    query[i].Value = mutations[1][i].Value;*/
            }
            return query;
        }
        private Dictionary<String, double> calculateByBM25(Dictionary<String, Dictionary<String, int>> documentVectors, Dictionary<string, int> queryVector)
        {
            
            Dictionary<String, double> result = new Dictionary<string, double>();
            String[] terms = queryVector.Keys.ToArray();
            for(int i = 0; i < terms.Length; i++)
            {
                Dictionary<String, int> termVector = documentVectors[terms[i]];
                String[] currentDocuments = termVector.Keys.ToArray();
                for(int j = 0; j < currentDocuments.Length; j++)
                {
                    if (!result.ContainsKey(currentDocuments[j]))
                        result.Add(currentDocuments[j], 0);
                    double numerator = queryVector[terms[i]] * (k + 1);
                    double denominator = termVector[currentDocuments[j]] + k * (1 - b + b * iS.getDocLen(currentDocuments[j]) / avgLen);
                    result[currentDocuments[j]] += numerator / denominator * this.calculateIDF(termVector);
                }
            }
            return result;
        }
        private Dictionary<String, double> calculateByCosSim(Dictionary<String, Dictionary<String, int>> documentVectors, Dictionary<string, int> queryVector)
        {
            //calculating power of query vector
            String[] queryWords = queryVector.Keys.ToArray();
            int sum = 0;
            for (int i = 0; i < queryWords.Length; i++)
            {
                sum += queryVector[queryWords[i]] * queryVector[queryWords[i]];
            }
            double queryPW = Math.Sqrt(sum);
            Dictionary<String, double> result = new Dictionary<String, double>();
            //building numerators by building them by term basis i.e. each document`s term value multiplied by the same value of query vector for all documents containing that value 
            String[] existingWords = documentVectors.Keys.ToArray();
            for (int i = 0; i < existingWords.Length; i++)
            {
                Dictionary<String, int> current = documentVectors[existingWords[i]];
                String[] documents = current.Keys.ToArray();
                for (int j = 0; j < documents.Length; j++)
                {
                    if (!result.ContainsKey(documents[j]))
                        result.Add(documents[j], 0);
                    result[documents[j]] += queryVector[existingWords[i]] * current[documents[j]];
                }
            }
            String[] finalDocumentsList = result.Keys.ToArray();
            for (int i = 0; i < finalDocumentsList.Length; i++)
            {
                double docVecPW = iS.getDocumentVectorPW(finalDocumentsList[i]);
                double denominator = docVecPW + queryPW;
                result[finalDocumentsList[i]] = result[finalDocumentsList[i]] / denominator;
            }
            return result;
        }
        private double calculateIDF(Dictionary<String, int> termVector)
        {
            double preLog = this.iS.getDocCount() / termVector.Count;
            return Math.Log(preLog, 10);
        }
        //hardcoded to hadle 2 results vectors only
        private Dictionary<String, double> produceResult(Dictionary<String,double>[] algResults)
        {            
            //here we decigiving ded the proportion for each algorithm
            List<String> docsList = new List<string>();
            String[] docs1 = algResults[0].Keys.ToArray();
            for(int i = 0; i < docs1.Length; i++)
            {
                docsList.Add(docs1[i]);
            }
            /*String[] docs2 = algResults[1].Keys.ToArray();
            for (int i = 0; i < docs2.Length; i++)
            {
                if(!docsList.Contains(docs2[i]))
                    docsList.Add(docs2[i]);
            }*/
            Dictionary<String, double> result = new Dictionary<string, double>();
            int len = Math.Max(algResults[0].Keys.Count, algResults[1].Keys.Count);
            for(int i = 0; i < docsList.Count; i++)
            {
                if (algResults[0].Keys.Contains(docsList[i]))
                    result.Add(docsList[i], algResults[0][docsList[i]] * this.cosSimMult);
                if (algResults[1].Keys.Contains(docsList[i]))
                {
                    if (!result.Keys.Contains(docsList[i]))
                        result.Add(docsList[i], 0);
                    result[docsList[i]] += algResults[1][docsList[i]] * this.bm25Mult;
                }
            }
            return result;
        }
        private List<String> uniteResults(Dictionary<String, double>[] algResults)
        {
            List<String> result = new List<string>();
            for(int i = 0; i < algResults.Length; i++)
            {
                for(int j=0;j< algResults[i].Keys.Count; j++)
                {
                    String[] buffer = algResults[i].Keys.ToArray();
                    if (!result.Contains(buffer[j]))
                        result.Add(buffer[j]);
                }
            }
            return result;
        }
        /***
         * function for performing set manipulations such as adding of subtracting values of vec2 from all values that exist in vec1
         */
        private Dictionary<string, double> manipulateResults(Dictionary<string, double> vec1, Dictionary<string, double> vec2, string operation)
        {
            String[] vec1Keys = vec1.Keys.ToArray();
            for (int i = 0; i < vec1Keys.Length; i++)
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
        /***
         * returns intersection of two vectors with values provided in the fisrt vector.
         */
        private Dictionary<string,double> intersect(Dictionary<string, double> vec1, Dictionary<string, double> vec2)
        {
            Dictionary<string, double> res = new Dictionary<string, double>();
            String[] keys1 = vec1.Keys.ToArray();
            for(int i = 0; i < keys1.Length; i++)
            {
                if (vec2.Keys.Contains(keys1[i]))
                    res.Add(keys1[i], vec1[keys1[i]]);
            }
            return res;
        }
        private Dictionary<string, double> intersect(Dictionary<string, double> vec1, List<string> vec2)
        {
            Dictionary<string, double> res = new Dictionary<string, double>();
            String[] keys1 = vec1.Keys.ToArray();
            for (int i = 0; i < keys1.Length; i++)
            {
                if (vec2.Contains(keys1[i]))
                    res.Add(keys1[i], vec1[keys1[i]]);
            }
            return res;
        }
        /***
         * returns unification of two vectors, values taken from vec1 if not unique
         */
        private Dictionary<string,double> unite(Dictionary<string,double> vec1,Dictionary<string,double> vec2)
        {
            Dictionary<string, double> res = new Dictionary<string, double>();
            String[] keys1 = vec1.Keys.ToArray();
            String[] keys2 = vec2.Keys.ToArray();
            for (int i = 0; i < keys1.Length; i++)
            {
                res.Add(keys1[i], vec1[keys1[i]]);
            }
            for(int i = 0; i < keys2.Length; i++)
            {
                if(!res.Keys.Contains(keys2[i]))
                    res.Add(keys2[i], vec2[keys2[i]]);
            }
            return res;
        }
        
    }
}
