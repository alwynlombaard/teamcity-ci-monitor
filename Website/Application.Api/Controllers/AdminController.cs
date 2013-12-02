using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using Ninject.Infrastructure.Language;
using Raven.Client;

namespace website.Application.Api.Controllers
{
    public class Configuration
    {
        public string TeamcityUrl { get; set; }
    }

    [RoutePrefix("admin")]
    public class AdminController : ApiController
    {
        private readonly IDocumentSession _session;

        public AdminController(IDocumentSession session)
        {
            _session = session;
        }

        [Route("config/tc/url")]
        public HttpStatusCodeResult PostTeamCityUrl(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var config = _session.Query<Configuration>()
                .AsProjection<Configuration>()
                .FirstOrDefault();
            config = config ?? new Configuration();
            config.TeamcityUrl = uri;
            _session.Store(config);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [Route("appkeys/generate/{length:int}")]
        public string GetGeneratedKey(int length)
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

            return sb.ToString();
        }
    }
}
