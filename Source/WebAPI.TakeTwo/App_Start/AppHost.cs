using DDDIntro.WebAPI.TakeTwo.App_Start;
using DDDIntro.WebAPI.TakeTwo.Infrastructure;
using DDDIntro.WebAPI.TakeTwo.Infrastructure.GlobalFilters;
using DDDIntro.WebAPI.TakeTwo.Resources;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.WebHost.Endpoints;

[assembly: WebActivator.PreApplicationStartMethod(typeof(AppHost), "Start")]

//IMPORTANT: Add the line below to MvcApplication.RegisterRoutes(RouteCollection) in the Global.asax:
//routes.IgnoreRoute("api/{*pathInfo}"); 

/**
 * Entire ServiceStack Starter Template configured with a 'Hello' Web Service and a 'Todo' Rest Service.
 *
 * Auto-Generated Metadata API page at: /metadata
 * See other complete web service examples at: https://github.com/ServiceStack/ServiceStack.Examples
 */

namespace DDDIntro.WebAPI.TakeTwo.App_Start
{
	//A customizeable typed UserSession that can be extended with your own properties
	//To access ServiceStack's Session, Cache, etc from MVC Controllers inherit from ControllerBase<CustomUserSession>
	public class CustomUserSession : AuthUserSession
	{
		public string CustomProperty { get; set; }
	}

	public class AppHost
		: AppHostBase
	{		
		public AppHost() //Tell ServiceStack the name and where to find your web services
			: base("StarterTemplate ASP.NET Host", typeof(HelloService).Assembly) { }

		public override void Configure(Funq.Container container)
		{
			//Set JSON web services to return idiomatic JSON camelCase properties
			ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

			//Configure User Defined REST Paths
			Routes
				.Add<Country>("/countries")
                .Add<Country>("/countries/{Id*}");

		    container.Adapter = WindsorContainerAdapter.Create();

		    var windsorLifestyleScopeFilter = new WindsorLifestyleScopeFilter(container);
            var sessionPerRequestFilter = new SessionPerRequestFilter(container);
            windsorLifestyleScopeFilter.ConfigureRequestFilters(RequestFilters);
            sessionPerRequestFilter.ConfigureRequestFilters(RequestFilters);
            sessionPerRequestFilter.ConfigureResponseFilters(ResponseFilters);
            windsorLifestyleScopeFilter.ConfigureResponseFilters(ResponseFilters);

		    //Change the default ServiceStack configuration
		    //SetConfig(new EndpointHostConfig {
		    //    DebugMode = true, //Show StackTraces in responses in development
		    //});

		    //Enable Authentication
		    //ConfigureAuth(container);

		    //Register all your dependencies
		    //container.Register(new TodoRepository());

		    ////Register In-Memory Cache provider. 
		    ////For Distributed Cache Providers Use: PooledRedisClientManager, BasicRedisClientManager or see: https://github.com/ServiceStack/ServiceStack/wiki/Caching
		    //container.Register<ICacheClient>(new MemoryCacheClient());
		    //container.Register<ISessionFactory>(c => 
		    //    new SessionFactory(c.Resolve<ICacheClient>()));
		}

		/* Uncomment to enable ServiceStack Authentication and CustomUserSession
		private void ConfigureAuth(Funq.Container container)
		{
			var appSettings = new AppSettings();

			//Default route: /auth/{provider}
			Plugins.Add(new AuthFeature(this, () => new CustomUserSession(),
				new IAuthProvider[] {
					new CredentialsAuthProvider(appSettings), 
					new FacebookAuthProvider(appSettings), 
					new TwitterAuthProvider(appSettings), 
					new BasicAuthProvider(appSettings), 
				})); 

			//Default route: /register
			Plugins.Add(new RegistrationFeature()); 

			//Requires ConnectionString configured in Web.Config
			var connectionString = ConfigurationManager.ConnectionStrings["AppDb"].ConnectionString;
			container.Register<IDbConnectionFactory>(c =>
				new OrmLiteConnectionFactory(connectionString, SqlServerOrmLiteDialectProvider.Instance));

			container.Register<IUserAuthRepository>(c =>
				new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()));

			var authRepo = (OrmLiteAuthRepository)container.Resolve<IUserAuthRepository>();
			authRepo.CreateMissingTables();
		}
		*/

		public static void Start()
		{
			new AppHost().Init();
		}
	}
}