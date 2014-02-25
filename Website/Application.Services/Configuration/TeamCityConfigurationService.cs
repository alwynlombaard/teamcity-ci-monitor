using Newtonsoft.Json;
using website.Application.Infrastructure.Store;

namespace website.Application.Services.Configuration
{
    public class TeamCityConfigurationService : ITeamCityConfigurationService
    {
        private readonly IStore _store;
        private const string TeamCityConfigKey = "TeamCityConfig";
        
        public TeamCityConfigurationService(IStore store)
        {
            _store = store;
            
        }

        public TeamCityConfig Load()
        {
            var config = _store.Get(TeamCityConfigKey);
            
            return string.IsNullOrEmpty(config)
                ? new TeamCityConfig { Uri="http://localhost"} 
                : JsonConvert.DeserializeObject<TeamCityConfig>(config);
        }

        public void Save(TeamCityConfig config)
        {
            if (config == null)
            {
                return;
            }

            _store.Save(TeamCityConfigKey, JsonConvert.SerializeObject(config));
        }
    }
}