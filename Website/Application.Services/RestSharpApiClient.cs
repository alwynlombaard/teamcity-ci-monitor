using System.Net;
using Newtonsoft.Json;
using RestSharp;

namespace website.Application.Services
{
    public class RestSharpApiClient : IApiClient
    {
        private readonly RestClient _client;

        public RestSharpApiClient(RestClient client)
        {
            _client = client;
        }

        public T GetResponse<T>(string resource)
        {
            try
            {
                var request = new RestRequest(resource);
                var response = _client.Execute(request);
                return response.StatusCode == HttpStatusCode.OK
                           ? JsonConvert.DeserializeObject<T>(response.Content)
                           : default(T);
            }
            catch
            {
                return default(T);
            }
        }
    }
}