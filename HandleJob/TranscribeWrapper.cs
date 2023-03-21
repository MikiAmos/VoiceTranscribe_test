using Amazon.TranscribeService;
using Amazon.TranscribeService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandleJob
{
    public class TranscribeWrapper
    {
        private readonly IAmazonTranscribeService _amazonTranscribeService;

        public TranscribeWrapper()
        {
            _amazonTranscribeService = new AmazonTranscribeServiceClient();
        }

        /// <summary>
        /// Start a transcription job for a media file. This method returns
        /// as soon as the job is started.
        /// </summary>
        /// <param name="jobName">A unique name for the transcription job.</param>
        /// <param name="mediaFileUri">The URI of the media file, typically an Amazon S3 location.</param>
        /// <param name="mediaFormat">The format of the media file.</param>
        /// <param name="languageCode">The language code of the media file, such as en-US.</param>
        /// <param name="vocabularyName">Optional name of a custom vocabulary.</param>
        /// <returns>A TranscriptionJob instance with information on the new job.</returns>
        public async Task<TranscriptionJob> StartTranscriptionJob(string jobName, string mediaFileUri,
            MediaFormat mediaFormat, LanguageCode languageCode, string vocabularyName = null)
        {
            var response = await _amazonTranscribeService.StartTranscriptionJobAsync(
                new StartTranscriptionJobRequest()
                {
                    TranscriptionJobName = jobName,
                    Media = new Media()
                    {
                        MediaFileUri = mediaFileUri
                    },
                    MediaFormat = mediaFormat,
                    LanguageCode = languageCode,
                    Settings = vocabularyName != null ? new Settings()
                    {
                        VocabularyName = vocabularyName
                    } : null
                });
            return response.TranscriptionJob;
        }
    }
}
