﻿using HandleJob.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HandleJob.Models
{
    public class Job
    {
        public Job(NewJobReqeust reqeust)
        {
            AudioUrl = reqeust.AudioUrl;
            Sentences = (from sentence in reqeust.Sentences
                         select new SentenceResult()
                         {
                             PlainText = sentence
                         }).ToList();
        }

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
