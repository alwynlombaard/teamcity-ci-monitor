using System.Text;
using System.Web.Mvc;
using Raven.Client;

namespace Website.Controllers
{
    public class TestController : Controller
    {
        private readonly IDocumentSession _session;

        public TestController(IDocumentSession session)
        {
            _session = session;
        }

        public ActionResult Hello()
        {
            return Content("Hello");
        }

        public ActionResult Add(string id)
        {
            _session.Store(new Test{Id = id, Value = id});
            return Content("done");
        }

        public ActionResult Index()
        {

            var os = _session.Query<Test>();
            var sb = new StringBuilder();
            foreach (var test in os)
            {
                sb.Append (test == null ? "null" : test.Id);
                sb.Append(":");

            }
            
           
            return Content(sb.ToString());
        }

    }

    public class Test
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }
}
