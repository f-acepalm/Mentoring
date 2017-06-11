using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class TestClass : ITestClass
    {
        [Inject]
        public ITestClassProperty Property { get; set; }

        [TestAspect]
        public int TestMethod(int a, int b)
        {
            return a + b;
        }
    }
}
