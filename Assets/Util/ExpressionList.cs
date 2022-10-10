using Assets.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LSB
{
    [Serializable]
    public class ExpressionList
    {
        public List<Expression> tokens;

        public ExpressionList()
        {

        }

        

        public string getexpressions()
        {
            string res="";
            foreach (var item in tokens)
            {
                res += "WORD: " + item.Word + " CODE: " + item.GetStringList()+"\n";
            }
            return res;
        }
    }
}
