﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTransformation.Transformers
{
    public class SubtractToDecrementTransformer : ExpressionVisitor
    {
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.Subtract)
            {
                ParameterExpression param = null;
                ConstantExpression constant = null;
                if (node.Left.NodeType == ExpressionType.Parameter)
                {
                    param = (ParameterExpression)node.Left;
                }

                if (node.Right.NodeType == ExpressionType.Constant)                
                {
                    constant = (ConstantExpression)node.Right;
                }

                if (param != null && constant != null && constant.Type == typeof(int) && (int)constant.Value == 1)
                {
                    return Expression.Decrement(param);
                }
            }

            return base.VisitBinary(node);
        }
    }
}
