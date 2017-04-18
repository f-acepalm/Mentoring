using PowerStateManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp // Этот проект создан для быстрой проверки чего-нибудь. Можно сказать, что его не существует в реальном мире.
{
    class Program
    {
        static void Main(string[] args)
        {
            //SYSTEM_POWER_INFORMATION spi;
            //uint retval = CallNtPowerInformation(
            //    SystemPowerInformation,
            //    IntPtr.Zero,
            //    0,
            //    out spi,
            //    Marshal.SizeOf(typeof(SYSTEM_POWER_INFORMATION))
            //);
            //if (retval == STATUS_SUCCESS)
            //    Console.WriteLine(spi.TimeRemaining);
            var powerManager = new PowerManager();

            var x = powerManager.GetBatteryState();
            //powerManager.Suspend();
            //Console.WriteLine(x);
            //Console.ReadLine();
        }
    }
}
