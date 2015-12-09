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
        [DataMember][Obsolete("Use the DriveInformation array. This property will be removed from v2.0.0")]
        public int? FreeSpace { get; set; }
        [DataMember]
        public DriveInformation[] DriveInformation { get; set; }
    }

    [DataContract]
    public class DriveInformation
    {
        [DataMember]
        public string Drive { get; set; }
        [DataMember]
        public long FreeSpaceBytes { get; set; }
        [DataMember]
        public string FreeSpaceFormatted { get; set; }
    }
}