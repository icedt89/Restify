namespace JanHafner.Restify.Header
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Provides a fixed header value that should be send on a declarative level.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class FixedRequestHeaderAttribute : MapHeaderAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FixedRequestHeaderAttribute"/> class.
        /// </summary>
        /// <param name="headerName">The name of the header.</param>
        /// <param name="value">The value of the header.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="headerName" />' cannot be null. </exception>
        public FixedRequestHeaderAttribute([NotNull] String headerName, [CanBeNull] String value)
            : base(headerName)
        {
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedRequestHeaderAttribute"/> class.
        /// </summary>
        /// <param name="headerName">The name of the header.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="headerName" />' cannot be null. </exception>
        public FixedRequestHeaderAttribute([NotNull] String headerName)
            : this(headerName, null)
        {
        }

        /// <summary>
        /// The value of the header.
        /// </summary>
        [CanBeNull]
        public String Value { get; private set; }

        /// <summary>
        /// Creates a new <see cref="HttpHeader"/> from this attribute instance.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public HttpHeader ToHttpHeader()
        {
            // ReSharper disable once AssignNullToNotNullAttribute This attribute requires in its construction that the name of the header is set.
            return new HttpHeader(this.HeaderName, this.Value);
        }
    }
}