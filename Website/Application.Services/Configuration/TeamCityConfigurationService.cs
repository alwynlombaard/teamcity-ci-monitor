using Newtonsoft.Json;
using Raven.Client;
using website.Application.Api.Controllers;
using website.Application.Infrastructure.DataProtection;

namespace website.Application.Services.Configuration
{
    public class TeamCityConfigurationService : ITeamCityConfigurationService
    {
        private const string TeamCityConfigKey = "TeamCityConfig";
        private readonly IDocumentSession _session;
        private readonly IProtector _protector;

        public TeamCityConfigurationService(IDocumentSession session, IProtector protector)
        {
            _session = session;
            _protector = protector;
        }

        public TeamCityConfig GetTeamCityConfig()
        {
            var config = _session.Load<Api.Controllers.Configuration>(TeamCityConfigKey);
            if (config.Value == null)
            {
                return new TeamCityConfig();
            }

            var teamcitySetting = JsonConvert.DeserializeObject<TeamCityConfig>(config.Value);
            teamcitySetting.Password = _protector.Unprotect(teamcitySetting.Password);
            return teamcitySetting;
        }
    }
}