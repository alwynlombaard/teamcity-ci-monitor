using System;
using System.Threading.Tasks;
using RestSharp;
using website.Application.Services.Configuration;
using website.Application.Services.TeamCity.Dto;

namespace website.Application.Services.TeamCity
{
    public class TeamCityRestSharpApiClient : ITeamCityApiClient
    {
        private readonly IApiClient _client;

        public TeamCityRestSharpApiClient(TeamCityConfig config)
        {
            var baseUri = new Uri(config.Uri);

            var apiUri = !string.IsNullOrWhiteSpace(config.UserName) 
                ? new Uri(baseUri, "/httpAuth/app/rest") 
                : new Uri(baseUri, "/guestAuth/app/rest");

            var c = new RestClient(apiUri.ToString());
            if (!string.IsNullOrWhiteSpace(config.UserName))
            {
                c.Authenticator = new HttpBasicAuthenticator(config.UserName, config.Password);
            }
            _client = new RestSharpApiClient(c);
        }

        public BuildType GetBuildType(string id)
        {
            return _client.GetResponse<BuildType>("buildTypes/" + id);
        }

        public Build GetBuild(string id)
        {
            return _client.GetResponse<Build>("builds/" + id);
        }
        
        public Task<Build> GetBuildAsync(string id)
        {
            return  Task.Run(() => GetBuild(id));
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

        public Builds GetLastBuildsForBuildType(string buildTypeId)
        {
            return _client.GetResponse<Builds>("buildTypes/" + buildTypeId + "/builds/?locator=count:1");
        }

        public Task<Builds> GetLastBuildsForBuildTypeAsync(string buildTypeId)
        {
            return _client.GetResponseAsync<Builds>("buildTypes/" + buildTypeId + "/builds/?locator=count:1");
        }

        public Builds GetAllRunningBuilds()
        {
            return _client.GetResponse<Builds>("/builds/?locator=running:true");
        }
    }
}