using System;
using System.Net;
using System.Threading.Tasks;
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
            var request = new RestRequest(resource);
            var response = _client.Execute(request);
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            throw new InvalidOperationException(string.Format("{0} {1} {2}", response.StatusCode, response.Content, response.ErrorMessage));
        }

        public Task<T> GetResponseAsync<T>(string resource)
        {
            var request = new RestRequest(resource);
            var tcs = new TaskCompletionSource<T>();
            _client.ExecuteAsync(request, response =>
            {
                if (response.ErrorException == null)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var result = JsonConvert.DeserializeObject<T>(response.Content);
                        tcs.SetResult(result);
                    }
                    else
                    {
                        tcs.SetException(new InvalidOperationException(string.Format("{0} {1} {2}", response.StatusCode, response.Content, response.ErrorMessage)));    
                    }
                }
                else
                {
                    tcs.SetException(response.ErrorException);
                }
            });
            return tcs.Task;
        }
    }
}