using PowerStateManagement.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PowerStateManagement
{
    public class PowerManager
    {
        private const int SystemPowerInformationLevel = 12;
        private const int LastWakeTimeLevel = 14;
        private const int LastSleepTimeLevel = 15;
        private const int SystemBatteryStateLevel = 5; 
        private const uint SuccessStatus = 0;

        public int GetBatteryState()
        {
            SystemBatteryState result;
            var outputBufferSize = Marshal.SizeOf(typeof(SystemBatteryState));
            uint resultStatus = CallNtPowerInformation(SystemBatteryStateLevel, IntPtr.Zero, 0, out result, outputBufferSize);
            if (resultStatus != SuccessStatus)
            {
                throw new ContextMarshalException(); //не знаю точно, какой ексепшен тут должен быть.
            }

            double remaining = result.RemainingCapacity;
            double max = result.MaxCapacity;
            double percent = remaining * 100 / max; // /0!

            return (int)percent;
        }

        public SystemPowerInformation GetSystemPowerInformation()
        {
            SystemPowerInformation result;
            var outputBufferSize = Marshal.SizeOf(typeof(SystemPowerInformation));
            uint resultStatus = CallNtPowerInformation(SystemPowerInformationLevel, IntPtr.Zero, 0, out result, outputBufferSize);
            if (resultStatus != SuccessStatus)
            {
                throw new ContextMarshalException();
            }

            return result;
        }

        public UInt64 GetLastSleepTime()
        {
            UInt64 result;
            var outputBufferSize = Marshal.SizeOf(typeof(UInt64));
            uint resultStatus = CallNtPowerInformation(LastSleepTimeLevel, IntPtr.Zero, 0, out result, outputBufferSize);
            if (resultStatus != SuccessStatus)
            {
                throw new ContextMarshalException();
            }

            return result;
        }

        public UInt64 GetLastWakeTime()
        {
            UInt64 result;
            var outputBufferSize = Marshal.SizeOf(typeof(UInt64));
            uint resultStatus = CallNtPowerInformation(LastWakeTimeLevel, IntPtr.Zero, 0, out result, outputBufferSize);
            if (resultStatus != SuccessStatus)
            {
                throw new ContextMarshalException();
            }

            return result;
        }

        [DllImport("powrprof.dll")]
        private static extern uint CallNtPowerInformation
        (
            int informationLevel,
            IntPtr inputBuffer,
            int inputBufferSize,
            out SystemPowerInformation result,
            int outputBufferSize
        );

        [DllImport("powrprof.dll")]
        private static extern uint CallNtPowerInformation
        (
            int informationLevel,
            IntPtr inputBuffer,
            int inputBufferSize,
            out UInt64 result,
            int outputBufferSize
        );

        [DllImport("powrprof.dll")]
        private static extern uint CallNtPowerInformation
        (
            int informationLevel,
            IntPtr inputBuffer,
            int inputBufferSize,
            out SystemBatteryState result,
            int outputBufferSize
        );
    }
}
