using System.Collections.Generic;
using System.Web.Http;
using website.Models;

namespace website.Controllers
{
    [RoutePrefix("tests")]
    public class TestModelsController : ApiController
    {
        [Route("")]
        public IEnumerable<TestModel> GetAllTestModels()
        {
            var l = new List<TestModel>
            {

                new TestModel {Message = "hello 1"},
                new TestModel {Message = "hello 2"}

            };
            return l;
        }
    }
}
