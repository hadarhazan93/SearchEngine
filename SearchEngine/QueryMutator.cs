using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    class QueryMutator
    {
        /**
         * class for creating variation of query using word embedding, GloVe algorithm.
         * returns n(permutationAmount) queries each consist of words permutated by the algorithm.
         * the queries sorted by relevancy of each word(in relation to original) i.e. every next permutation will be less relevant to original query.
         * synonim quality depends on quality(size) of .vec file given.
         */
        private GloVe.GloVeEmbedder glv;
        private int permutationAmount;
        public QueryMutator(String vecPath,int permutationAmount)
        {
            GloVe.GloVeEmbedder.resultAmount = permutationAmount;
            this.permutationAmount = permutationAmount;
            glv = new GloVe.GloVeEmbedder(vecPath, null, null);
        }
        //resulting tokens will contain distance from original term in location field, always a negative value the further from original the lower the value is
        public Token[][] getPermutations(Token[] target)
        {
            Token[][] result = new Token[this.permutationAmount][];
            String[][] synonims = new String[target.Length][];
            for(int i = 0; i < this.permutationAmount; i++)
            {
                result[i] = new Token[target.Length];
            }
            for (int i = 0; i < target.Length; i++)
            {
                synonims[i] = glv.getAnalogy(target[i].Value);
            }
            for(int i = 0; i < this.permutationAmount; i++)
            {
                for(int j = 0; j < target.Length;j++)
                {
                    if (synonims[j] != null)
                        result[i][j] = new Token(TokenType.Word, synonims[j][i], null, -i-1);
                    else
                        result[i][j] = new Token(TokenType.Word, target[j].Value, null, -i-1);
                }
            }
                return result;
        }
    }
}
