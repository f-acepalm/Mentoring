using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTransformation.Transformers
{
    public class ParameterToConstantTransformer : ExpressionVisitor
    {
        public Dictionary<string, object> ParameterMapping { get; set; }

        public ParameterToConstantTransformer()
        {
            ParameterMapping = new Dictionary<string, object>();
        }

        public ParameterToConstantTransformer(Dictionary<string, object> parameterMapping)
        {
            ParameterMapping = parameterMapping;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return Expression.Lambda<T>(Visit(node.Body), node.Parameters);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            object value;
            if (ParameterMapping.TryGetValue(node.Name, out value))
            {
                return Expression.Constant(value);
            }

            return base.VisitParameter(node);
        }
    }
}
