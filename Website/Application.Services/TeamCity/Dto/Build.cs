using System.Runtime.Serialization;

namespace website.Application.Services.TeamCity.Dto
{
    [DataContract]
    public class Build
    {
        public Build()
        {
            LastChanges = new LastChanges();
        }

       [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "statusText")]
        public string StatusText { get; set; }


        [DataMember(Name = "number")]
        public string Number { get; set; }

         [DataMember(Name = "buildTypeId")]
        public string BuildTypeId { get; set; }

        [DataMember(Name = "buildType")]
        public BuildType BuildType { get; set; }

        [DataMember(Name = "lastChanges")]
        public LastChanges LastChanges { get; set; }

        [DataMember(Name = "startDate")]
        public string StartDate { get; set; }

        [DataMember(Name = "finishDate")]
        public string FinishDate { get; set; }
        
        public int PercentageComplete { get; set; }

        [DataMember(Name = "webUrl")]
        public string WebUrl { get; set; }
        
        [DataMember(Name = "running-info")]
        public RunningInfo RunningInfo { get; set; }
    }

    [DataContract]
    public class RunningInfo
    {
        [DataMember(Name = "percentageComplete")]
        public int PercentageComplete { get; set; }

        [DataMember(Name = "currentStageText")]
        public string CurrentStageText { get; set; }

    }
}