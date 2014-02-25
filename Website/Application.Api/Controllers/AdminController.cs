using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;
using Raven.Client;
using website.Application.Infrastructure.DataProtection;

namespace website.Application.Api.Controllers
{
    [RoutePrefix("admin")]
    public class AdminController : ApiController
    {
        private readonly IDocumentSession _session;
        private readonly IProtector _protector;
        private const string TeamCityConfigKey = "TeamCityConfig";

        public AdminController(IDocumentSession session, IProtector protector)
        {
            _session = session;
            _protector = protector;
        }

        [Route("config/tc")]
        public IHttpActionResult PostTeamCityConfig(TeamCityConfig request)
        {
            if (request == null 
                || string.IsNullOrEmpty(request.Uri) 
                || !Uri.IsWellFormedUriString(request.Uri, UriKind.Absolute))
            {
                return BadRequest();
            }
            request.Password = _protector.Protect(request.Password);
            
            var config = _session.Load<Configuration>(TeamCityConfigKey);
            config = config ?? new Configuration { Id = TeamCityConfigKey};
            config.Value = JsonConvert.SerializeObject(request);
            
            _session.Store(config);
            
            return Ok();
        }

        [Route("config/tc")]
        public IHttpActionResult GetTeamCityConfig()
        {
            var config = _session.Load<Configuration>(TeamCityConfigKey);
            config = config ?? new Configuration {Id = TeamCityConfigKey};

            if (config.Value == null)
            {
                return Ok();
            }

            var teamcitySetting = JsonConvert.DeserializeObject<TeamCityConfig>(config.Value);
            teamcitySetting.Password = _protector.Unprotect(teamcitySetting.Password);
            return Ok(teamcitySetting);
        }

        [Route("appkeys/generate/{length:int}")]
        public IHttpActionResult GetGeneratedKey(int length)
        {
            if (length > 2048)
            {
                length = 2048;
            }
            var buff = new byte[length / 2];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(buff);
            }

            var sb = new StringBuilder(length);
            foreach (var value in buff)
            {
                sb.Append(string.Format("{0:X2}", value));
            }

            return Ok(sb.ToString());
        }
    }
}
