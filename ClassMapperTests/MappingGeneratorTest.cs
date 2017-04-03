using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassMapper;
using System.Collections.Generic;

namespace ClassMapperTests
{
    [TestClass]
    public class MappingGeneratorTest
    {
        [TestMethod]
        public void Map_Test()
        {
            var mappingGenerator = new MappingGenerator();
            var mapper = mappingGenerator.Generate<Foo, Bar>();
            var expected = new Foo(1)
            {
                SomeInt = 1,
                SomeString = "test",
                SomeObject = new { Test = 10, TestStr = "a" }
            };

            var actual = mapper.Map(expected);

            Assert.AreEqual(expected.SomeInt, actual.SomeInt);
            Assert.AreEqual(expected.SomeString, actual.SomeString);
            Assert.AreEqual(expected.SomeObject, actual.SomeObject);
            Assert.AreNotEqual(expected.SomeIntField, actual.SomeIntField);
        }

        [TestMethod]
        public void Map_Custom_Test()
        {
            var mappingGenerator = new MappingGenerator();
            var mapper = mappingGenerator.Generate<Foo, Bar>(
                new Dictionary<string, string>()
                {
                    { "FooProperty", "BarProperty" }
                });
            var expected = new Foo(1)
            {
                FooProperty = 1,
                SomeString = "test",
                SomeObject = new { Test = 10, TestStr = "a" }
            };

            var actual = mapper.Map(expected);

            Assert.AreEqual(expected.FooProperty, actual.BarProperty);
            Assert.AreNotEqual(expected.SomeIntField, actual.SomeIntField);
        }
    }
}
