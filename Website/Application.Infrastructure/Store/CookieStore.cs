using System;
using System.Linq;
using System.Web;
using website.Application.Infrastructure.DataProtection;

namespace website.Application.Infrastructure.Store
{
    public class CookieStore : IStore
    {
        private readonly IDataProtector _dataProtector;

        public enum StoreCollections
        {
            Unkown,
            Projects,
            BuildTypes
        }

        private readonly HttpRequestBase _request;
        private readonly HttpResponseBase _response;

        public CookieStore(HttpContextBase request, IDataProtector dataProtector)
        {
            _dataProtector = dataProtector;
            _request = request.Request;
            _response = request.Response;
        }

        public void Save(string key, string value)
        {
            if (key == null || value == null)
            {
                throw new ArgumentNullException();
            }
            value = _dataProtector.Protect(value);
            _response.SetCookie(new HttpCookie(key, value){ Expires = DateTime.MaxValue});
        }

        public void Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }
            if (_response.Cookies.AllKeys.Contains(key))
            {
                _response.Cookies.Remove(key);
            }
        }

        public string Get(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }

            var httpCookie = _request.Cookies.Get(key);
            return httpCookie != null ? _dataProtector.Unprotect(httpCookie.Value) : null;
        }
    }
}