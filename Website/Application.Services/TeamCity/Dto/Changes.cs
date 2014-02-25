using System.Collections.Generic;

namespace website.Application.Services.TeamCity.Dto
{
    public class Changes
    {
        public int Count { get; set; }
        public IList<Change> Change { get; set; } 
    }
}