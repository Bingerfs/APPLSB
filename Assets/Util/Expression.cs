using System;
using System.Collections.Generic;

namespace LSB
{
    [Serializable]
    public class Expression
    {
        public string word;
        public List<string> code;

        public Expression()
        {

        }

        public Expression(string code)
        {
            var splitCode = code.Split('#');
            word = splitCode[0];
            this.code = new List<string>();
            this.code.Add($"#{splitCode[1]}");
        }

        public string getWord()
        {
            return word;
        }

        public string getList()
        {
            string res="";
            foreach (string c in code)
            {
                res += c;
            }
            return res;
        }
         
    }

    
}
