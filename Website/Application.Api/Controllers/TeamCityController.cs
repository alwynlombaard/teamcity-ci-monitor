using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using website.Application.Services.Preferences;
using website.Application.Services.TeamCity;
using website.Application.Services.TeamCity.Dto;

namespace website.Application.Api.Controllers
{

    public class GetLastBuildsRequest
    {
        public string[] BuildTypeIds { get; set; }
    }

    [RoutePrefix("tc")]
    public class TeamCityController : ApiController
    {
        private readonly ITeamCityApiClient _apiClient;
        private readonly IPreferencesService _preferencesService;

        public TeamCityController(ITeamCityApiClient apiClient, IPreferencesService preferencesService)
        {
            _apiClient = apiClient;
            _preferencesService = preferencesService;
        }

        [Route("model")]
        public IHttpActionResult GetModel()
        {
            try
            {
                var model = LoadModel();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("runningbuilds")]
        public IHttpActionResult GetRunningBuilds()
        {
            try
            {
                var buildsDto = _apiClient.GetAllRunningBuilds();
                var model = new RunningBuidlsModel();

                if (buildsDto == null)
                {
                    return Ok(model);
                }

                foreach (var build in buildsDto.Build.OrderBy(b => b.Id))
                {
                    model.Builds.Add(new BuildModel
                    {
                        Id = build.Id,
                        Status = build.Status,
                        StatusText = build.StatusText,
                        Number = build.Number,
                        BuildTypeId = build.BuildTypeId,
                        FinishDate = build.FinishDate,
                        RunningInfo = new RunningInfoModel {PercentageComplete = build.PercentageComplete},
                        WebUrl = build.WebUrl
                    });
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("build/{id}")]
        public IHttpActionResult GetBuild(string id)
        {
            try
            {
                var build = _apiClient.GetBuild(id);
                var model = new BuildModel();
                if (build == null)
                {
                    return Ok(model);
                }
                model.Id = build.Id;
                model.Number = build.Number;
                model.BuildTypeId = build.BuildType.Id;

                if (build.RunningInfo != null)
                {
                    model.RunningInfo = new RunningInfoModel
                    {
                        PercentageComplete = build.RunningInfo.PercentageComplete,
                        CurrentStageText = build.RunningInfo.CurrentStageText
                    };
                }

                model.Status = build.Status;
                model.StatusText = build.StatusText;
                model.WebUrl = build.WebUrl;
                model.StartDate = build.StartDate;
                model.FinishDate = build.FinishDate;

                return Ok(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("buildtype/builds")]
        public async Task<IHttpActionResult> PostLastBuilds(GetLastBuildsRequest request)
        {
            
            var result = new List<BuildModel>();

            if (request == null || request.BuildTypeIds == null)
            {
                return Ok(result);
            }

            foreach (var buildTypeId in request.BuildTypeIds)
            {
                var model = new BuildModel();
                try
                {
                    var buildDto = await _apiClient.GetLastBuildsForBuildTypeAsync(buildTypeId);
                    if (buildDto == null)
                    {
                        continue;
                    }
                    var build = buildDto.Build.FirstOrDefault();

                    if (build == null)
                    {
                        continue;
                    }

                    model.Id = build.Id;
                    model.Number = build.Number;
                    model.BuildTypeId = build.BuildTypeId;
                    model.Status = build.Status;
                    model.StatusText = build.StatusText;
                    model.WebUrl = build.WebUrl;
                    model.FinishDate = build.FinishDate;
                    model.StartDate = build.StartDate;

                    result.Add(model);
                }
                catch
                {
                    result.Add(new BuildModel {BuildTypeId = buildTypeId, Status = "Failed to update"});
                }
            }
            return Ok(result);
        }

        [Route("buildType/{buildTypeId}/lastbuild")]
        public IHttpActionResult GetLastBuild(string buildTypeId)
        {
            try
            {
                var buildDto = _apiClient.GetLastBuildsForBuildType(buildTypeId);
                var model = new BuildModel();
                if (buildDto == null)
                {
                    return Ok(model);
                }
                var build = buildDto.Build.FirstOrDefault();

                if (build == null) return Ok(model);

                model.Id = build.Id;
                model.Number = build.Number;
                model.BuildTypeId = build.BuildTypeId;
                model.Status = build.Status;
                model.StatusText = build.StatusText;
                model.WebUrl = build.WebUrl;
                model.FinishDate = build.FinishDate;
                model.StartDate = build.StartDate;

                return Ok(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("project/{Id}/hide")]
        public IHttpActionResult PostHideProject(string id)
        {
            _preferencesService.HideProject(id);
            return Ok();
        }

        [Route("project/{Id}/show")]
        public IHttpActionResult PostShowProject(string id)
        {
            _preferencesService.ShowProject(id);
            return Ok();
        }

        [Route("buildtype/{Id}/hide")]
        public IHttpActionResult PostHideBuildType(string id)
        {
            _preferencesService.HideBuildType(id);
            return Ok();
        }

        [Route("buildtype/{Id}/show")]
        public IHttpActionResult PostShowBuildType(string id)
        {
            _preferencesService.ShowBuildType(id);
            return Ok();
        }

        private Model LoadModel()
        {
            var projectDto = _apiClient.GetAllProjects();
            var buildTypesDto = _apiClient.GetAllBuildTypes();
            var hiddenProjects = _preferencesService.GetHiddenProjects();
            var hiddenBuildTypes = _preferencesService.GetHiddenBuildTypes();


            var model = new Model();
            if (projectDto == null || buildTypesDto == null)
            {
                return model;
            }

            foreach (var project in projectDto.Project.Where(p => hiddenProjects.All(h => h != p.Id)))
            {
                var buildTypesForProject = buildTypesDto.BuildType
                    .Where(b => b.ProjectId == project.Id && hiddenBuildTypes.All(h => h != b.Id))
                    .ToList();

                var buildTypeRows = buildTypesForProject
                    .Select((p, index) => new {p, index})
                    .GroupBy(a => a.index/4)
                    .Select(grp => grp.Select(g => g.p).ToList())
                    .ToList();

                var projectModel = new ProjectModel {Name = project.Name, Id = project.Id};

                foreach (var buildTypeRow in buildTypeRows)
                {
                    var buildType1 = buildTypeRow.FirstOrDefault();
                    var buildType2 = buildTypeRow.Skip(1).FirstOrDefault();
                    var buildType3 = buildTypeRow.Skip(2).FirstOrDefault();
                    var buildType4 = buildTypeRow.Skip(3).FirstOrDefault();

                    var row = new BuildTypeRow();
                    if (buildType1 != null)
                    {
                        row.BuildTypes.Add(new BuildTypeModel {Name = buildType1.Name, BuildTypeId = buildType1.Id});
                    }
                    if (buildType2 != null)
                    {
                        row.BuildTypes.Add(new BuildTypeModel { Name = buildType2.Name, BuildTypeId = buildType2.Id });
                    }
                    if (buildType3 != null)
                    {
                        row.BuildTypes.Add(new BuildTypeModel { Name = buildType3.Name, BuildTypeId = buildType3.Id });
                    }
                    if (buildType4 != null)
                    {
                        row.BuildTypes.Add(new BuildTypeModel { Name = buildType4.Name, BuildTypeId = buildType4.Id });
                    }

                    projectModel.BuildTypeRows.Add(row);
                }

                model.Projects.Add(projectModel);
            }
            return model;
        }
    }

    public class RunningBuidlsModel
    {
        public RunningBuidlsModel()
        {
            Builds = new List<BuildModel>();
        }
        public IList<BuildModel> Builds { get; set; }
    }

    public class Model
    {
        public Model()
        {
            Projects = new List<ProjectModel>();
        }
        public  IList<ProjectModel> Projects;
    }
    
    public class ProjectModel
    {
        public ProjectModel()
        {
            BuildTypeRows = new List<BuildTypeRow>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public IList<BuildTypeRow> BuildTypeRows { get; set; }
    }

    public class BuildTypeModel
    {
        public BuildTypeModel()
        {
            LastBuild = new BuildModel();
        }
        public string Name { get; set; }
        public string BuildTypeId { get; set; }
        public BuildModel LastBuild { get; set; }
        public string NextUpdate { get; set; }
    }

    public class BuildModel
    {
        public BuildModel()
        {
            Status = "UNKNOWN";
        }
        public int Id { get; set; }
        public string Status { get; set; }
        public string StatusText { get; set; }
        public string Number { get; set; }
        public string BuildTypeId { get; set; }
        public string FinishDate { get; set; }
        public string StartDate { get; set; }
        public string WebUrl { get; set; }
        public RunningInfoModel RunningInfo { get; set; }
    }

    public class RunningInfoModel
    {
        public int PercentageComplete { get; set; }
        public string CurrentStageText { get; set; }
    }

    public class BuildTypeRow
    {
        public BuildTypeRow()
        {
            BuildTypes = new List<BuildTypeModel>();
        }
        public IList<BuildTypeModel> BuildTypes { get; set; }
    }

}