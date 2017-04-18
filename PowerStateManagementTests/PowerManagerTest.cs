using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerStateManagement;

namespace PowerStateManagementTests
{
    [TestClass]
    public class PowerManagerTest
    {
        [TestMethod]
        public void GetBatteryState()
        {
            var powerManager = new PowerManager();
            var actual = powerManager.GetBatteryState();
            Console.WriteLine(actual);
        }

        [TestMethod]
        public void GetLastSleepTime()
        {
            var powerManager = new PowerManager();
            var actual = powerManager.GetLastSleepTime();
            Console.WriteLine(actual);
        }

        [TestMethod]
        public void GetLastWakeTime()
        {
            var powerManager = new PowerManager();
            var actual = powerManager.GetLastWakeTime();
            Console.WriteLine(actual);
        }

        [TestMethod]
        public void ReserveHibernationFile()
        {
            var powerManager = new PowerManager();
            powerManager.ReserveHibernationFile();            
        }

        [TestMethod]
        public void DeleteHibernationFile()
        {
            var powerManager = new PowerManager();
            powerManager.DeleteHibernationFile();            
        }

        [TestMethod]
        public void GetIdleness()
        {
            var powerManager = new PowerManager();
            var actual = powerManager.GetIdleness();
            Console.WriteLine(actual);
        }

        //[TestMethod]
        //public void Suspend()
        //{
        //    var powerManager = new PowerManager();
        //    powerManager.Suspend();
        //}
    }
}
