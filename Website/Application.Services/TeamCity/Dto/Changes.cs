using System.Collections.Generic;
using System.Runtime.Serialization;

namespace website.Application.Services.TeamCity.Dto
{
    public class LastChanges
    {
        public LastChanges()
        {
            Change = new List<Change>();
        }
        
        [DataMember(Name = "count")]
        public int Count { get; set; }

        [DataMember(Name = "change")]
        public List<Change> Change { get; set; }
    }
}