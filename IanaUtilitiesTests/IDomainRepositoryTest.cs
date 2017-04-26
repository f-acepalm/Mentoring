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
                Console.WriteLine($"{item.Name} : {item.WhoisServerName}");
            }
        }

        [TestMethod]
        public void DomainRepositoryTPL()
        {
            var repository = new DomainRepositoryTPL();
            var domains = repository.GetAll();
            foreach (var item in domains)
            {
                Console.WriteLine($"{item.Name} : {item.WhoisServerName}");
            }
        }

        [TestMethod]
        public void DomainRepositoryThreadPool()
        {
            var repository = new DomainRepositoryThreadPool();
            var domains = repository.GetAll();
            foreach (var item in domains)
            {
                Console.WriteLine($"{item.Name} : {item.WhoisServerName}");
            }
        }
    }
}
