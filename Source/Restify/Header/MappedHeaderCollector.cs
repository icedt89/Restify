namespace JanHafner.Restify.Header
{
    using System;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;
    using Request;
    using Toolkit.Common.ExtensionMethods;

    /// <summary>
    /// Fills the supplied <see cref="HttpHeaderCollection"/> with values from properties annoated with the <see cref="MapHeaderAttribute"/>.
    /// </summary>
    public sealed class MappedHeaderCollector : IRequestHeaderCollector
    {
        /// <summary>
        /// Inspects the <see cref="RequestBase"/> and fill the <see cref="HttpHeaderCollection"/> with applicable headers.
        /// </summary>
        /// <param name="request">The <see cref="RequestBase"/>.</param>
        /// <param name="collectedHttpHeaders">The <see cref="HttpHeaderCollection"/> to fill with headers.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="request"/>' and '<paramref name="collectedHttpHeaders"/>' cannot be null. </exception>
        public void CollectRequestHeaders(RequestBase request, HttpHeaderCollection collectedHttpHeaders)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (collectedHttpHeaders == null)
            {
                throw new ArgumentNullException(nameof(collectedHttpHeaders));
            }

            var members = request.GetType().GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public).Where(property => property.HasAttributeExactly<MapHeaderAttribute>());
            foreach (var memberInfo in members)
            {
                var mapHeaderAttribute = memberInfo.GetAttribute<MapHeaderAttribute>();
                var memberValue = memberInfo.GetValue(request);
                if (memberValue == null)
                {
                    continue;
                }

                // ReSharper disable once AssignNullToNotNullAttribute Attribute is defined cause of filter in LINQ expression.
                var headerName = GetHeaderName(memberInfo, mapHeaderAttribute);
                var httpHeader = new HttpHeader(headerName, (string)memberValue);
                collectedHttpHeaders.Add(httpHeader);
            }
        }

        /// <summary>
        /// Returns the name of the header, in case the header name is empty and UsePropertyNameAsHeader is <c>>true</c>, the name of the <see cref="MemberInfo"/> is taken.
        /// </summary>
        /// <param name="memberInfo">The <see cref="MemberInfo"/>.</param>
        /// <param name="mapHeaderAttribute">The <see cref="MapHeaderAttribute"/>.</param>
        /// <returns>The name of the header.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="memberInfo"/>' and '<paramref name="mapHeaderAttribute"/>' cannot be null. </exception>
        [NotNull]
        private static String GetHeaderName([NotNull] MemberInfo memberInfo, [NotNull] MapHeaderAttribute mapHeaderAttribute)
        {
            if (memberInfo == null)
            {
                throw new ArgumentNullException(nameof(memberInfo));
            }

            if (mapHeaderAttribute == null)
            {
                throw new ArgumentNullException(nameof(mapHeaderAttribute));
            }

            var result = mapHeaderAttribute.HeaderName;
            if (!mapHeaderAttribute.UsePropertyNameAsHeader)
            {
                result = memberInfo.Name;
            }

            return result;
        }
    }
}