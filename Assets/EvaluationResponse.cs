using LSB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class EvaluationResponse
    {
        public Expression Expression { get; private set; }

        public string Response { get; set; }

        public bool IsCorrect => Expression.getWord().Equals(Response);

        public bool IsAlreadyResponded => string.IsNullOrWhiteSpace(Response);

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
