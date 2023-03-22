using System.Runtime.Serialization;

namespace HR.DTOs.HR
{
    [DataContract]
    public class MessageTag
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
    }
}
