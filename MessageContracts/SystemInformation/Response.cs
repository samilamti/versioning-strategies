using System;
using System.Runtime.Serialization;

namespace MessageContracts.SystemInformation
{
    [DataContract]
    public class Response
    {
        [DataMember]
        public DateTime? DateTime { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public int? FreeSpace { get; set; }
    }
}