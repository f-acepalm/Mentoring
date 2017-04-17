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
        
        
        private const uint SuccessStatus = 0;

        public int GetBatteryState()
        {
            SystemBatteryState result;
            var outputBufferSize = Marshal.SizeOf(typeof(SystemBatteryState));
            uint resultStatus = CallNtPowerInformation(InformationLevels.SystemBatteryState, IntPtr.Zero, 0, out result, outputBufferSize);
            if (resultStatus != SuccessStatus)
            {
                throw new ContextMarshalException(); //не знаю точно, какой ексепшен тут должен быть, но очень хочется тут сгенерить ексепшен.
            }                                        //PS: Слава, эти комменты только для тебя, считай, что на продакшене их нет.

            double remaining = result.RemainingCapacity;
            double max = result.MaxCapacity;
            double percent = 0;
            if (result.BatteryPresent)
            {
                percent = remaining * 100 / max;
            }

            return (int)percent;
        }

        public SystemPowerInformation GetSystemPowerInformation()
        {
            SystemPowerInformation result;
            var outputBufferSize = Marshal.SizeOf(typeof(SystemPowerInformation));
            uint resultStatus = CallNtPowerInformation(InformationLevels.SystemPowerInformation, IntPtr.Zero, 0, out result, outputBufferSize);
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
            uint resultStatus = CallNtPowerInformation(InformationLevels.LastSleepTime, IntPtr.Zero, 0, out result, outputBufferSize);
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
            uint resultStatus = CallNtPowerInformation(InformationLevels.LastWakeTime, IntPtr.Zero, 0, out result, outputBufferSize);
            if (resultStatus != SuccessStatus)
            {
                throw new ContextMarshalException();
            }

            return result;
        }

        public void ReserveHibernationFile()
        {
            var inputBuffer = new[] { true };

            var result = CallNtPowerInformation(
                InformationLevels.SystemReserveHiberFile,
                inputBuffer,
                (uint)Marshal.SizeOf<bool>(),
                IntPtr.Zero,
                0);
            if (result != SuccessStatus)
                throw new ContextMarshalException();
            ////Int32 input = 1;
            //var pointer = Marshal.AllocHGlobal(sizeof(int));
            //Marshal.WriteInt32(pointer, 1);
            //var inputBufferSize = Marshal.SizeOf(typeof(Int32));
            //uint resultStatus = CallNtPowerInformation(SystemReserveHiberFileLevel, ref input, inputBufferSize, IntPtr.Zero, 0);
            ////Marshal.FreeHGlobal(pointer);
            //if (resultStatus != SuccessStatus)
            //{
            //    throw new ContextMarshalException();
            //}
        }
        
        [DllImport("powrprof.dll")]
        private static extern uint CallNtPowerInformation
        (
            InformationLevels informationLevel,
            IntPtr inputBuffer,
            int inputBufferSize,
            out SystemPowerInformation result,
            int outputBufferSize
        );

        [DllImport("powrprof.dll")]
        private static extern uint CallNtPowerInformation
        (
            InformationLevels informationLevel,
            IntPtr inputBuffer,
            int inputBufferSize,
            out UInt64 result,
            int outputBufferSize
        );

        [DllImport("powrprof.dll")]
        private static extern uint CallNtPowerInformation
        (
            InformationLevels informationLevel,
            IntPtr inputBuffer,
            int inputBufferSize,
            out SystemBatteryState result,
            int outputBufferSize
        );

        [DllImport("powrprof.dll", SetLastError = true)]
        public static extern uint CallNtPowerInformation(
            InformationLevels InformationLevel, 
            [In] bool[] lpInputBuffer,
            uint nInputBufferSize,
            IntPtr lpOutputBuffer,
            uint nOutputBufferSize);
    }
}
