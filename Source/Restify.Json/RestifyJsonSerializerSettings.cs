namespace JanHafner.Restify.Json
{
    using Newtonsoft.Json;

    /// <summary>
    /// Defines the default <see cref="JsonSerializerSettings"/> class used by this module.
    /// </summary>
    public sealed class RestifyJsonSerializerSettings : JsonSerializerSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestifyJsonSerializerSettings"/> class.
        /// </summary>
        public RestifyJsonSerializerSettings()
        {
            this.ContractResolver = new JsonPropertyOnlyContractResolver();
            this.DefaultValueHandling = DefaultValueHandling.Ignore;
        }
    }
}