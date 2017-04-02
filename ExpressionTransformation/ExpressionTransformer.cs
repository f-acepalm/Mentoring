using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTransformation
{
    public class ExpressionTransformer
    {
        public List<ExpressionVisitor> Transformers { get; set; }

        public ExpressionTransformer()
        {
            Transformers = new List<ExpressionVisitor>();
        }

        public ExpressionTransformer(ExpressionVisitor transformer)
        {
            Transformers = new List<ExpressionVisitor>() { transformer };
        }

        public ExpressionTransformer(params ExpressionVisitor[] transformers)
        {
            Transformers = transformers.ToList();
        }

        public Expression<TDelegate> Transform<TDelegate>(Expression<TDelegate> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            var result = source;

            foreach (var item in Transformers)
            {
                result = item.VisitAndConvert(result, "");
            }

            return result;
        }
    }
}
