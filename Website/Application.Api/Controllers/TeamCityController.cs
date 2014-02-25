using System.Web.Http;
using Raven.Client;
using website.Application.Services.TeamCity;

namespace website.Application.Api.Controllers
{
    [RoutePrefix("tc")]
    public class TeamCityController : ApiController
    {
        private readonly IDocumentSession _session;
        private readonly ITeamCityApiClient _apiClient;

        public TeamCityController(IDocumentSession session, ITeamCityApiClient apiClient)
        {
            _session = session;
            _apiClient = apiClient;
        }

        [Route("test")]
        public IHttpActionResult GetTest()
        {
            var projects = _apiClient.GetAllProjects();
            return Ok(projects);
        }
    }
}