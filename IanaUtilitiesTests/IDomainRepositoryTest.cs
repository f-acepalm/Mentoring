using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IanaUtilities;

namespace IanaUtilitiesTests
{
    [TestClass]
    public class IDomainRepositoryTest
    {
        [TestMethod]
        public void DomainRepositoryAsync()
        {
            var repository = new DomainRepositoryAsync();
            var domains = repository.GetAll();
            foreach (var item in domains)
            {
                Console.WriteLine($"{item.Name} : {item.WHOISServerName}");
            }
        }
    }
}
