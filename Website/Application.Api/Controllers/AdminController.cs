using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Ninject.Infrastructure.Language;
using Raven.Client;

namespace website.Application.Api.Controllers
{
    [RoutePrefix("admin")]
    public class AdminController : ApiController
    {
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
