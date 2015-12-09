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
        [DataMember]
        public DriveInformation[] DriveInformation { get; set; }
    }
}