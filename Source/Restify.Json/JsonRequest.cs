namespace JanHafner.Restify.Json
{
    using System;
    using System.Net.Http;
    using System.Text;
    using Request;

    using Newtonsoft.Json;

    /// <summary>
    /// Base class for all Json requests.
    /// </summary>
    public abstract class JsonRequest : RequestBase
    {
        /// <summary>
        /// Sets headers and addition information regarding the body of the request.
        /// </summary>
        /// <param name="restRequest">The <see cref="HttpRequestMessage"/> to prepare.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="restRequest"/>' cannot be null. </exception>
        public override void PrepareRequestContent(HttpRequestMessage restRequest)
        {
            if (restRequest == null)
            {
                throw new ArgumentNullException(nameof(restRequest));    
            }

            restRequest.Content = new StringContent(JsonConvert.SerializeObject(this.GetContent()), Encoding.UTF8, "application/json");
        }
    }
}