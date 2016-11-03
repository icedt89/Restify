namespace JanHafner.Restify.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;
    using Newtonsoft.Json;

    /// <summary>
    /// Filters the sequence by the appearance of the <see cref="JsonPropertyAttribute"/>.
    /// </summary>
    public sealed class JsonPropertyAttributePropertyFilter : IPropertyFilter
    {
        /// <summary>
        /// Implements a filter function which filters the elements in the source list by the appearance of the <see cref="JsonPropertyAttribute"/>.
        /// </summary>
        /// <param name="source">The source list.</param>
        /// <returns>The filtered list.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>' cannot be null. </exception>
        [LinqTunnel]
        public IEnumerable<PropertyInfo> Filter(IEnumerable<PropertyInfo> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(this.IsSatisfied);
        }

        /// <summary>
        /// Determines if the supplied <see cref="PropertyInfo"/> satisfies the condition.
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/>.</param>
        /// <returns>A vlaue indicating whether the <see cref="PropertyInfo"/> satisfies the condition.</returns>
        [LinqTunnel]
        public bool IsSatisfied(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(typeof (JsonPropertyAttribute), true).Length > 0;
        }
    }
}