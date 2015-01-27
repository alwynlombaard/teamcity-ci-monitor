using System.Collections.Generic;

namespace website.Application.Api.Dto
{
    public class ProjectModel
    {
        public ProjectModel()
        {
            BuildTypeRows = new List<BuildTypeRowModel>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public IList<BuildTypeRowModel> BuildTypeRows { get; set; }
    }
}