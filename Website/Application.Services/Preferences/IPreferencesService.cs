using System.Collections.Generic;

namespace website.Application.Services.Preferences
{
    public interface IPreferencesService
    {
        void HideProject(string id);
        void HideBuildType(string id);
        void ShowProject(string id);
        void ShowBuildType(string id);
        IEnumerable<string> GetHiddenProjects();
        IEnumerable<string> GetHiddenBuildTypes();
    }
}