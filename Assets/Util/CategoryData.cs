using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public class CategoryData
    {
        public string Name { get; set; }

        private string _code;

        public string Code
        { 
            get => _code;
            set
            {
                string codeValue = value;
                if (value.Contains("#"))
                {
                    codeValue = codeValue.Replace("#", "");
                }

                _code = codeValue;
            } 
        }

        private string _setCode;

        public string SetCode
        {
            get => _setCode;
            set
            {
                string codeValue = value;
                if (value.Contains("#"))
                {
                    codeValue = codeValue.Replace("#", "");
                }

                _setCode = codeValue;
            }
        }

        public string IdentifierCode => $"#{Code}{SetCode}";

        public List<ExpressionData> Expressions { get; set; }   
    }
}
