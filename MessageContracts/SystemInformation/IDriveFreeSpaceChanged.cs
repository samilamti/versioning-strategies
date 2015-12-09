using System;

namespace MessageContracts.SystemInformation
{
    public interface IDriveFreeSpaceChanged
    {
        DriveInformation[] Drives { get; set; }
    }
}