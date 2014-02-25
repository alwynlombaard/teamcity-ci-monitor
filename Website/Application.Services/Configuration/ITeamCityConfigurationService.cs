using website.Application.Api.Controllers;

namespace website.Application.Services.Configuration
{
    public interface ITeamCityConfigurationService
    {
        TeamCityConfig GetTeamCityConfig();
    }
}