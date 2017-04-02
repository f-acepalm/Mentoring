using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;
using ExpressionTransformation;
using ExpressionTransformation.Transformers;
using System.Collections.Generic;

namespace ExpressionTransformationTests
{
    [TestClass]
    public class ExpressionTransformerTest
    {
        [TestMethod]
        public void Transform_Test()
        {
            Expression<Func<int, int, int, int>> source_exp = (a, b, c) => a + (a + 1) * (a + 5) * (1 - a) * (a - 1) - b + (9 * b) + c;
            var transformer = new ExpressionTransformer(
                new AddToIncrementTransformer(),
                new SubtractToDecrementTransformer(),
                new ParameterToConstantTransformer(
                    new Dictionary<string, object>()
                    {
                        { "a", 1 },
                        { "c", 3 },
                    }));
            var result_exp = (transformer.Transform(source_exp));

            Console.WriteLine(source_exp + " " + source_exp.Compile().Invoke(1, 2, 3));
            Console.WriteLine(result_exp + " " + result_exp.Compile().Invoke(1, 2, 3));
        }
    }
}
