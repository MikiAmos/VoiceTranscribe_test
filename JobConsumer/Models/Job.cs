using System.Text.Json.Serialization;

namespace JobConsumer.Models
{
    public class Job
    { 

        [JsonPropertyName("id")]
        public Guid Id { get; init; } = Guid.NewGuid();

        [JsonPropertyName("status")]
        public JobStatus Status { get; set; }

        [JsonPropertyName("audio_url")]
        public string AudioUrl { get; set; }

        [JsonPropertyName("sentences")]
        public List<SentenceResult> Sentences { get; set; }

    }
}
