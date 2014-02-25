using System.Runtime.Serialization;

namespace website.Application.Services.TeamCity.Dto
{
    [DataContract]
    public class VcsRootInstance
    {
        [DataMember(Name = "vcs-root-id")]
        public string VcsRootId { get; set; }
    }
}