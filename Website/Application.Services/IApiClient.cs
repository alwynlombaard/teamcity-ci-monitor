using System.Threading.Tasks;

namespace website.Application.Services
{

    public interface IApiClient
    {
        T GetResponse<T>(string resource);
        Task<T> GetResponseAsync<T>(string resource);
    }

}