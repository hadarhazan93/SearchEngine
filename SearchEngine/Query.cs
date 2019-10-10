using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    class Query
    {
        private String number;
        private String relevant;
        private String nonRelevant;
        private String title;
        private String query;
        private String topQuery;


        public Query(String q)
        {
            // the case that it is <top> to </top> query
            if (q.Contains("<num>") && q.Contains("<desc>") && q.Contains("<title>") && q.Contains("<narr>")) {
                topQuery = q;
                number = getNum();
                relevant = getdNarrative()["Relevant"];
                nonRelevant = getdNarrative()["Non Relevant"];
                title = getTitle();
                query = getQuery();

            }
            // the case that is a regular query.
            else
            {
                query = q;
                number = "";
                relevant = "";
                nonRelevant = "";
                title = "";
            }

        }
        public String getRelevant()
        {
            return this.relevant;
        }
        public String getNonRelevant()
        {
            return this.nonRelevant;
        }
        //return the number of the query
        public String getNum()
        {
            String[] querySplited = topQuery.Split(new string[] { " ", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            String num = "";
            Boolean flag = true;
            for (int i = 0; i< querySplited.Length; i++)
            {
                if (flag && querySplited[i] == "<num>" && querySplited[i+1] == "Number:")
                {
                    num = querySplited[i + 2];
                    flag = false;
                }
            }
            return num;
        }
        public String getNum(bool flag)
        {
            return this.number;
        }
        //return the title of the query
        public string getTitle()
        {
            String[] querySplited = topQuery.Split(new string[] { " ", "\n" ,"\r"}, StringSplitOptions.RemoveEmptyEntries);
            String title = "";
            int flag = 0;
            for (int i = 0; i < querySplited.Length; i++)
            {
                if (flag == 0 && querySplited[i] == "<title>")
                {
                    flag = 1;
                    i++;
                }
                if (flag == 1 && querySplited[i] == "<desc>")
                    flag = 2;
                if (flag == 1)
                    title = title + querySplited[i] + " ";
            }
            return title;
        }
        public Dictionary<String, String> getdNarrative()
        {
            String[] querySplited = topQuery.Split(new string[] { " ", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            String narrative = "";
            int flag = 0;
            for (int i = 0; i < querySplited.Length; i++)
            {
                if (flag == 0 && querySplited[i] == "<narr>" && querySplited[i + 1] == "Narrative:")
                {
                    flag = 1;
                    i++;
                }
                if (flag == 1 && querySplited[i] == "</top>")
                    flag = 2;
                if (flag == 1 && querySplited[i] != "narrative:")
                    narrative = narrative + querySplited[i] + " ";
            }
            Dictionary<String, String> splitNarrative = new Dictionary<String, String>();
            splitNarrative = getSplitedNarrstive(narrative);
            splitNarrative.Add("query", narrative);
            return splitNarrative;
        }
        public Dictionary<String, String> getSplitedNarrstive(String narrative)
        {
            Dictionary<String, String> splitNarrative = new Dictionary<String, String>();
            String relevant = "";
            String NonRelevant = "";
            String narrativToC = narrative.Replace("\r", " ");
            narrativToC = narrativToC.Replace("\n", " ");
            // Non sequential sentences , the split is by " - "
            if (narrativToC.Contains(" - ") && narrativToC.Contains("not relevant"))
            {
                String[] narrativeSplited = narrative.Split(new string[] { "not relevant:" }, StringSplitOptions.RemoveEmptyEntries);
                String[] NonRelevantSplit = narrativeSplited[1].Split(new string[] { "</top>" }, StringSplitOptions.RemoveEmptyEntries);
                NonRelevant = NonRelevantSplit[0];
                NonRelevant = NonRelevant.Replace("\r", " ");
                String[] RelevantSplit = narrativeSplited[0].Split(new string[] { "relevant:" }, StringSplitOptions.RemoveEmptyEntries);
                relevant = RelevantSplit[1];
                relevant = relevant.Replace("\r", " ");
            }
            // the regular case, consecutive sentences
            else
            {
                String[] narrativeSplited = narrative.Split('.');
                for (int i=0; i< narrativeSplited.Length; i++)
                {
                    if (narrativeSplited[i].Contains("not relevant"))
                    {
                        narrativeSplited[i] = narrativeSplited[i].Replace("not relevant", "");
                        narrativeSplited[i] = narrativeSplited[i].Replace("</top>", "");
                        narrativeSplited[i] = narrativeSplited[i].Replace("\n", "");
                        narrativeSplited[i] = narrativeSplited[i].Replace("\r", "");
                        NonRelevant = NonRelevant + '.' + narrativeSplited[i];
                    }
                    else if (narrativeSplited[i].Contains("non relevant"))
                    {
                        narrativeSplited[i] = narrativeSplited[i].Replace("non relevant", "");
                        narrativeSplited[i] = narrativeSplited[i].Replace("</top>", "");
                        narrativeSplited[i] = narrativeSplited[i].Replace("\n", "");
                        narrativeSplited[i] = narrativeSplited[i].Replace("\r", "");
                        NonRelevant = NonRelevant + '.' + narrativeSplited[i];
                    }
                    else if (narrativeSplited[i].Contains("non-relevant"))
                    {
                        narrativeSplited[i] = narrativeSplited[i].Replace("non-relevant", "");
                        narrativeSplited[i] = narrativeSplited[i].Replace("</top>", "");
                        narrativeSplited[i] = narrativeSplited[i].Replace("\n", "");
                        narrativeSplited[i] = narrativeSplited[i].Replace("\r", "");
                        NonRelevant = NonRelevant + '.' + narrativeSplited[i];
                    }
                    else
                    {

                        if (narrativeSplited[i].Contains("relevant"))
                        {
                            narrativeSplited[i] = narrativeSplited[i].Replace("relevant", "");
                        }
                        narrativeSplited[i] = narrativeSplited[i].Replace("</top>", "");
                        narrativeSplited[i] = narrativeSplited[i].Replace("\n", "");
                        narrativeSplited[i] = narrativeSplited[i].Replace("\r", "");
                        relevant = relevant + '.' + narrativeSplited[i];
                    }
                }
            }
            if (relevant.Contains(".Narrative:"))
            {
                relevant = relevant.Replace(".Narrative:", "");
            }
            else if (relevant.Contains("Narrative:"))
            {
                relevant = relevant.Replace("Narrative:", "");
            }
            splitNarrative.Add("Relevant", relevant);
            splitNarrative.Add("Non Relevant", NonRelevant);
            return splitNarrative;
        }

        public string getQuery()
        {
            String[] querySplited = topQuery.Split(new string[] { " ", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            String dscription = "";
            int flag = 0;
            for (int i = 0; i < querySplited.Length; i++)
            {
                if (flag == 0 && querySplited[i] == "<desc>" && querySplited[i + 1] == "Description:") { 
                    flag = 1;
                    i++;
                }
                if (flag == 1 && querySplited[i] == "<narr>")
                    flag = 2;
                if (flag == 1 && querySplited[i] != "Description:")
                    dscription = dscription + querySplited[i] + " ";
            }
            return dscription;
        }

        public static void Main()
        {
            String queryString = "<top> " + "<num> Number: 351 " + "<title> Falkland petroleum exploration " +
                            "<desc> Description: What information is available on petroleum exploration in " + "the South Atlantic near the Falkland Islands? " +
                            "<narr> Narrative: " + "Any document discussing petroleum exploration in the South Atlantic near the Falkland Islands is considered relevant.Documents discussing petroleum exploration in continental South America are not relevant. " +
                            "</top>";
            Query query = new Query(queryString);

        }

    }
}
