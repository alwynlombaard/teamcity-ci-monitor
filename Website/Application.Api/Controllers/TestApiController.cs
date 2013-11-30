using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using website.Application.Api.Dto;

namespace website.Application.Api.Controllers
{
    [RoutePrefix("tests")]
    public class TestModelsController : ApiController
    {
        private readonly List<TestModel> _allTestModels;

        public TestModelsController()
        {
            _allTestModels = new List<TestModel>
            {
                new TestModel {Id = 1, Message = "hello 1"},
                new TestModel {Id = 2,Message = "hello 2"}
            };
        }

        [Route("")]
        public IEnumerable<TestModel> GetAllTestModels()
        {
            
            return _allTestModels;
        }

        [Route("{id:int}")]
        public TestModel GetById(int id)
        {
            return  _allTestModels
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
