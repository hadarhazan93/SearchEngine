using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/**
 * basic token for parsing
 */
namespace SearchEngine
{
    enum TokenType{Word,Number,Currency,Percentage,Date,Range,Quote,State}
    class Token : IEquatable<Token>
    {
        public String Value;
        public TokenType Type;
        public String docID;
        public int location;

        public Token(TokenType type, String value,String docId,int location)
        {
            this.Value = value;
            this.Type = type;
            this.docID = docId;
            this.location = location;

        }
        public override bool Equals(object obj)
        {
            var convert = obj as Token;
            if (convert == null)
                return false;
            return this.Value.Equals(convert.Value,StringComparison.InvariantCultureIgnoreCase);
        }
        public bool Equals(Token obj)
        {
            return this.Value.Equals(obj.Value, StringComparison.InvariantCultureIgnoreCase);
        }
        public override int GetHashCode()
        {
            int hash = 17;
            for(int i = 0; i < this.Value.Length; i++)
            {
                hash = hash * 23 + (int)this.Value[i];
            }
            return hash;
        }
    }
}
