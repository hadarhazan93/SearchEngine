using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchEngine
{
    class Searcher
    {
        Parser parser;
        StemmingSequence stemming;
        Ranker ranker;
        QueryMutator qm;
        bool stemmed;

        public Searcher(bool stem)
        {
            stemmed = stem;
            stemming = new StemmingSequence();
            parser = new Parser(StartDialog.stopWordsPath, false);
            ranker = new Ranker(StartDialog.indexPath, stemmed, @"C:\Users\welta\Desktop\New folder\glove.840B.300d\glove.840B.300d.vec"/*@"glove.6B.100dc.vec"*/);

        }
        // read from queries file and send queries ,one by one
        // the query need to look like this <top> to </top>
        public Dictionary<String, List<String>> multipleSearch(String pathOfQureries, bool semantic, string[] cityCheckedByUser)
        {
            Dictionary<String, List<String>> toReturn = new Dictionary<string, List<string>>();
            // the path from the user
            String content = File.ReadAllText(pathOfQureries);
            String[] querySplited = content.Split(new string[] { "<top>" }, StringSplitOptions.RemoveEmptyEntries);
            if (semantic)
            {
                for (int i = 0; i < querySplited.Length; i++)
                {
                    Query query = new Query(querySplited[i]);
                    toReturn.Add(query.getNum(), semanticSearch(query.getQuery(), query.getRelevant(), cityCheckedByUser));
                }
            }
            else  
            {
                for (int i = 0; i < querySplited.Length; i++)
                {
                    Query query = new Query(querySplited[i]);
                    toReturn.Add(query.getNum(), regularSearch(query.getQuery(), query.getRelevant(), cityCheckedByUser));
                }
            }
            return toReturn;
        }

        public List<String> regularSearch(String query, String relevant, string[] cityCheckedByUser)
        {
            Dictionary<String, double> rankedDocuments;
            Document queryInSidDoc = new Document("", "", "", "", query, "");
            List<String> max50Docs = new List<String>();
            // false in parser min that it is query (not document)
            Token[] tokens = parser.processDoc(queryInSidDoc);
            Document queryInSidDocRelevant = new Document("", "", "", "", relevant, "");
            Token[] tokensRelevant = parser.processDoc(queryInSidDocRelevant);
            if (stemmed)
            {
                tokens = stemming.StemTokens(tokens);
            }
            if (cityCheckedByUser != null)
            {
                rankedDocuments = ranker.processQueryWithCities(tokens, tokensRelevant, cityCheckedByUser);
            }
            else
            {
                rankedDocuments = ranker.processQuery(tokens, tokensRelevant);
            }
            List<KeyValuePair<string, double>> sorted = (from kv in rankedDocuments orderby kv.Value select kv).ToList();
            // we return only 50 docs to query
            sorted.Reverse();
            for (int i = 0; i < 50 && i < sorted.Count; i++)
            {
                max50Docs.Add(sorted[i].Key.ToString());
                Console.WriteLine(max50Docs[i]);
            }
    
            return max50Docs;
        }
        ///
        public List<String> semanticSearch(String query, String relevant, string[] cityCheckedByUser)
        {
            Dictionary<String, double> rankedDocuments;
            Document queryInSidDoc = new Document("", "", "", "", query, "");
            List<String> max50Docs = new List<String>();
            List<String> m_max50Docs = new List<String>();
            Document queryInSidDocRelevant = new Document("", "", "", "", relevant, "");
            Token[] tokensRelevant = parser.processDoc(queryInSidDocRelevant);
            // false in parser min that it is query (not document)
            Token[] tokens = parser.processDoc(queryInSidDoc);
            if (stemmed)
            {
                tokens = stemming.StemTokens(tokens);
            }
            if (cityCheckedByUser != null)
            {
                rankedDocuments = ranker.processQuerySemanticallyWithCities(tokens, tokensRelevant, cityCheckedByUser);
            }
            else
            {
                rankedDocuments = ranker.processQuerySemantically(tokens, tokensRelevant);
            }
            List<KeyValuePair<string, double>> sorted = (from kv in rankedDocuments orderby kv.Value select kv).ToList();
            sorted.Reverse();
            // we return only 50 docs to query
            for (int i = 0; i < 50 && i < sorted.Count; i++)
            {
                max50Docs.Add(sorted[i].Key.ToString());
                // m_max50Docs.Add(m_sorted[i].Key.ToString());

            }
            return max50Docs;
        }
    }
}
