using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace website.Application.Infrastructure.DataProtection
{
    public class DataProtector : IDataProtector
    {
        public string Protect(string value)
        {
            if(value == null)
            {
                return null;
            }
            var unprotectedBytes = Encoding.Unicode.GetBytes(value);
            var protectedValue = MachineKey.Protect(unprotectedBytes);
            return HttpServerUtility.UrlTokenEncode(protectedValue);
        }

        public string Unprotect(string value)
        {
            if (value == null)
            {
                return null;
            }
            var protectedBytes = HttpServerUtility.UrlTokenDecode(value);
            if (protectedBytes != null)
            {
                var unprotectedBytes = MachineKey.Unprotect(protectedBytes);
                if (unprotectedBytes != null)
                {
                    return Encoding.Unicode.GetString(unprotectedBytes);
                }
            }
            return null;
        }

        public string GenerateKey(int length)
        {
            if (length < 0)
            {
                return "";
            }
            var buff = new byte[length/2];
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