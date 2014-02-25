namespace website.Application.Services.Configuration
{
    public interface ITeamCityConfigurationService
    {
        TeamCityConfig Load();
        void Save(TeamCityConfig config);
    }
}