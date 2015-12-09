using System;

namespace MessageContracts.SystemInformation
{
    public interface IDriveFreeSpaceChanged
    {
        [Obsolete("Use Drives array. This will be removed from v2.0.0")]
        int FreeSpace { get; set; }

        DriveInformation[] Drives { get; set; }
    }
}