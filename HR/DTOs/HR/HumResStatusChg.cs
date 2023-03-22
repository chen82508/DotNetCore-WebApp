using System.Runtime.Serialization;

namespace HR.DTOs.HR
{
    [DataContract]
    public class HumResStatusChg
    {
        [DataMember(Name = "humanId")]
        public int HumanId { get; set; }
        [DataMember(Name = "origin")]
        public int Origin { get; set; }
        [DataMember(Name = "changed")]
        public int Changed { get; set; }
        [DataMember(Name = "reason")]
        public int Reason { get; set; }
    }
}
