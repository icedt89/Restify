namespace JanHafner.Restify.Services.OAuth2.Configuration
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines GooKit configuration specific information.
    /// </summary>
    public interface IRestifyConfiguration
    {
        /// <summary>
        /// Gets the <see cref="IAuthorizationContextConfiguration"/> with the supplied name.
        /// </summary>
        [NotNull]
        IAuthorizationContextConfiguration GetAuthorizationContextConfiguration([NotNull] String name);

        /// <summary>
        /// Gets a list of all registered <see cref="IAuthorizationContextConfiguration"/>s.
        /// </summary>
        [NotNull]
        IEnumerable<IAuthorizationContextConfiguration> AuthorizationContextConfigurations { get; }
    }
}