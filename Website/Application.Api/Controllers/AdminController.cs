using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Raven.Client;

namespace website.Application.Api.Controllers
{
    public class Configuration
    {
        public string Id { get; set; }
        public string Setting { get; set; }
    }

    public class ConfigRequest
    {
        public string Uri { get; set; }
    }

    [RoutePrefix("admin")]
    public class AdminController : ApiController
    {
        private readonly IDocumentSession _session;
        private const string TeamCityUrlId = "TeamCityUrl";

        public AdminController(IDocumentSession session)
        {
            _session = session;
        }

        [Route("config/tc/url")]
        public IHttpActionResult PostTeamCityUrl(ConfigRequest request)
        {
            if (request == null 
                || string.IsNullOrEmpty(request.Uri) 
                || !Uri.IsWellFormedUriString(request.Uri, UriKind.Absolute))
            {
                return BadRequest();
            }
            var config = _session.Load<Configuration>(TeamCityUrlId);
            config = config ?? new Configuration { Id = TeamCityUrlId};
            config.Setting = request.Uri;
            _session.Store(config);
            return Ok();
        }

        [Route("config/tc/url")]
        public IHttpActionResult GetTeamCityUrl()
        {
            var config = _session.Load<Configuration>(TeamCityUrlId);
            config = config ?? new Configuration {Id = TeamCityUrlId};
            return Ok(config.Setting);
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
