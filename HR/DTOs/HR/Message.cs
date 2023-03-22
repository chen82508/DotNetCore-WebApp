using System.Runtime.Serialization;

namespace HR.DTOs.HR
{
    [DataContract]
    public class Message
    {
        [DataMember(Name = "cid")]
        public int CID { get; set; }
        [DataMember(Name = "id")]
        public int ID { get; set; }
        [DataMember(Name = "subject")]
        public string Subject { get; set; }
        [DataMember(Name = "content")]
        public string Content { get; set; }
    }
}
