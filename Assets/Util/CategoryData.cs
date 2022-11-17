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

        private Dictionary<string, Dictionary<string, ExpressionData>> _dictionaryOfSets; 

        public Dictionary<string, Dictionary<string, ExpressionData>> DictionaryOfSets { get => _dictionaryOfSets; set => _dictionaryOfSets = value; }  

        public CategoryData()
        {
            _dictionaryOfSets = new Dictionary<string, Dictionary<string,ExpressionData>>();
        }
        public CategoryData(string name, string code)
        {
            Name = name;
            Code = code;
            _dictionaryOfSets = new Dictionary<string, Dictionary<string, ExpressionData>>();
        }

        public void AddSet(string setCode)
        {
            if (!_dictionaryOfSets.ContainsKey(setCode))
            {
                _dictionaryOfSets.Add(setCode, new Dictionary<string, ExpressionData>());
            }
        }

        public void AddExpressionToSet(ExpressionData expressionData, string setCode)
        {
            if (_dictionaryOfSets.ContainsKey(setCode))
            {
                var set = _dictionaryOfSets[setCode];
                if (!set.ContainsKey(expressionData.ExpressionCode))
                {
                    set.Add(expressionData.ExpressionCode, expressionData);
                }
            }
        }

        public void AddExpressionsToSet(List<ExpressionData> expressionsData, string setCode)
        {
            foreach (var expressionData in expressionsData)
            {
                AddExpressionToSet(expressionData, setCode);
            }
        }
    }
}
