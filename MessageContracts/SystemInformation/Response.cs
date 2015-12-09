using System;
using System.Runtime.Serialization;

namespace MessageContracts.SystemInformation
{
    [DataContract]
    public class Response
    {
        [DataMember]
        public string DateTime { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember][Obsolete("Use the DriveInformation array and/or the SystemDriveFreeBytes. This property will be removed from v2.0.0")]
        public int? FreeSpace { get; set; }
        [DataMember]
        public long? SystemDriveFreeBytes { get; set; }
        [DataMember]
        public DriveInformation[] DriveInformation { get; set; }
    }

    [DataContract]
    public class DriveInformation
    {
        public string Drive { get; set; }
        public long FreeSpaceBytes { get; set; }
        public string FreeSpaceFormatted { get; set; }
    }
}