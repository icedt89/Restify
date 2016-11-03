namespace JanHafner.Restify.Services.OAuth2.AuthorizationContext
{
    using System;
    using System.Linq;
    using Configuration;
    using Handler;
    using Header;
    using JetBrains.Annotations;
    using Ninject;
    using Properties;
    using Request;
    using Storage;

    /// <summary>
    /// Default implementation of an <see cref="IAuthorizationContext"/>.
    /// </summary>
    public class AuthorizationContext : IAuthorizationContext
    {
        [NotNull]
        private readonly IAuthorizationStore authorizationStore;

        [NotNull]
        private readonly IAuthorizationHandler authorizationHandler;

        /// <summary>
        /// The <see cref="IAuthorizationContextConfiguration"/>.
        /// </summary>
        [NotNull]
        protected readonly IAuthorizationContextConfiguration AuthorizationContextConfiguration;

        /// <summary>
        /// The <see cref="IKernel"/>.
        /// </summary>
        [NotNull]
        protected readonly IKernel Kernel;

        /// <summary>
        /// The synchronization object.
        /// </summary>
        [NotNull]
        protected readonly Object ServiceCreationLock = new Object();

        /// <summary>
        /// The <see cref="AuthorizationContextState"/>.
        /// </summary>
        [NotNull]
        protected AuthorizationContextState AuthorizationContextState;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationContext"/> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <param name="authorizationStore">The <see cref="IAuthorizationStore"/>.</param>
        /// <param name="authorizationHandler">The <see cref="IAuthorizationHandler"/>.</param>
        /// <param name="authorizationContextConfiguration">The <see cref="IAuthorizationContextConfiguration"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="kernel"/>', '<paramref name="authorizationHandler"/>', '<paramref name="authorizationStore"/>' and '<paramref name="authorizationContextConfiguration"/>' cannot be null. </exception>
        public AuthorizationContext([NotNull] IKernel kernel, [NotNull] IAuthorizationStore authorizationStore, [NotNull] IAuthorizationHandler authorizationHandler, [NotNull] IAuthorizationContextConfiguration authorizationContextConfiguration)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException(nameof(kernel));
            }

            if (authorizationStore == null)
            {
                throw new ArgumentNullException(nameof(authorizationStore));
            }

            if (authorizationHandler == null)
            {
                throw new ArgumentNullException(nameof(authorizationHandler));
            }

            if (authorizationContextConfiguration == null)
            {
                throw new ArgumentNullException(nameof(authorizationContextConfiguration));
            }

            this.Kernel = kernel;
            this.authorizationStore = authorizationStore;
            this.authorizationHandler = authorizationHandler;
            this.AuthorizationContextConfiguration = authorizationContextConfiguration;
            this.AuthorizationContextState = new UnauthorizedAuthorizationContextState(authorizationStore, authorizationHandler);
        }

        /// <summary>
        /// Indicates if the 
        /// </summary>
        public Boolean IsAuthorized
        {
            get
            {
                return this.AuthorizationContextState is AuthorizedAuthorizationContextState;
            }
        }

        /// <summary>
        /// Starts the authorization process.
        /// </summary>
        public void StartAuthorization()
        {
            var authorizedAuthorizationState = this.AuthorizationContextState.StartAuthorization();
            this.AuthorizationContextState = new AuthorizedAuthorizationContextState(authorizedAuthorizationState, this.authorizationStore, this.authorizationHandler, this.AuthorizationContextConfiguration.Authorization);
        }

        /// <summary>
        /// Checks if the request is authorized and can be send.
        /// </summary>
        /// <param name="request">The authorizable request.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="request"/>' cannot be null. </exception>
        /// <exception cref="InvalidOperationException">The request needs to be authorized but the authorization context has not handled the authorization process.</exception>
        public virtual void EnsureAuthorization(RequestBase request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // ReSharper disable once SuspiciousTypeConversion.Global Third-party developers can provide the implementation.
            var iNeedAuthorization = request as INeedAuthorization;
            if (iNeedAuthorization != null)
            {
                if (!this.IsAuthorized)
                {
                    throw new InvalidOperationException(ExceptionMessages.RequestNeedsAuthorizationExceptionMessage);
                }

                this.ValidateScopes(request);
            }
        }

        /// <summary>
        /// Validates if all scopes for the request are authorized by this context.
        /// </summary>
        /// <param name="authorizableRequest"></param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="authorizableRequest"/>' cannot be null. </exception>
        /// <exception cref="InsufficientAuthorizationException">The authorization context needs grant on '%scopes%' for request '<paramref name="authorizableRequest"/>'.</exception>
        private void ValidateScopes([NotNull] RequestBase authorizableRequest)
        {
            if (authorizableRequest == null)
            {
                throw new ArgumentNullException(nameof(authorizableRequest));
            }

            var grantedScopes = Enumerable.Empty<String>();
            // ReSharper disable once SuspiciousTypeConversion.Global Third-party developers can provide the implementation.
            var neededScopesForRequest = ((INeedAuthorization)authorizableRequest).NeededScopes;
            var missingScopesForRequest = grantedScopes.Except(neededScopesForRequest).ToList();
            if (missingScopesForRequest.Any())
            {
                throw new InsufficientAuthorizationException(missingScopesForRequest, authorizableRequest);
            }
        }

        /// <summary>
        /// Revokes the whole authorization and clears all data associated with it.
        /// </summary>
        public void RevokeAuthorization()
        {
            this.AuthorizationContextState.RevokeAuthorization();
        }

        /// <summary>
        /// Clears the authorization information from this context.
        /// </summary>
        public void ClearAuthorization()
        {
            this.RevokeAuthorization();
            this.AuthorizationContextState = new UnauthorizedAuthorizationContextState(this.authorizationStore, this.authorizationHandler);
        }

        /// <summary>
        /// Creates the Bearer-authorization header for the current authorization.
        /// </summary>
        /// <returns>A new <see cref="HttpHeader"/> with the access token.</returns>
        public HttpHeader CreateBearer()
        {
            return this.AuthorizationContextState.CreateBearer();
        }

        /// <summary>
        /// Refreshes the authorization of this <see cref="IAuthorizationContext"/>.
        /// </summary>
        public void RefreshAuthorization()
        {
            this.AuthorizationContextState.RefreshAuthorization();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!(this.AuthorizationContextState is UnauthorizedAuthorizationContextState))
            {
                this.ClearAuthorization();
            }
        }

        /// <summary>
        /// Creates a new service that is bound to this <see cref="IAuthorizationContext"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to create.</typeparam>
        /// <returns>An instance of the service.</returns>
        public TService CreateAuthorizationBoundService<TService>()
            where TService : IRestService
        {
            lock (this.ServiceCreationLock)
            {
                // Because we used the same regisrations that was used to create this IAuthorizationContext instance,
                // we must temporary unbind all bindings for IAuthorizationContext and rebind it to the current (this) instance.
                // After service creation the binding will be set to the state it has before.
                // These actions are only affect the kernel of this isntance.
                this.Kernel.Rebind<IAuthorizationContext>().ToConstant(this);
                var service = this.Kernel.Get<TService>();

                this.Kernel.Rebind<IAuthorizationContext>().To(this.GetType());

                return service;
            }
        }
    }
}