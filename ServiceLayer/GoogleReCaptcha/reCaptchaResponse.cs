using Newtonsoft.Json;


namespace ServiceLayer
{
    public class reCaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public required string ValidatedDateTime { get; set; }

        [JsonProperty("hostname")]
        public required string HostName { get; set; }

        [JsonProperty("error-codes")]
        public required List<string> ErrorCodes { get; set; }
    }
}
