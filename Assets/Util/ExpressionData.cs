using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public class ExpressionData
    {
        public string Expression { get; set; }

        private string _expressionCode;

        public string ExpressionCode 
        { 
            get => _expressionCode;
            set 
            {
                string codeValue = value;
                if (value.Contains("#"))
                {
                    codeValue = codeValue.Replace("#", "");
                }

                _expressionCode = codeValue;
            } 
        }

        private string _categoryCode;

        public string CategoryCode
        { 
            get => _categoryCode;
            set 
            {
                string codeValue = value;
                if (value.Contains("#"))
                {
                    codeValue = codeValue.Replace("#", "");
                }

                _categoryCode = codeValue;
            }
        }

        public string WholeCode => $"#{CategoryCode}{ExpressionCode}";
    }
}
