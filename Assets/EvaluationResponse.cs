using LSB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    [Serializable]
    public class EvaluationResponse
    {
        public Expression Expression { get; private set; }

        private string _response;

        public string Response
        {
            get { return _response; }
            set 
            {
                var normalizedValue = value;
                normalizedValue = LocalParser.RemoveDiacritics(normalizedValue);
                normalizedValue = normalizedValue.Trim(' ').Trim('.');
                _response = normalizedValue; 
            }
        }

        public bool IsCorrect => LocalParser.RemoveDiacritics(Expression.Word.ToLower()).Equals(Response.ToLower());

        public bool IsAlreadyResponded => !string.IsNullOrWhiteSpace(Response);

        public EvaluationResponse(Expression expression)
        {
            Expression = expression;
        }

        public EvaluationResponse(Expression expression, string response)
        {
            Expression = expression;
            Response = response;
        }
    }
}
