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
                value = LocalParser.RemoveDiacritics(value);
                value = value.Trim('.');
                _response = value; 
            }
        }

        public bool IsCorrect => Expression.Word.ToLower().Equals(Response.ToLower());

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
