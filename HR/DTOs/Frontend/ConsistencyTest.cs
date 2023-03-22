using System.Runtime.Serialization;

namespace HR.DTOs.Frontend
{
    [DataContract]
    public class ConsistencyTest
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "choosen")]
        public string Choosen { get; set; }
        [DataMember(Name = "exclude")]
        public string Exculde { get; set; }
        [DataMember(Name = "sequence")]
        public string Sequence { get; set; }
        [DataMember(Name = "seconds")]
        public int Seconds { get; set; }
    }
}
