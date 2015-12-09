using System.Runtime.Serialization;

namespace MessageContracts.SystemInformation
{
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