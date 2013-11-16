using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Database.Server;

namespace Website.App_Start
{
    public class RavenDbNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDocumentStore>()
                .ToMethod(context =>
                {

                    NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8080);
                    return new EmbeddableDocumentStore
                    {
                        DataDirectory = "App_Data",
                        Configuration = {Port = 8080},
                        UseEmbeddedHttpServer = false

                    }.Initialize();
                }).InSingletonScope();


            Bind<IDocumentSession>().ToMethod(context => context.Kernel.Get<IDocumentStore>().OpenSession())
               .InRequestScope()
               .OnDeactivation(x =>
               {
                   if (x == null)
                       return;

                   x.SaveChanges();
                   x.Dispose();
               });
        
        }
    }
}