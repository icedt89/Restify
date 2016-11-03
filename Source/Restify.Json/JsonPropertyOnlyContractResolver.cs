namespace JanHafner.Restify.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Serializes only members which have the <see cref="JsonPropertyAttribute"/> attribute.
    /// </summary>
    public sealed class JsonPropertyOnlyContractResolver : DefaultContractResolver
    {
        [NotNull]
        private readonly IPropertyFilter propertyFilter = new JsonPropertyAttributePropertyFilter();

        /// <summary>
        /// Creates properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract"/>.
        /// </summary>
        /// <param name="type">The type to create properties for.</param>
        /// <param name="memberSerialization">The member serialization mode for the type.</param>
        /// <returns>
        /// Properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="type"/>' cannot be null. </exception>
        [NotNull]
        protected override IList<JsonProperty> CreateProperties([NotNull] Type type, MemberSerialization memberSerialization)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return base.CreateProperties(type, memberSerialization).Where(
                property =>
                    {
                        var propertyInfo = property.DeclaringType.GetProperty(property.UnderlyingName);
                        return this.propertyFilter.IsSatisfied(propertyInfo)
                               && !property.Ignored;
                    }).ToList();
        }
    }
}