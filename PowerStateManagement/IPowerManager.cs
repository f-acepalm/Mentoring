using System;
using System.Runtime.InteropServices;

namespace PowerStateManagement
{
    [ComVisible(true)]
    [Guid("F66BE0CE-196F-4555-B866-184B564EEEDD")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IPowerManager
    {
        int GetBatteryState();
        
        DateTime GetLastSleepTime();

        DateTime GetLastWakeTime();

        void ReserveHibernationFile();

        void DeleteHibernationFile();

        void Suspend();

        uint GetIdleness();
    }
}
