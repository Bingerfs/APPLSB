using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public class ExpressionCode
    {
        private string _setCode;

        public string SetCode
        {
            get { return _setCode; }

            set
            {
                value = CleanUpText(value);
                _setCode = value;
            }
        }

        private string _catergoryCode;

        public string CategoryCode
        {
            get { return _catergoryCode;}

            set
            {
                value = CleanUpText(value);
                _catergoryCode = value;
            }
        }

        private string _wordCode;

        public string WordCode
        {
            get { return _wordCode; }

            set 
            {
                value = CleanUpText(value);
                _wordCode = value;
            }
        }

        public string ModuleCode => $"#{CategoryCode}{SetCode}";

        public string WholeCode => $"{ModuleCode}{WordCode}";

        public ExpressionCode()
        {

        }

        public ExpressionCode(string setCode, string categoryCode, string wordCode)
        {
            SetCode = setCode;
            CategoryCode = categoryCode;
            WordCode = wordCode;
        }

        public ExpressionCode(string code)
        {
            code = CleanUpText(code);
            CategoryCode = code.Substring(0, 2);
            SetCode = code.Substring(2, 1);
            WordCode = code.Substring(3);
        }

        private string CleanUpText(string code)
        {
            var auxCode = code.Trim();
            auxCode = code.Trim('#');
            return auxCode;
        }
    }
}
