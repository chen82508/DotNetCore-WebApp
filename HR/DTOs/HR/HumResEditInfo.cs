using System.Runtime.Serialization;

namespace HR.DTOs.HR
{
    [DataContract]
    public class HumResEditInfo
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "sex")]
        public int Sex { get; set; }
        [DataMember(Name = "tel")]
        public string Tel { get; set; }
        [DataMember(Name = "email")]
        public string Email { get; set; }
        [DataMember(Name = "src")]
        public int Src { get; set; }
        [DataMember(Name = "vacancies")]
        public List<int> Vacancies { get; set; }
        [DataMember(Name = "oriStatus")]
        public int OriStatus { get; set; }
        [DataMember(Name = "status")]
        public int Status { get; set; }
        [DataMember(Name = "principal")]
        public string Principal { get; set; }
        [DataMember(Name = "interviews")]
        public List<HumItvw> Interviews { get; set; }
        [DataMember(Name = "reason")]
        public int Reason { get; set; }
        [DataMember(Name = "otherReason")]
        public string OtherReason { get; set; }
    }

    [DataContract]
    public class HumItvw
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }
        [DataMember(Name = "time")]
        public string Time { get; set; }
        [DataMember(Name = "content")]
        public string Content { get; set; }
        [DataMember(Name = "creator")]
        public string Creator { get; set; }
    }
}
