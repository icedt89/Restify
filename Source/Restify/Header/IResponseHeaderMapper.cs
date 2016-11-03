namespace JanHafner.Restify.Header
{
    using JetBrains.Annotations;
    using Response;

    /// <summary>
    /// Maps a <see cref="HttpHeaderCollection"/> to a <see cref="ResponseBase"/>.
    /// </summary>
    public interface IResponseHeaderMapper
    {
        /// <summary>
        /// Maps all <see cref="HttpHeader"/> instances of the supplied <see cref="HttpHeaderCollection"/> to the supplied <see cref="ResponseBase"/>.
        /// </summary>
        /// <param name="responseBase">The <see cref="ResponseBase"/>.</param>
        /// <param name="httpHeaderCollection">The <see cref="HttpHeaderCollection"/>.</param>
        void MapHeaders([NotNull] ResponseBase responseBase, [NotNull] HttpHeaderCollection httpHeaderCollection);
    }
}