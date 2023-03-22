using System.Runtime.Serialization;

namespace HR.DTOs.Frontend
{
    [DataContract]
    public class ResumeForm
    {
        [DataMember(Name = "id")]
        public string? Id { get; set; }
        [DataMember(Name = "baseInfo")]
        public string BaseInfo { get; set; }
        [DataMember(Name = "experiences")]
        public string Experiences { get; set; }
        [DataMember(Name = "status")]
        public string Status { get; set; }
        [DataMember(Name = "QA")]
        public string QA { get; set; }
        [DataMember(Name = "photo")]
        public byte[]? Photo { get; set; }
        [DataMember(Name = "contentType")]
        public string? ContentType { get; set; }
        [DataMember(Name = "date")]
        public string Date { get; set; }
    }
}
