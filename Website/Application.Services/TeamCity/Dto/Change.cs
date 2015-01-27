using System.Runtime.Serialization;

namespace website.Application.Services.TeamCity.Dto
{
    public class Change
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "username")]
        public string Username { get; set; }

        [DataMember(Name = "version")]
        public string Version { get; set; }
    }
}