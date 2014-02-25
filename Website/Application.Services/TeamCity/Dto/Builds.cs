using System.Collections.Generic;

namespace website.Application.Services.TeamCity.Dto
{
    public class Builds
    {
        public int Count { get; set; }
        public IList<Build> Build { get; set; }
    }
}