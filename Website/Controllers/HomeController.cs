using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using website.Application.Services.Preferences;
using website.Application.Services.TeamCity;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPreferencesService _preferencesService;
        private readonly ITeamCityApiClient _teamCityApi;

        public HomeController(IPreferencesService preferencesService, ITeamCityApiClient teamCityApi)
        {
            _preferencesService = preferencesService;
            _teamCityApi = teamCityApi;
        }

        [RequireHttps]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Preferences()
        {
            var model = new PreferencesModel();

            var projectsDto = _teamCityApi.GetAllProjects();
            var buildTypesDto = _teamCityApi.GetAllBuildTypes();

            if (projectsDto == null || buildTypesDto == null)
            {
                return View(model);
            }

            foreach (var project in projectsDto.Project)
            {
                model.Projects.Add(new ProjectModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    BuildTypes = buildTypesDto.BuildType
                        .Where(b => b.ProjectId == project.Id)
                        .Select(p => new BuildTypeModel {Id = p.Id, Name = p.Name})
                        .ToList()
                });
            }
            
            model.HiddenProjects = _preferencesService.GetHiddenProjects().ToList();
            model.HiddenBuildTypes = _preferencesService.GetHiddenBuildTypes().ToList();
            return View(model);
        }
        
        public ActionResult Test()
        {
            return View("Index");
        }

        public ActionResult StyleGuide()
        {
            return View();
        }

       
    }

    public class PreferencesModel
    {
        public PreferencesModel()
        {
            HiddenProjects = new List<string>();
            HiddenBuildTypes = new List<string>();
            Projects = new List<ProjectModel>();
        }
        public List<string> HiddenProjects { get; set; }
        public List<string> HiddenBuildTypes { get; set; }
        public List<ProjectModel> Projects { get; set; }
    }

    public class ProjectModel
    {
        public ProjectModel()
        {
            BuildTypes = new List<BuildTypeModel>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public List<BuildTypeModel> BuildTypes { get; set; }
    }

    public class BuildTypeModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}