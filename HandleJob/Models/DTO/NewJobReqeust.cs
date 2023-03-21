using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HandleJob.Models.DTO
{
    public class NewJobReqeust
    {
        [JsonPropertyName("audio_url")]
        public string AudioUrl { get; set; }

        [JsonPropertyName("sentences")]
        public List<string> Sentences { get; set; }
    }
}
