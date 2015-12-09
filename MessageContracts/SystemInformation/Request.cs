using System.Runtime.Serialization;

namespace MessageContracts.SystemInformation
{
    [DataContract]
    public class Request
    {
        [DataMember]
        public bool IncludeDateTime { get; set; }
        [DataMember]
        public string DateTimeFormat { get; set; }
        [DataMember]
        public bool IncludeUserName { get; set; }
        [DataMember]
        public string[] IncludeInformationForDrives { get; set; }
    }
}
