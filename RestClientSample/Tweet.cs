using System;
using Newtonsoft.Json;

namespace RestClientSample
{
    public class Tweet
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

    }
}
