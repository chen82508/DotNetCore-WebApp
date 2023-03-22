using System.Runtime.Serialization;

namespace HR.DTOs.HR
{
    [DataContract]
    public class StatisticData
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "itvw")]
        public int Itvw { get; set; }
        [DataMember(Name = "adm")]
        public int Adm { get; set; }
        [DataMember(Name = "chkIn")]
        public int ChkIn { get; set; }
        [DataMember(Name = "isOpened")]
        public bool IsOpened { get; set; }
    }

    public class Vacancy
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }
        [DataMember(Name = "name")]
        public int Name { get; set; }
    }

    public class VacancyStatistic
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }
        [DataMember(Name = "status")]
        public int Status { get; set; }
    }
}
