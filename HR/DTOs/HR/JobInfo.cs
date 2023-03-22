using System.Runtime.Serialization;

namespace HR.DTOs.HR
{
    [DataContract]
    public class JobInfo
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "isOpen")]
        public bool IsOpen { get; set; }
        [DataMember(Name = "itvStart")]
        public string ItvStart { get; set; }
        [DataMember(Name = "itvEnd")]
        public string ItvEnd { get; set; }
        [DataMember(Name = "daysIndicator")]
        public int DaysIndicator { get; set; }
        [DataMember(Name = "categories")]
        public List<int> Categories { get; set; }
        [DataMember(Name = "company")]
        public int Company { get; set; }
        [DataMember(Name = "closeDate")]
        public DateTime? CloseDate { get; set; }
        [DataMember(Name = "closeReason")]
        public string? CloseReason { get; set; }
        [DataMember(Name = "history")]
        public List<string> History { get; set; }
    }
}
