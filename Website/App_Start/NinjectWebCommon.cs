using System.Web.Http;
using website.Application.Api.Controllers;
using website.Application.Infrastructure.DataProtection;
using website.Application.Infrastructure.Store;
using website.Application.Services.Configuration;
using website.Application.Services.Preferences;
using website.Application.Services.TeamCity;
using website.App_Start;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Website.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Website.App_Start.NinjectWebCommon), "Stop")]

namespace Website.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(kernel);
            kernel.Bind<IDataProtector>().To<DataProtector>();
            kernel.Bind<IPreferencesService>().To<PreferencesService>();
            kernel.Bind<ITeamCityApiClient>().To<TeamCityRestSharpApiClient>();
            kernel.Bind<ITeamCityConfigurationService>().To<TeamCityConfigurationService>();
            kernel.Bind<TeamCityConfig>()
                .ToMethod(c => kernel.Get<ITeamCityConfigurationService>().Load())
                .InRequestScope();
            kernel.Bind<IStore>()
                .ToMethod(x => new CookieStore(kernel.Get<HttpContextBase>(), kernel.Get<IDataProtector>()))
                .InRequestScope();

        }        
    }
}
