namespace JanHafner.Restify.Services.OAuth2.AuthorizationContext
{
    using System;
    using Configuration;
    using Handler;
    using Header;
    using JetBrains.Annotations;
    using Ninject;
    using Ninject.Modules;
    using Queryfy.UrlPathBuilder;
    using Request;
    using Restify;
    using RestMethod;
    using Services.RequestExecutionStrategy;
    using Storage;

    /// <summary>
    /// Provides <see cref="IKernel"/> initialization for the Rest Services.
    /// </summary>
    public sealed class ServiceInitialization : NinjectModule
    {
        [NotNull]
        private readonly String authorizationContextName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceInitialization"/> class.
        /// </summary>
        /// <param name="authorizationContextName">The name of the authorization context configurationto pass through.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="authorizationContextName"/>' cannot be null. </exception>
        public ServiceInitialization([NotNull] String authorizationContextName)
        {
            if (String.IsNullOrEmpty(authorizationContextName))
            {
                throw new ArgumentNullException(nameof(authorizationContextName));
            }

            this.authorizationContextName = authorizationContextName;
        }

        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            var configuration = RestifyConfigurationSection.GetXmlConfiguration();

            this.Bind<IRestMethodExtractor>().To<AttributeBasedRestMethodExtractor>().InSingletonScope();
            this.Bind<IRequestHeaderFactory>().To<RequestHeaderFactory>().InSingletonScope();
            this.Bind<IUrlPathBuilder>().To<UrlPathBuilder>().InSingletonScope();
            
            this.Bind<IRequestHeaderCollector>().To<DefaultRequestHeaderCollector>().InSingletonScope();
            this.Bind<IRequestHeaderCollector>().To<FixedRequestHeaderCollector>().InSingletonScope();
            this.Bind<IRequestHeaderCollector>().To<MappedHeaderCollector>().InSingletonScope();
            this.Bind<IRequestHeaderCollector>().To<AuthorizingHeaderCollector>();
            
            this.Bind<IResponseHeaderMapper>().To<ResponseHeaderMapper>().InSingletonScope();
            this.Bind<IRestifyConfiguration>().ToConstant(configuration).InSingletonScope();
            this.Bind<IHttpRequestMessageFactory>().To<HttpRequestMessageFactory>().InSingletonScope();

            this.Bind<IAuthorizationStore>().To<FileSystemAuthorizationStore>();
            this.Bind<IRequestExecutionStrategy>().To<DefaultRequestExecutionStrategy>();

            this.Bind<IAuthorizationContext>().To<AuthorizationContext>();
            this.Bind<IRequestProcessor>().To<RequestProcessor>();
            this.Bind<IAuthorizationHandler>().To<NullAuthorizationHandler>();

            var authorizationContextConfiguration = configuration.GetAuthorizationContextConfiguration(this.authorizationContextName);
            this.Bind<IAuthorizationContextConfiguration>().ToConstant(authorizationContextConfiguration).InSingletonScope();
            this.Bind<IAuthorizationConfiguration>().ToConstant(authorizationContextConfiguration.Authorization).InSingletonScope();

            //this.Bind<ILog>().ToMethod(m => LogManager.GetLogger("Default")).InSingletonScope();
        }
    }
}