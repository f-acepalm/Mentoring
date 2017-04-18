using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using PowerStateManagement.Structures;

namespace PowerStateManagement
{
    [ComVisible(true)]
    [Guid("F1C2EDF1-A592-446C-9E51-6D5E57E57839")]
    [ClassInterface(ClassInterfaceType.None)]
    public class PowerManager : IPowerManager
    {
        private const uint SuccessStatus = 0;

        public int GetBatteryState()
        {
            SystemBatteryState result;
            var outputBufferSize = Marshal.SizeOf(typeof(SystemBatteryState));
            uint resultStatus = CallNtPowerInformation(InformationLevels.SystemBatteryState, IntPtr.Zero, 0, out result, outputBufferSize);
            if (resultStatus != SuccessStatus)
            {
                throw new Win32Exception((int)resultStatus);
            }

            double remaining = result.RemainingCapacity;
            double max = result.MaxCapacity;
            double percent = 0;
            if (result.BatteryPresent)
            {
                percent = remaining * 100 / max;
            }

            return (int)percent;
        }
                
        public DateTime GetLastSleepTime()
        {
            return GetSleepInformation(InformationLevels.LastSleepTime);
        }

        public DateTime GetLastWakeTime()
        {
            return GetSleepInformation(InformationLevels.LastWakeTime);
        }

        public void ReserveHibernationFile()
        {
            ReserveHibernationFile(true);
        }

        public void DeleteHibernationFile()
        {
            ReserveHibernationFile(false);
        }

        public void Suspend()
        {
            SetSuspendState(false, false, false);
        }

        public uint GetIdleness()
        {
            return GetSystemPowerInformation().Idleness;
        }

        [DllImport("powrprof.dll", SetLastError = true)]
        private static extern uint CallNtPowerInformation
        (
            InformationLevels informationLevel,
            IntPtr inputBuffer,
            int inputBufferSize,
            out SystemPowerInformation result,
            int outputBufferSize
        );

        [DllImport("powrprof.dll", SetLastError = true)]
        private static extern uint CallNtPowerInformation
        (
            InformationLevels informationLevel,
            IntPtr inputBuffer,
            int inputBufferSize,
            out UInt64 result,
            int outputBufferSize
        );

        [DllImport("powrprof.dll", SetLastError = true)]
        private static extern uint CallNtPowerInformation
        (
            InformationLevels informationLevel,
            IntPtr inputBuffer,
            int inputBufferSize,
            out SystemBatteryState result,
            int outputBufferSize
        );

        [DllImport("powrprof.dll", SetLastError = true)]
        private static extern uint CallNtPowerInformation
        (
            InformationLevels InformationLevel,
            IntPtr inputBuffer,
            int inputBufferSize,
            IntPtr result,
            int outputBufferSize
        );

        [DllImport("powrprof.dll", SetLastError = true)]
        private static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        [DllImport("kernel32")]
        public static extern ulong GetTickCount64();

        private DateTime GetSleepInformation(InformationLevels level)
        {
            UInt64 result;
            var outputBufferSize = Marshal.SizeOf(typeof(UInt64));
            uint resultStatus = CallNtPowerInformation(level, IntPtr.Zero, 0, out result, outputBufferSize);
            if (resultStatus != SuccessStatus)
            {
                throw new Win32Exception((int)resultStatus);
            }

            var systemStartupTime = GetTickCount64() * 10000;

            return DateTime.Now - TimeSpan.FromTicks((long)systemStartupTime) + TimeSpan.FromTicks((long)result);
        }

        private void ReserveHibernationFile(bool shoulReserve)
        {
            int inputBufferSize = Marshal.SizeOf(typeof(int));
            IntPtr input = Marshal.AllocHGlobal(inputBufferSize);
            Marshal.WriteInt32(input, 0, shoulReserve ? 1 : 0);
            uint resultStatus = CallNtPowerInformation(InformationLevels.SystemReserveHiberFile, input, inputBufferSize, IntPtr.Zero, 0);
            Marshal.FreeHGlobal(input);
            if (resultStatus != SuccessStatus)
            {
                throw new Win32Exception((int)resultStatus);
            }
        }

        private SystemPowerInformation GetSystemPowerInformation()
        {
            SystemPowerInformation result;
            var outputBufferSize = Marshal.SizeOf(typeof(SystemPowerInformation));
            uint resultStatus = CallNtPowerInformation(InformationLevels.SystemPowerInformation, IntPtr.Zero, 0, out result, outputBufferSize);
            if (resultStatus != SuccessStatus)
            {
                throw new Win32Exception((int)resultStatus);
            }

            return result;
        }
    }
}
