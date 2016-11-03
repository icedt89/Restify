namespace JanHafner.Restify.Services.OAuth2
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using AuthorizationContext;
    using JetBrains.Annotations;
    using Properties;
    using Request;

    /// <summary>
    /// Should thrown if the <see cref="RequestBase"/> needs authorization (implements <see cref="INeedAuthorization"/>) and needs scopes on which the <see cref="IAuthorizationContext"/> is not authorized.
    /// </summary>
    [Serializable]
    public sealed class InsufficientAuthorizationException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsufficientAuthorizationException"/> class.
        /// </summary>
        /// <param name="missingScopes">
        /// The missing scopes.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        public InsufficientAuthorizationException([NotNull] IEnumerable<String> missingScopes, [NotNull] RequestBase request)
            : base(String.Format(ExceptionMessages.InsufficientAuthorizationExceptionMessage, String.Join(", ", missingScopes), request.GetType().Name))
        {
            this.MissingScopes = missingScopes;
            this.Request = request;
        }

        /// <inheritdoc />
        // ReSharper disable once NotNullMemberIsNotInitialized
        protected InsufficientAuthorizationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// The scopes that are missing for the request.
        /// </summary>
        [NotNull]
        public IEnumerable<String> MissingScopes { get; private set; }

        /// <summary>
        /// The request that lacks on authorization.
        /// </summary>
        [NotNull]
        public RequestBase Request { get; private set; }
    }
}