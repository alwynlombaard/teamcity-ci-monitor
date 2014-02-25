namespace website.Application.Infrastructure.Store
{
    public interface IStore
    {
        void Save(string key, string value);
        void Remove(string key);
        string Get(string key);
    }
}