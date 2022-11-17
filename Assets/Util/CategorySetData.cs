using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public class CategorySetData
    {
        private string _categoryCode;

        public string CategoryCode { get => _categoryCode; set => _categoryCode = value; }

        private string _setCode;

        public string SetCode { get => _setCode; set => _setCode = value; }

        public CategorySetData()
        {

        }

        public CategorySetData(string categoryCode, string setCode)
        {
            _categoryCode = categoryCode;
            _setCode = setCode;
        }
    }
}
