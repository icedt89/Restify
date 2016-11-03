namespace JanHafner.Restify.Services.OAuth2
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    /// <summary>
    /// Marks a request for authorization and provides properties that define metadata what the request needs to get processed by the server.
    /// </summary>
    public interface INeedAuthorization
    {
        /// <summary>
        /// Defines the scopes that are needed to process the request.
        /// </summary>
        [NotNull]
        IEnumerable<String> NeededScopes { get; }
    }
}