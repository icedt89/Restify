namespace JanHafner.Restify.Header
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using JetBrains.Annotations;

    /// <summary>
    /// Represents a collection of <see cref="HttpHeader"/>.
    /// </summary>
    public sealed class HttpHeaderCollection : ICollection<HttpHeader>
    {
        [NotNull]
        private readonly IDictionary<String, HttpHeader> internalCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHeaderCollection"/> class.
        /// </summary>
        public HttpHeaderCollection()
        {
            this.internalCollection = new SortedDictionary<String, HttpHeader>();
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public Int32 Count
        {
            get { return this.internalCollection.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public Boolean IsReadOnly
        {
            get { return this.internalCollection.IsReadOnly; }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        [LinqTunnel]
        public IEnumerator<HttpHeader> GetEnumerator()
        {
            return this.internalCollection.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        [LinqTunnel]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// If the name of the item already exist
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="item"/>' cannot be null. </exception>
        public void Add([NotNull] HttpHeader item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (this.internalCollection.ContainsKey(item.Name))
            {
                this.internalCollection.Remove(item.Name);
            }

            this.internalCollection.Add(item.Name, item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        public void Clear()
        {
            this.internalCollection.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="item"/>' cannot be null. </exception>
        public bool Contains([NotNull] HttpHeader item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return this.internalCollection.ContainsKey(item.Name);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type <paramref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
        /// <exception cref="NotSupportedException">Not supported. </exception>
        public void CopyTo(HttpHeader[] array, Int32 arrayIndex)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="item"/>' cannot be null. </exception>
        public Boolean Remove([NotNull] HttpHeader item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return this.internalCollection.Remove(item.Name);
        }

        /// <summary>
        /// Creates a new <see cref="HttpHeaderCollection"/> from the supplied <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="httpResponseMessage">The <see cref="HttpResponseMessage"/>.</param>
        /// <returns>The newly created <see cref="HttpHeaderCollection"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="httpResponseMessage"/>' cannot be null. </exception>
        [NotNull]
        public static HttpHeaderCollection FromHttpResponseMessage([NotNull] HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage == null)
            {
                throw new ArgumentNullException(nameof(httpResponseMessage));
            }

            var result = new HttpHeaderCollection();
            foreach (var httpResponseHeader in httpResponseMessage.Headers)
            {
                var httpHeader = new HttpHeader(httpResponseHeader.Key, String.Join(" ", httpResponseHeader.Value));
                result.Add(httpHeader);
            }

            return result;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(String.Format("{0} headers in collection", this.Count));
            foreach (var httpHeader in this)
            {
                stringBuilder.AppendLine(httpHeader.ToString());
            }

            return stringBuilder.ToString();
        }
    }
}