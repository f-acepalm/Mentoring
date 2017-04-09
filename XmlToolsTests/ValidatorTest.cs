using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlTools;
using System.Linq;

namespace XmlToolsTests
{
    [TestClass]
    public class ValidatorTest
    {
        private const string FilePath = @"..\..\books.xml";
        private const string SchemaPath = @"..\..\booksSchema.xsd";
        private const string InvalidFilePath = @"..\..\booksInvalid.xml";

        [TestMethod]
        public void Validate_valid()
        {
            var validator = new Validator(SchemaPath);
            var result = validator.DetectErrors(FilePath);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void Validate_invalid()
        {
            var validator = new Validator(SchemaPath);
            var result = validator.DetectErrors(InvalidFilePath);
            Assert.IsTrue(result.Any());
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
        }
    }
}
