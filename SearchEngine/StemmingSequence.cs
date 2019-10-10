using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    /**
     * envelope class for Porter stemmer so it will comply with the rest of the pipeline
     */
    class StemmingSequence
    {
        private Stemmer stemmer;
        public StemmingSequence()
        {
            this.stemmer = new Stemmer();
        }
        public Token[] StemTokens(Token[] tokens)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i].Type == TokenType.Word)
                    tokens[i].Value = this.stemmer.stemTerm(tokens[i].Value);
            }
            return tokens;
        }
    }
}
