using System.Runtime.Serialization;

namespace HR.DTOs.HR
{
    [DataContract]
    public class UserInfo
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "password")]
        public string Password { get; set; }
        [DataMember(Name = "email")]
        public string Email { get; set; }
    }
}
