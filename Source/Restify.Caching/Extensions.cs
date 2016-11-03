namespace JanHafner.Restify.Caching
{
    using System;
    using System.Net.Http;
    using JetBrains.Annotations;
    using Request;
    using Response;

    /// <summary>
    /// Provides extensions for this assembly.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Extracts the ETag from the ETag header of the <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>The header value.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="response"/>' cannot be null. </exception>
        [CanBeNull]
        internal static String ExtractETag([NotNull] this HttpResponseMessage response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            return response.Headers.ETag.Tag;
        }

        /// <summary>
        /// Extracts the ETag from the If-None-Match header of the <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="response">The request.</param>
        /// <returns>The header value.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="response"/>' cannot be null. </exception>
        [CanBeNull]
        [Obsolete]
        internal static String ExtractETag([NotNull] this HttpRequestMessage response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            return response.Headers.IfNoneMatch.ToString();
        }

        /// <summary>
        /// Checks if the <see cref="ResponseBase"/> provides an etag.
        /// </summary>
        /// <param name="response">The <see cref="ResponseBase"/> to check for an etag.</param>
        /// <param name="etag">The e-tag.</param>
        /// <returns>A value indicating if the etag can be used.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="response"/>' cannot be null. </exception>
        private static Boolean DoesProvideEtagt<TRequest>([NotNull] this ResponseBase<TRequest> response, [CanBeNull] out String etag)
            where TRequest : RequestBase, IReRequestable
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            etag = null;
            // ReSharper disable once SuspiciousTypeConversion.Global Third-party developers can provide the implementation.
            var etagProvider = response as IReRequestable;
            if (etagProvider != null)
            {
                etag = etagProvider.ETag;
            }

            return etagProvider != null && !String.IsNullOrWhiteSpace(etagProvider.ETag);
        }

        /// <summary>
        /// Sets the ETag property of the original request to the ETag extracted from the response and returns the original request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="response">The response.</param>
        /// <returns>The original request upgraded by with the ETag from the response.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="response"/>' cannot be null. </exception>
        [NotNull]
        public static TRequest ReRequest<TRequest>([NotNull] this ResponseBase<TRequest> response)
            where TRequest : RequestBase, IReRequestable
        {
            String etag;
            if (response.DoesProvideEtagt(out etag))
            {
                response.OriginalRequest.ETag = etag;
                return response.OriginalRequest;
            }

            throw new ResponseDoesNotProvideEtagException(response);
        }
    }
}