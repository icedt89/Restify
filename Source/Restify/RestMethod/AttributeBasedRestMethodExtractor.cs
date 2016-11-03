namespace JanHafner.Restify.RestMethod
{
    using System;
    using System.Net.Http;
    using Request;
    using Toolkit.Common.ExtensionMethods;

    /// <summary>
    /// Extracts the <see cref="RestMethod"/> from the <see cref="RestMethodAttribute"/>.
    /// </summary>
    public sealed class AttributeBasedRestMethodExtractor : IRestMethodExtractor
    {
        /// <summary>
        /// Extracts the <see cref="HttpMethod"/> from the <see cref="RequestBase"/>.
        /// </summary>
        /// <param name="request">The <see cref="RequestBase"/>.</param>
        /// <returns>The extracted <see cref="HttpMethod"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="request"/>' cannot be null. </exception>
        public HttpMethod Extract(RequestBase request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var restMethodAttribute = request.GetType().GetAttribute<RestMethodAttribute>();
            if (restMethodAttribute == null)
            {
                return HttpMethod.Get;
            }

            return restMethodAttribute.ToHttpMethod();
        }
    }
}