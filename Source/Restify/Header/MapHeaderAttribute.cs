namespace JanHafner.Restify.Header
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Instructs the <see cref="MappedHeaderCollector"/> to map the annotated member to the <see cref="HttpHeader"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class MapHeaderAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapHeaderAttribute"/> class.
        /// </summary>
        /// <param name="headerName">The name of the header.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="headerName"/>' cannot be null. </exception>
        public MapHeaderAttribute([NotNull] String headerName)
        {
            if (String.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));    
            }

            this.HeaderName = headerName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapHeaderAttribute"/> class.
        /// </summary>
        public MapHeaderAttribute()
        {
        }

        /// <summary>
        /// Gets the name of the header.
        /// </summary>
        [CanBeNull]
        public String HeaderName { get; private set; }

        /// <summary>
        /// <c>True</c> if the <see cref="HeaderName"/> is empty and the 
        /// </summary>
        public Boolean UsePropertyNameAsHeader
        {
            get
            {
                return String.IsNullOrEmpty(this.HeaderName);
            }
        }
    }
}