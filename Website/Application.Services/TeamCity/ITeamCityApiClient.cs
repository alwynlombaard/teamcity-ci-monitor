using System.Threading.Tasks;
using website.Application.Services.TeamCity.Dto;

namespace website.Application.Services.TeamCity
{
    public interface ITeamCityApiClient
    {
        BuildType GetBuildType(string id);
        Build GetBuild(string id);
        Change GetChange(string id);
        VcsRoot GetVcsRoot(string id);

        BuildTypes GetAllBuildTypes();
        Projects GetAllProjects();
        Builds GetAllRunningBuilds();

        Changes GetChangesForBuild(string buildId);
        
        Builds GetLastBuildsForBuildType(string buildTypeId);
        Task<Builds> GetLastBuildsForBuildTypeAsync(string buildTypeId);
    }
}