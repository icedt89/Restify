namespace JanHafner.Restify.Caching
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Signals that the implementor provides an e-tag.
    /// </summary>
    public interface IReRequestable
    {
        /// <summary>
        /// The E-Tag of the resource.
        /// </summary>
        [CanBeNull]
        String ETag { get; set; }
    }
}