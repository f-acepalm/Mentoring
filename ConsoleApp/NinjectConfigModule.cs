using Castle.DynamicProxy;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class NinjectConfigModule : NinjectModule
    {
        public override void Load()
        {
            var generator = new ProxyGenerator();

            Bind<ITestClassProperty>().ToMethod(context => new TestClassProperty() { Name = "test" });
            Bind<ITestClass>().ToMethod(context => generator.CreateInterfaceProxyWithTarget<ITestClass>(new TestClass(), new ProxyTest()));
        }
    }
}
