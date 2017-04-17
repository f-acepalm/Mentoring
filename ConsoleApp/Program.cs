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
        const int SystemPowerInformation = 12;
        const uint STATUS_SUCCESS = 0;

        struct SYSTEM_POWER_INFORMATION
        {
            public uint MaxIdlenessAllowed;
            public uint Idleness;
            public uint TimeRemaining;
            public byte CoolingMode;
        }

        [DllImport("powrprof.dll")]
        static extern uint CallNtPowerInformation(
            int informationLevel,
            IntPtr inputBuffer,
            int inputBufferSize,
            out SYSTEM_POWER_INFORMATION spi,
            int outputBufferSize
        );

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
            Console.WriteLine(x);
            Console.ReadLine();
        }
    }
}
