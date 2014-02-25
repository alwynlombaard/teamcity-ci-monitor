using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using website.Application.Services.Configuration;

namespace website.Application.Api.Controllers
{
    [RoutePrefix("admin")]
    public class AdminController : ApiController
    {
        private readonly ITeamCityConfigurationService _teamCityConfigurationService;

        public AdminController(ITeamCityConfigurationService teamCityConfigurationService)
        {
            _teamCityConfigurationService = teamCityConfigurationService;
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
            
            _teamCityConfigurationService.Save(request);
            
            return Ok();
        }

        [Route("config/tc")]
        public IHttpActionResult GetTeamCityConfig()
        {
            return Ok(_teamCityConfigurationService.Load());
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
