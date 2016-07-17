using Conarh_2016.Application.Domain.JsonConverters;
using Newtonsoft.Json;

namespace Conarh_2016.Application.Domain.Kinvey
{
    [JsonConverter(typeof(TypedJsonConverter<KinveyMetaData>))]
    public sealed class KinveyMetaData
    {
        public const string LastModifiedTimePropertyName = "Lmt";
        public const string EntityCreationTimePropertyName = "Ect";
        public const string AuthTokenPropertyName = "AuthToken";

        [JsonProperty(JsonKeys.Ect)]
        public string Ect
        {
            set;
            get;
        }

        [JsonProperty(JsonKeys.Lmt)]
        public string Lmr
        {
            set;
            get;
        }

        [JsonProperty(JsonKeys.Authtoken)]
        public string Authtoken
        {
            set;
            get;
        }

        public new static class JsonKeys
        {
            public const string Ect = "ect";
            public const string Lmt = "lmt";
            public const string Authtoken = "authtoken";
        }
    }
}