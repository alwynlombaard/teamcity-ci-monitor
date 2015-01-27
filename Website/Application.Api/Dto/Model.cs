using System.Collections.Generic;

namespace website.Application.Api.Dto
{
    public class Model
    {
        public Model()
        {
            Projects = new List<ProjectModel>();
        }
        public  IList<ProjectModel> Projects;
    }
}