namespace website.Application.Services.TeamCity.Dto
{
    public class Build
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string StatusText { get; set; }
        public string Number { get; set; }
        public string BuildTypeId { get; set; }
        public BuildType BuildType { get; set; }
        public Changes Changes { get; set; }
        public string FinishDate { get; set; }
        public int PercentageComplete { get; set; }
        public string WebUrl { get; set; }
    }
}