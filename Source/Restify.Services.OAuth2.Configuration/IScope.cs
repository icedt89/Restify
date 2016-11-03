namespace JanHafner.Restify.Services.OAuth2.Configuration
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines information about 
    /// </summary>
    public interface IScope
    {
        /// <summary>
        /// Indicates if the scope should be granted.
        /// </summary>
        Boolean Grant { get; }

        /// <summary>
        /// The scope which should be granted.
        /// </summary>
        [NotNull]
        String Scope { get; }
    }
}