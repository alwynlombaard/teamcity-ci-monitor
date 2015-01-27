using System.Collections.Generic;

namespace website.Application.Api.Dto
{
    public class BuildTypeRowModel
    {
        public BuildTypeRowModel()
        {
            BuildTypes = new List<BuildTypeModel>();
        }
        public IList<BuildTypeModel> BuildTypes { get; set; }
    }
}