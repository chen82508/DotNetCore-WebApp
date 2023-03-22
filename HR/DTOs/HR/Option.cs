using System.Runtime.Serialization;

namespace HR.DTOs.HR
{
    [DataContract]
    public class Option
    {
        [DataMember(Name = "tid")]
        public int TID { get; set; }
        [DataMember(Name = "id")]
        public int ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
