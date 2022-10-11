using Assets.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LSB
{
    [Serializable]
    public class Expression
    {

        public string Word { get; set; }

        public IEnumerable<ExpressionCode> Codes { get; set; }

        public ExpressionType Type { get; set; }

        public Expression()
        {
            Type = ExpressionType.REGULAR;
        }

        public Expression(string code, string word)
        {
            var expressionCode = new ExpressionCode(code);
            Codes = new List<ExpressionCode>();
            Codes = Codes.Append(expressionCode);
            Type = code.Contains("#99") ? ExpressionType.TENSE : ExpressionType.REGULAR;
            Word = word;
        }

        public Expression(IEnumerable<string> codes, string word)
        {
            Type = codes.Any(c => c.Contains("#99")) ? ExpressionType.TENSE : ExpressionType.REGULAR;
            Word = word.Trim();
            Codes = codes.Select(c => new ExpressionCode(c));
        }

        public string GetStringList()
        {
            var res = Codes.Aggregate("", (current, next) => string.IsNullOrWhiteSpace(current) ? "" : $"{current}, {next}");
            return res;
        }

        public bool HasExpressionCode(string expressionCode)
        {
            return Codes.Any(code => code.WholeCode == expressionCode);
        }
         
        public bool IsPartOf(IEnumerable<string> expressionCodes)
        {
            var lista = Codes.ToList();
            var response = Codes.All(code => expressionCodes.Any(expressionCode => expressionCode.Contains(code.WholeCode)));
            return response;
        }
    }

    
}
