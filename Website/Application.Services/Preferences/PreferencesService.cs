using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using website.Application.Infrastructure.Store;

namespace website.Application.Services.Preferences
{
    public class PreferencesService : IPreferencesService
    {
        private readonly IStore _store;

        public PreferencesService(IStore store)
        {
            _store = store;
        }
        
        private IEnumerable<string> GetStoreCollection(CookieStore.StoreCollections collection)
        {
            try
            {
                var value = _store.Get(collection.ToString());
                return string.IsNullOrEmpty(value)
                           ? Enumerable.Empty<string>()
                           : JsonConvert.DeserializeObject<IList<string>>(value);
            }
            catch
            {
                return Enumerable.Empty<string>();
            }
        }
        
        public void HideProject(string id)
        {
            var ids = GetStoreCollection(CookieStore.StoreCollections.Projects).ToList();
            if (ids.Contains(id))
            {
                return;
            }
            ids.Add(id);
            _store.Save(CookieStore.StoreCollections.Projects.ToString(), JsonConvert.SerializeObject(ids));
        }

        public void HideBuildType(string id)
        {
            var ids = GetStoreCollection(CookieStore.StoreCollections.BuildTypes).ToList();
            if (ids.Contains(id))
            {
                return;
            }
            ids.Add(id);
            _store.Save(CookieStore.StoreCollections.BuildTypes.ToString(), JsonConvert.SerializeObject(ids));
        }

        public void ShowProject(string id)
        {
            var ids = GetStoreCollection(CookieStore.StoreCollections.Projects).ToList();
            if (!ids.Contains(id))
            {
                return;
            }
            ids.Remove(id);
            _store.Save(CookieStore.StoreCollections.Projects.ToString(), JsonConvert.SerializeObject(ids));
        }

        public void ShowBuildType(string id)
        {
            var ids = GetStoreCollection(CookieStore.StoreCollections.BuildTypes).ToList();
            if (!ids.Contains(id))
            {
                return;
            }
            ids.Remove(id);
            _store.Save(CookieStore.StoreCollections.BuildTypes.ToString(), JsonConvert.SerializeObject(ids));
        }

        public IEnumerable<string> GetHiddenProjects()
        {
            return GetStoreCollection(CookieStore.StoreCollections.Projects);
        }

        public IEnumerable<string> GetHiddenBuildTypes()
        {
            return GetStoreCollection(CookieStore.StoreCollections.BuildTypes);
        }
    }
}