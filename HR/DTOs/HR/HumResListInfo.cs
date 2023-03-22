using HR.Utils.Enumerate;
using System.Runtime.Serialization;

namespace HR.DTOs.HR
{
    [DataContract]
    public class HumResListInfo
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "sex")]
        public int Sex { get; set; }
        [DataMember(Name = "tel")]
        public string Tel { get; set; }
        [DataMember(Name = "src")]
        public string Src { get; set; }
        [DataMember(Name = "arrange")]
        public string Arrange { get; set; }
        [DataMember(Name = "status")]
        public string Status { get; set; }
        [DataMember(Name = "createAt")]
        public string CreateAt { get; set; }
        [DataMember(Name = "creator")]
        public string Creator { get; set; }
        [DataMember(Name = "modifyAt")]
        public string ModifyAt { get; set; }
        [DataMember(Name = "principal")]
        public string Principal { get; set; }
        [DataMember(Name = "isBlocked")]
        public bool IsBlocked { get; set; }
    }
}
