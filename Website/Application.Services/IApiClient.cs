namespace website.Application.Services
{

    public interface IApiClient
    {
        T GetResponse<T>(string resource);
    }

}