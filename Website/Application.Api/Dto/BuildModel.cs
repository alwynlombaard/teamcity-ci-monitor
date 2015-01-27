using website.Application.Services.TeamCity.Dto;

namespace website.Application.Api.Dto
{
    public class BuildModel
    {
        public BuildModel()
        {
            Status = "UNKNOWN";
        }
        public int Id { get; set; }
        public string Status { get; set; }
        public string StatusText { get; set; }
        public string Number { get; set; }
        public string BuildTypeId { get; set; }
        public string FinishDate { get; set; }
        public string StartDate { get; set; }
        public string WebUrl { get; set; }
        public RunningInfoModel RunningInfo { get; set; }
        public string LastChange { get; set; }
    }
}