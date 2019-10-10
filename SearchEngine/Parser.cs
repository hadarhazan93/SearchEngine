using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
/**
 * text parsing class inputs a single doc outputs array of tokens 
 */
namespace SearchEngine
{
    class Parser
    {
        //month indentification enum add here for additional format support
        enum month {jan=01,feb=02,mar=03,apr=04,may=05,june=06,july=07,aug=08,sep=09,oct=10,nov=11,dec=12,
            january =01,february=02,march=03,april=04,jun=06,jul=07,august=08,sept=09,october=10,november=11,december=12}
        private HashSet<String> stopWords = new HashSet<string>();//stopwords collection
        private bool actual;
        public Parser(String stopWordsPath,bool actual)
        {
            String wordList = System.IO.File.ReadAllText(@"stop_words.txt");
            String[] words = wordList.Split(new string[] { "\r\n" },StringSplitOptions.None);
            this.addToHashSet(words);
            this.actual = actual;
        }
        /**works by filtering cases until point that only plain word expected
         * if word couldn`t be identified as known type it`s thrown away
         * can throw number parsing exceptions, they occur in some unidentified cases
         * can print dignostic prints which iclude all thrown away words
         */
        public Token[] processDoc(Document doc)
        {
            //need to distinguish actual runs and runs on query
            String currendId;
            if (actual)
            {
                currendId = doc.id;
                DocumentIndex.coutryList_prtc.WaitOne();
                DocumentIndex.countryList.Add(doc.id, doc.city);
                DocumentIndex.coutryList_prtc.ReleaseMutex();
            }
            else
                currendId = "query";
            List<Token> currentResult = new List<Token>();
            String[] preParce = doc.text.Split(new string[] { "'", ";", "(", ")", "!", " ", "?", "&", "\\", "/", "<", ">", "+", "=", "_", "*", "^", "#", "~", ":", "\"", "|", "[", "]", "{", "}","`","--" }, StringSplitOptions.RemoveEmptyEntries);
            int placeCount = 0;
            for (int i = 0; i < preParce.Length; i++) {
                if (preParce[i][preParce[i].Length - 1] == '.' || preParce[i][preParce[i].Length - 1] == ',')
                    preParce[i] = preParce[i].Substring(0, preParce[i].Length - 1);
                if (preParce[i].Length < 2)
                    continue;
                if (char.IsPunctuation(preParce[i][0]))
                    preParce[i] = preParce[i].Substring(1);
                if (preParce[i].Length == 0|| preParce[i].Equals(",")|| preParce[i].Equals(".")|| preParce[i].Contains('�'))
                    continue;
                if (preParce[i].Length < 2)
                    continue;
                if (preParce[i][0] == '-'&& preParce[i].Length>1&& preParce[i][1]=='-'&&preParce[i].All(char.IsPunctuation))
                    preParce[i] = preParce[i].Substring(2);
                if (preParce[i].Length < 2)
                    continue;
                if (preParce[i][0] == '-')
                    preParce[i] = preParce[i].Substring(2);
                if (preParce[i].Length < 2)
                    continue;
                //range identification
                else if (preParce[i].Contains("-") || preParce[i].Equals("between"))
                {
                    if (preParce[i][0] == '$')
                        continue;
                    if (preParce[i].Equals("between") && preParce.Length > i + 2 && preParce[i + 2].Equals("and"))//any range including Between number and number
                    {
                        if (!preParce[i + 2].Equals("and"))
                        {
                            currentResult.Add(new Token(TokenType.Word, preParce[i], doc.id, placeCount));
                            placeCount++;
                        }
                        else//no "and" found @ +2 position therefore it`s not a range but a word
                        {
                            currentResult.Add(new Token(TokenType.Range, preParce[i] + " " + preParce[i + 1] + " and " + preParce[i + 3], doc.id, placeCount));
                            placeCount++;
                            i += 3;
                        }
                    }
                    else
                    {
                        currentResult.Add(new Token(TokenType.Range, preParce[i].Replace("s/[,$.%]/", ""), doc.id, placeCount));
                        placeCount++;
                    }
                }
                //number identification sequence
                else if (preParce[i].Any(char.IsDigit))
                {
                    //currency identification
                    //checks location of word dollars by offset position (+1,+2,+3) ignores cases
                    if (preParce[i][0] == '$' || (preParce.Length > i + 1 && preParce[i + 1].Equals("Dollars", StringComparison.InvariantCultureIgnoreCase)) || (preParce.Length > i + 2 && preParce[i + 2].Equals("Dollars", StringComparison.InvariantCultureIgnoreCase)) || (preParce.Length > i + 3 && preParce[i + 3].Equals("Dollars", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        if (preParce[i][0] == '$')
                        {
                            //thousand,million,billion,trillion
                            bool flag = false;
                            String[] go = null;
                            if ((preParce.Length > i + 1 && preParce[i + 1].Equals("thousand", StringComparison.InvariantCultureIgnoreCase)) || (preParce.Length > i + 1 && preParce[i + 1].Equals("million", StringComparison.InvariantCultureIgnoreCase)) || (preParce.Length > i + 1 && preParce[i + 1].Equals("billion", StringComparison.InvariantCultureIgnoreCase)) || (preParce.Length > i + 1 && preParce[i + 1].Equals("trillion", StringComparison.InvariantCultureIgnoreCase)))
                            {
                                go = new String[2];
                                go[1] = preParce[i + 1];
                                flag = true;
                            }
                            else
                                go = new String[1];
                            go[0] = preParce[i].Substring(1);
                            try
                            {
                                String s = this.processNumber(go);
                                if (s == null)
                                    continue;
                                currentResult.Add(new Token(TokenType.Currency, s + " Dollars", doc.id, placeCount));
                                placeCount++;
                                if (flag)
                                    i++;
                            }
                            catch (Exception e)
                            {
                                continue;
                            }
                        }
                        else if ((preParce.Length > i + 1 && preParce[i + 1].Equals("Dollars", StringComparison.InvariantCultureIgnoreCase)))
                        {
                            String[] go = new String[1];
                            go[0] = preParce[i];
                            try
                            {
                                String s = this.processNumber(go);
                                if (s == null)
                                    continue;
                                currentResult.Add(new Token(TokenType.Currency, s + " Dollars", doc.id, placeCount));
                                placeCount++;
                                i++;
                            }
                            catch (Exception e)
                            {
                                continue;
                            }
                        }
                        else if ((preParce.Length > i + 2 && preParce[i + 2].Equals("Dollars", StringComparison.InvariantCultureIgnoreCase)))
                        {
                            String[] go = new String[1];
                            go[0] = preParce[i];
                            try
                            {
                                String s = this.processNumber(go);
                                if (s == null)
                                    continue;
                                currentResult.Add(new Token(TokenType.Currency, s + " " + preParce[i + 1] + " Dollars", doc.id, placeCount));
                                i += 2;
                                placeCount++;
                            }
                            catch (Exception e)
                            {
                                continue;
                            }
                        }
                        else if ((preParce.Length > i + 3 && preParce[i + 3].Equals("Dollars", StringComparison.InvariantCultureIgnoreCase)))
                        {
                            String[] go = new String[2];
                            go[0] = preParce[i];
                            go[1] = preParce[i + 1];
                            try
                            {
                                String s = this.processNumber(go);
                                if (s == null)
                                    continue;
                                currentResult.Add(new Token(TokenType.Currency, s + " Dollars", doc.id, placeCount));
                                placeCount++;
                                i += 3;
                            }
                            catch (Exception e)
                            {
                                continue;
                            }
                        }
                        /*else
                             Console.WriteLine("Unidentified currency value: " + preParce[i]);*/

                    }
                    //percentage identification
                    else if (preParce[i].Contains("%") || (preParce.Length > i + 1 && preParce[i + 1].Equals("percent", StringComparison.InvariantCultureIgnoreCase)) || (preParce.Length > i + 1 && preParce[i + 1].Equals("percentage", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        if (preParce[i].Contains("%"))
                        {
                            currentResult.Add(new Token(TokenType.Percentage, preParce[i], doc.id, placeCount));
                            placeCount++;
                        }
                        else
                        {
                            currentResult.Add(new Token(TokenType.Percentage, preParce[i] + "%", doc.id, placeCount));
                            placeCount++;
                        }
                    }
                    //date identification
                    else if ((i>0 && Enum.GetNames(typeof(month)).Any(x => x.ToLower() == preParce[i - 1]) )|| (preParce.Length > i + 1 && Enum.GetNames(typeof(month)).Any(x => x.ToLower() == preParce[i + 1])))
                    {
                        if (Enum.GetNames(typeof(month)).Any(x => x.ToLower() == preParce[i - 1]))
                        {
                            //pop previous entry
                            if (currentResult.Any()) //prevent IndexOutOfRangeException for empty list
                            {
                                currentResult.RemoveAt(currentResult.Count - 1);
                            }
                            month curMon = (month)Enum.Parse(typeof(month), preParce[i - 1], true);
                            String additionalZero = "";
                            if ((int)curMon < 10)
                                additionalZero = "0";
                            int num = 0;
                            try
                            {
                                num = int.Parse(preParce[i]);
                            }
                            catch (Exception E)
                            {
                                continue;
                            }
                            //it`s day
                            if (num < 31)
                            {
                                String n = "";
                                if (num < 10)
                                    n = "0" + num;
                                else
                                    n = "" + num;
                                currentResult.Add(new Token(TokenType.Date, additionalZero + (int)curMon + "-" + n, doc.id, placeCount));
                                placeCount++;
                            }
                            //it`s a year
                            else
                            {
                                currentResult.Add(new Token(TokenType.Date, "" + num + "-" + additionalZero + (int)curMon, doc.id, placeCount));
                                placeCount++;
                            }
                        }

                        else if (preParce.Length > i + 1 && Enum.GetNames(typeof(month)).Any(x => x.ToLower() == preParce[i + 1]))
                        {
                            month curMon = (month)Enum.Parse(typeof(month), preParce[i + 1], true);
                            String additionalZero = "";
                            if ((int)curMon < 10)
                                additionalZero = "0";
                            int num;
                            try
                            {
                                num = int.Parse(preParce[i]);
                            }
                            catch (Exception e)
                            {
                                continue;
                            }
                            String n = "";
                            if (num < 10)
                                n = "0" + num;
                            currentResult.Add(new Token(TokenType.Date, additionalZero + (int)curMon + "-" + n, doc.id, placeCount));
                            placeCount++;
                            i++;

                        }

                        /*else
                            Console.WriteLine("unidentified date format: " + preParce[i]);*/
                    }
                    //non plain number
                    else if ((preParce.Length > i + 1 && preParce[i + 1].Equals("thousand", StringComparison.InvariantCultureIgnoreCase)) || (preParce.Length > i + 1 && preParce[i + 1].Equals("million", StringComparison.InvariantCultureIgnoreCase)) || (preParce.Length > i + 1 && preParce[i + 1].Equals("billion", StringComparison.InvariantCultureIgnoreCase)) || (preParce.Length > i + 1 && preParce[i + 1].Equals("trillion", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        String[] go = new String[2];
                        go[0] = preParce[i];
                        go[1] = preParce[i + 1];
                        try
                        {
                            String s = this.processNumber(go);
                            if (s == null)
                                continue;
                            currentResult.Add(new Token(TokenType.Number, s, doc.id, placeCount));
                            placeCount++;
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                        i++;
                    }
                    //plain number
                    else if (this.checkIfNumber(preParce[i]))
                    {
                        String[] go = new String[1];
                        go[0] = preParce[i];
                        try
                        {
                            String s = this.processNumber(go);
                            if (s == null)
                                continue;
                            currentResult.Add(new Token(TokenType.Number, s, doc.id, placeCount));
                            placeCount++;
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                    }
                    //junk
                    /*else
                    {
                        Console.WriteLine("unidentified number format: " + preParce[i]);
                    }*/
                }
                //additional rules
                else if (preParce[i].Contains('.') && preParce[i].Contains('U') && (preParce[i].Equals("U.S.") || preParce[i].Equals("U.K.")))
                {
                    if (preParce[i].Equals("U.S."))
                    {
                        currentResult.Add(new Token(TokenType.State, "United States Of America", doc.id, placeCount));
                        placeCount++;
                    }
                    if (preParce[i].Equals("U.K."))
                    {
                        currentResult.Add(new Token(TokenType.State, "United Kingdom", doc.id, placeCount));
                        placeCount++;
                    }

                }
                else if (preParce[i].All(char.IsLetter) && preParce[i].Length > 1)
                {
                    if (!this.stopWords.Contains(preParce[i].ToLower()))
                        currentResult.Add(new Token(TokenType.Word, preParce[i], doc.id, placeCount));
                    placeCount++;
                }
                //else filter junk words and add as a word or special stopwords: "and"
                /*else
                    Console.WriteLine("unidentified term discovered: " + preParce[i] + " proceeding...");
                    */
            }
            return currentResult.ToArray();
        }
        //accepts string array of 2 strings secound can be empty in case this is a plain number (int or decimal)
        private String processNumber(String[] number)
        {
            String res = null;
            //case: plain number using commas
            if (number.Contains(","))
            {
                number[0] = number[0].Replace(",", "");
                int num = 0;
                if (!int.TryParse(number[0], out num))
                    return null;
                if (num > 1000000000)
                    res = "" + Math.Round((double)(num / 1000000000), 9) + "B";
                else if (num > 1000000)
                    res = "" + Math.Round((double)(num / 1000000), 6) + "M";
                else if (num > 1000)
                    res = "" + Math.Round((double)(num / 1000), 3) + "K";
                else
                {
                    //Console.WriteLine("unidefined number found, case plain: " + number[0] + " parsed value: " + num);
                }
            }
            //case plain decimal number no commas
            else if (number.Contains("."))
            {
                double num = 0;
                if (!double.TryParse(number[0], out num))
                    return null;
                if (num > 1000000000)
                    res = "" + Math.Round(num / 1000000000, 11) + "B";
                else if (num > 1000000)
                    res = "" + Math.Round(num / 1000000, 8) + "M";
                else if (num > 1000)
                    res = "" + Math.Round(num / 1000, 5) + "K";
                else
                {
                    //Console.WriteLine("unidefined number found, case decimal: " + number[0] + " parsed value: " + num);
                }
            }
            //case number using keywords: thousand,million,billion,trillion
            else if (number.Length > 1)
            {
                double num = 0;
                if (!double.TryParse(number[0], out num))
                    return null;
                if (number[1].Equals("thousand", StringComparison.InvariantCultureIgnoreCase))
                {
                    res = "" + Math.Round(num, 5) + "K";
                }
                else if (number[1].Equals("million", StringComparison.InvariantCultureIgnoreCase))
                {
                    res = "" + Math.Round(num, 5) + "M";

                }
                else if (number[1].Equals("billion", StringComparison.InvariantCultureIgnoreCase))
                {
                    res = "" + Math.Round(num, 5) + "B";
                }
                else if (number[1].Equals("trillion", StringComparison.InvariantCultureIgnoreCase))
                {
                    res = "" + Math.Round(num, 5) * 1000 + "T";
                }
                else
                {
                    //Console.WriteLine("unidefined number found, case added text: " + number[0] + " parsed value: " + num + " added value: " + number[1]);
                }
            }
            else if (number[0].All(char.IsDigit))
            {
                int value = 0;
                if (!int.TryParse(number[0], out value))
                    return null;
                res = "" + value;
            }
            else if (number[0].Contains("s/[m]|[bn]{2}/"))
            {
                int index = this.findEndOfDigits(number[0]);
                if (index == -1||!number[0].All(char.IsLetterOrDigit))
                    throw new Exception("unidefined number found");
                int num = int.Parse(number[0].Substring(0, index + 1));
                String mod = number[0].Substring(index + 1);
                if (mod.Equals("bn"))
                    res = "" + num + "B";
                else if (mod.Equals("m"))
                    res = "" + num + "M";
                else
                    throw new Exception("unidefined number found");

            }
            else
            {
                //Console.WriteLine("unidefined number found: " + number[0]);
            }
            return res;
        }
        private bool checkIfNumber(String s)
        {
            for(int i = 0; i < s.Length; i++)
            {
                if (!(char.IsDigit(s[i]) || s[i] == ',' || s[i] == '.'))
                    return false;
            }
            return true;
        }
        private String[] addToHashSet(String[] list)
        {
            List<String> res = new List<String>();
            for (int i = 0; i < list.Length; i++)
            {
                this.stopWords.Add(list[i]);
            }
            return res.ToArray();
        }
        private int findEndOfDigits(String s)
        {
            for(int i = 0; i < s.Length;i++)
            {
                if (char.IsLetter(s[i]))
                    return i;
            }
            return -1;
        }
    }
}
