using RestSharp;
using website.Application.Api.Controllers;
using website.Application.Services.TeamCity.Dto;

namespace website.Application.Services.TeamCity
{
    public class TeamCityRestSharpApiClient : ITeamCityApiClient
    {
        private readonly IApiClient _client;

        public TeamCityRestSharpApiClient(TeamCityConfig config)
        {
            _client = new RestSharpApiClient(new RestClient(config.Uri +  "/httpAuth/app/rest")
                {
                    Authenticator = new HttpBasicAuthenticator(config.UserName, config.Password),
                    Timeout = 500
                });
        }

        public BuildType GetBuildType(string id)
        {
            return _client.GetResponse<BuildType>("buildTypes/" + id);
        }

        public Build GetBuild(string id)
        {
            return _client.GetResponse<Build>("builds/" + id);
        }

        public Change GetChange(string id)
        {
            return _client.GetResponse<Change>("changes/" + id);
        }

        public VcsRoot GetVcsRoot(string id)
        {
            return _client.GetResponse<VcsRoot>("vcs-roots/id:" +  id);
        }

        public BuildTypes GetAllBuildTypes()
        {
            return _client.GetResponse<BuildTypes>("buildTypes");
        }

        public Projects GetAllProjects()
        {
            return _client.GetResponse<Projects>("projects");
        }

        public Changes GetChangesForBuild(string buildId)
        {
            return _client.GetResponse<Changes>("changes?locator=build:(id:" + buildId + ")");
        }

        public Builds GetLastBuildsForBuildType(string buildTypeId)
        {
            return _client.GetResponse<Builds>("buildTypes/" + buildTypeId + "/builds/?locator=count:1");
        }

        public Builds GetAllRunningBuilds()
        {
            return _client.GetResponse<Builds>("/builds/?locator=running:true");
        }
    }
}