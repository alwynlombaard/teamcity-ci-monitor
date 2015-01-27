using System.Collections.Generic;

namespace website.Application.Api.Dto
{
    public class RunningBuidlsModel
    {
        public RunningBuidlsModel()
        {
            Builds = new List<BuildModel>();
        }
        public IList<BuildModel> Builds { get; set; }
    }
}