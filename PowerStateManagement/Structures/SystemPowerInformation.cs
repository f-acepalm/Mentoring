using System;

namespace PowerStateManagement.Structures
{
    public struct SystemPowerInformation
    {
        public UInt32 MaxIdlenessAllowed;
        public UInt32 Idleness;
        public UInt32 TimeRemaining;
        public byte CoolingMode;
    }
}
