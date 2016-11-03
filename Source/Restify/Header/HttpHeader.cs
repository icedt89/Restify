namespace JanHafner.Restify.Header
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines a http header.
    /// </summary>
    public class HttpHeader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHeader"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="name"/>' cannot be null. </exception>
        public HttpHeader([NotNull] String name, [CanBeNull] String value)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// The name of the http header.
        /// </summary>
        [NotNull]
        public String Name { get; private set; }

        /// <summary>
        /// The value of the http header.
        /// </summary>
        [CanBeNull]
        public String Value { get; private set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0}={1}", this.Name, this.Value);
        }
    }
}