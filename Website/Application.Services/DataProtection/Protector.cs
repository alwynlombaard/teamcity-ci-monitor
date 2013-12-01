using System.Security.Cryptography;
using System.Text;

namespace website.Application.Services.DataProtection
{
    public class Protector : IProtector
    {
        public string Protect(string value)
        {
            return null;
        }

        public string Unprotect(string value)
        {
            return null;
        }

        public string GenerateKey(int length)
        {
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