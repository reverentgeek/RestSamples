using Newtonsoft.Json;

namespace Rest.Client.Sample
{
    public class Tweet
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

    }
}
