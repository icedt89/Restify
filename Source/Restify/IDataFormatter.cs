namespace JanHafner.Restify
{
    using System;
    using JetBrains.Annotations;
    using Response;

    /// <summary>
    /// Provides methods for deserializing the response.
    /// </summary>
    public interface IDataFormatter
    {
        /// <summary>
        /// Deserializes the specified response content.
        /// </summary>
        /// <param name="responseContent">Content of the response.</param>
        /// <returns>An implementation of the <see cref="ResponseBase"/> class.</returns>
        [NotNull]
        ResponseBase Deserialize([NotNull] String responseContent);
    }
}