using System.Runtime.Serialization;

namespace HR.DTOs.Frontend
{
    [DataContract]
    public class ResumeAttachment
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "seq")]
        public int Seq { get; set; }
        [DataMember(Name = "file")]
        public byte[]? File { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "extension")]
        public string Extension { get; set; }
        [DataMember(Name = "contentType")]
        public string contentType { get; set; }
    }
}
