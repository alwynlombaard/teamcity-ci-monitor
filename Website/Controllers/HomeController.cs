using System.Web.Mvc;
using Raven.Client;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDocumentSession _session;

        public HomeController(IDocumentSession session)
        {
            _session = session;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StyleGuide()
        {
            return View();
        }
    }
}