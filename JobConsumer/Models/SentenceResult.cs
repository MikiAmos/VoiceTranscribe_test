using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JobConsumer.Models
{
    public class SentenceResult
    {
        [JsonPropertyName("plain_text")]
        public string PlainText { get; set; }

        [JsonPropertyName("was_present")]
        public bool WasPresent { get; set; }

        [JsonPropertyName("start_index")]
        public int StartIndex { get; set; }

        [JsonPropertyName("end_index")]
        public int EndIndex { get; set; }
    }
}
