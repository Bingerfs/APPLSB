using LSB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    [Serializable]
    public class ExpressionListReqResponse
    {
        public List<ExpressionJson> tokens;

        public ExpressionListReqResponse()
        {

        }

        public IEnumerable<Expression> ToDomainObject()
        {
            return tokens.Select(t => 
            {
                var expression = new Expression(t.code, t.word);
                return expression;
            });
        }
    }
}
