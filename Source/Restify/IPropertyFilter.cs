namespace JanHafner.Restify
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines methods for filtering properties during serialization and deserialization.
    /// </summary>
    public interface IPropertyFilter
    {
        /// <summary>
        /// Implements a filter function which filters the elements in the source list by a predicate.
        /// </summary>
        /// <param name="source">The source list.</param>
        /// <returns>The filtered list.</returns>
        [LinqTunnel]
        [NotNull]
        IEnumerable<PropertyInfo> Filter([NotNull] IEnumerable<PropertyInfo> source);

        /// <summary>
        /// Determines if the supplied <see cref="PropertyInfo"/> satisfies the condition.
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/>.</param>
        /// <returns>A vlaue indicating whether the <see cref="PropertyInfo"/> satisfies the condition.</returns>
        [LinqTunnel]
        Boolean IsSatisfied([NotNull] PropertyInfo propertyInfo);
    }
}