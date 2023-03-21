using Amazon.TranscribeService.Model;
using JobConsumer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JobConsumer
{
    public class SentencesValidator
    {
        public SentencesValidator() { }

        public Task<List<SentenceResult>> UpdateSentenceResults(Stream transcriptionJob,List<string> sentences) 
        {
            var source = ConvertStreamToString(transcriptionJob);
            var results = (from sentence in sentences
                           select ValidateSentence(source, sentence) into res
                           where res != null
                           select res).ToList();

            return Task.FromResult(results);
        }

        private SentenceResult ValidateSentence(string source, string sentence) 
        {
            var regex = new Regex(sentence, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var matches = regex.Matches(source);
            if (matches.Count > 0) 
            {
                var matche = matches[0];
                return new SentenceResult()
                {
                    WasPresent = true,
                    PlainText = sentence,
                    StartIndex = matche.Index,
                    EndIndex = matche.Index + sentence.Length
                };
            }
            return null;
        }

        private string ConvertStreamToString(Stream transcriptionJob) 
        {
            StreamReader reader = new StreamReader(transcriptionJob);
            string text = reader.ReadToEnd();
            return text;
        }
    }
}
