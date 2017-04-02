using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMapperTests
{
    public class Foo
    {
        private int _someInt;

        public Foo() { }

        public Foo(int field)
        {
            _someInt = field;
        }
        
        public int SomeInt { get; set; }

        public string SomeString { get; set; }

        public object SomeObject { get; set; }

        public int FooProperty { get; set; }

        public int SomeIntField
        {
            get
            {
                return _someInt;
            }
        }
    }
}
