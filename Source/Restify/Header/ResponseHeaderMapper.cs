namespace JanHafner.Restify.Header
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Queryfy.Inspection;
    using Response;
    using Toolkit.Common.ExtensionMethods;

    /// <summary>
    /// Provides the implementation of thE <see cref="IResponseHeaderMapper"/>.
    /// </summary>
    public sealed class ResponseHeaderMapper : IResponseHeaderMapper
    {
        /// <summary>
        /// Maps all <see cref="Header"/> instances of the supplied <see cref="HttpHeaderCollection"/> to the supplied <see cref="ResponseBase"/>.
        /// </summary>
        /// <param name="responseBase">The response.</param>
        /// <param name="httpHeaderCollection">The headers-</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="responseBase"/>' and '<paramref name="httpHeaderCollection"/>' cannot be null. </exception>
        /// <exception cref="MemberIsNotWritableException">The member is not writable.</exception>
        public void MapHeaders(ResponseBase responseBase, HttpHeaderCollection httpHeaderCollection)
        {
            if (responseBase == null)
            {
                throw new ArgumentNullException(nameof(responseBase));
            }
            
            if (httpHeaderCollection == null)
            {
                throw new ArgumentNullException(nameof(httpHeaderCollection));
            }

            var members = responseBase.GetType().GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public).Where(property => property.HasAttribute<MapHeaderAttribute>());
            foreach (var memberInfo in members)
            {
                if (memberInfo.IsReadonly())
                {
                    throw new MemberIsNotWritableException(memberInfo);
                }

                var mapHeaderAttribute = memberInfo.GetAttribute<MapHeaderAttribute>();
                var mappedHeader = httpHeaderCollection.SingleOrDefault(h => h.Name.Equals(mapHeaderAttribute.HeaderName, StringComparison.InvariantCultureIgnoreCase));
                if (mappedHeader == null || String.IsNullOrEmpty(mappedHeader.Value))
                {
                    continue;
                }

                memberInfo.SetValue(responseBase, mappedHeader.Value);
            }
        }
    }
}