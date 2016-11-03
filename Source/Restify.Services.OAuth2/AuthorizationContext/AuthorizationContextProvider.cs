namespace JanHafner.Restify.Services.OAuth2.AuthorizationContext
{
    using System;
    using System.Collections.Generic;
    using Configuration;
    using JetBrains.Annotations;
    using Ninject;
    using Ninject.Modules;
    using Queryfy;
    using Toolkit.Common.ExtensionMethods;

    /// <summary>
    /// Provides instances of Google services.
    /// </summary>
    public class AuthorizationContextProvider : IDisposable
    {
        /// <summary>
        /// The name of the default context.
        /// </summary>
        [NotNull]
        public readonly String DefaultContextName = "Default";

        [NotNull]
        private readonly IDictionary<String, IKernel> authorizationContextKernels = new Dictionary<String, IKernel>();

        /// <summary>
        /// Initializes static members of the <see cref="AuthorizationContextProvider"/> class. 
        /// </summary>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="additionalModules"/>' cannot be null. </exception>
        public AuthorizationContextProvider([NotNull] params INinjectModule[] additionalModules)
        {
            if (additionalModules == null)
            {
                throw new ArgumentNullException(nameof(additionalModules));
            }

            foreach (var authorizationContextConfiguration in RestifyConfigurationSection.GetXmlConfiguration().AuthorizationContextConfigurations)
            {
                var authorizationContextKernel = new StandardKernel(new QueryfyDotNetRegistrations(), new ServiceInitialization(authorizationContextConfiguration.Name));
                authorizationContextKernel.Load(additionalModules);

                this.authorizationContextKernels.Add(authorizationContextConfiguration.Name, authorizationContextKernel);
            }
        }

        /// <summary>
        /// Creates a <see cref="IAuthorizationContext"/> based on the configuration with the supplied <see cref="Type.Name"/> of the Rest service.
        /// </summary>
        /// <typeparam name="TService">The <see cref="Type"/> of the Rest service.</typeparam>
        /// <returns>The <see cref="IAuthorizationContext"/>.</returns>
        [NotNull]
        public IAuthorizationContext CreateAuthorizationContext<TService>()
            where TService : IRestService
        {
            return this.CreateAuthorizationContext(typeof(TService).Name);
        }

        /// <summary>
        /// Creates a <see cref="IAuthorizationContext"/> based on the configuration with the supplied name.
        /// </summary>
        /// <param name="configurationName">The name of the configuration on which the <see cref="IAuthorizationContext"/> is based.</param>
        /// <returns>The <see cref="IAuthorizationContext"/> with the name.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="configurationName"/>' cannot be null. </exception>
        [NotNull]
        public IAuthorizationContext CreateAuthorizationContext([NotNull] String configurationName)
        {
            if (String.IsNullOrEmpty(configurationName))
            {
                throw new ArgumentNullException(nameof(configurationName));
            }

            return this.authorizationContextKernels[configurationName].Get<IAuthorizationContext>();
        }

        /// <summary>
        /// Creates a <see cref="IAuthorizationContext"/> based on the configuration named "Default".
        /// </summary>
        /// <returns>The default <see cref="IAuthorizationContext"/>.</returns>
        [NotNull]
        public IAuthorizationContext CreateAuthorizationContext()
        {
            return this.CreateAuthorizationContext(this.DefaultContextName);
        }

        /// <summary>
        /// Registers the specified binding.
        /// </summary>
        /// <typeparam name="TInterface">The interface.</typeparam>
        /// <typeparam name="TImplementation">The implementation.</typeparam>
        public void RegisterService<TInterface, TImplementation>()
            where TImplementation : TInterface
            where TInterface : IRestService
        {
            this.authorizationContextKernels.ForEach(kernel => kernel.Value.Rebind<TInterface>().To<TImplementation>());
        }

        /// <summary>
        /// Registers the specified binding.
        /// </summary>
        /// <typeparam name="TRestServiceBase">The base class..</typeparam>
        /// <typeparam name="TInterface">The interface.</typeparam>
        public void RegisterDynamicService<TRestServiceBase, TInterface>()
            where TInterface : IRestService
            where TRestServiceBase : RestService
        {
            var dynamicServiceType = DynamicRestServiceProxyBuilder.Proxy<TRestServiceBase, TInterface>();

            this.authorizationContextKernels.ForEach(kernel => kernel.Value.Rebind<TInterface>().To(dynamicServiceType));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.authorizationContextKernels.ForEach(ack => ack.Value.Dispose());
        }
    }
}