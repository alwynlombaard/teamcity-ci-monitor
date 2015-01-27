namespace website.Application.Api.Dto
{
    public class BuildTypeModel
    {
        public BuildTypeModel()
        {
            LastBuild = new BuildModel();
        }
        public string Name { get; set; }
        public string BuildTypeId { get; set; }
        public BuildModel LastBuild { get; set; }
        public string NextUpdate { get; set; }
    }
}