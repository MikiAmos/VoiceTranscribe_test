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

        public Task UpdateSentenceResults(Stream transcriptionJob, List<SentenceResult> sentences)
        {
            var source = ConvertStreamToString(transcriptionJob);
            foreach (var sentenceResult in sentences)
            {
                ValidateSentence(source, sentenceResult);
            }

            return Task.CompletedTask;
        }

        private void ValidateSentence(string source, SentenceResult sentence) 
        {
            var regex = new Regex(sentence.PlainText, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var matches = regex.Matches(source);
            if (matches.Count > 0) 
            {
                var matche = matches[0];
                sentence.WasPresent = true;
                sentence.StartIndex = matche.Index;
                sentence.EndIndex = matche.Length;
            }
        }

        private string ConvertStreamToString(Stream transcriptionJob) 
        {
            StreamReader reader = new StreamReader(transcriptionJob);
            string text = reader.ReadToEnd();
            return text;
        }
    }
}
