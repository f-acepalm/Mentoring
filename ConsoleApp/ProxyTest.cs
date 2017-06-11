using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class ProxyTest : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var x = invocation.Arguments;
            invocation.Proceed();
        }
    }
}
