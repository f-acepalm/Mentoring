using System;
using System.Runtime.InteropServices;

namespace PowerStateManagement.Structures
{
    public struct SystemBatteryState
    {
        //[MarshalAs(UnmanagedType.Bool)]
        public bool AcOnLine;
        public bool BatteryPresent;
        public bool Charging;
        public bool Discharging;
        public bool Spare1;
        public byte Tag;
        public uint MaxCapacity;
        public uint RemainingCapacity;
        public uint Rate;
        public uint EstimatedTime;
        public uint DefaultAlert1;
        public uint DefaultAlert2;
    }
}
