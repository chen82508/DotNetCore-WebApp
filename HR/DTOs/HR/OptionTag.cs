using System.Runtime.Serialization;

namespace HR.DTOs.HR
{
    [DataContract]
    public class OptionTag
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
