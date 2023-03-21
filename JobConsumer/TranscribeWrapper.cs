using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.TranscribeService;
using Amazon.TranscribeService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JobConsumer
{
    public class TranscribeWrapper
    {
        private const string BUCKET_NAME = "my-serverless-app-t";
        private readonly IAmazonTranscribeService _amazonTranscribeService;
        private readonly TransferUtility _transferUtility;

        public TranscribeWrapper()
        {
            _amazonTranscribeService = new AmazonTranscribeServiceClient();
            _transferUtility = new TransferUtility(new AmazonS3Client());
        }

        public async Task<Stream> GetTranscription(string jobName, string mediaFileUri)
        {
            var startTranscriptionJobResponse = await StartTranscriptionJob( jobName, mediaFileUri);
            Console.WriteLine($"Status of start transcription job request: {startTranscriptionJobResponse}");
            if (await PollTranscriptionJob(jobName))
            {
                return await _transferUtility.OpenStreamAsync(BUCKET_NAME, $"{jobName}.json");
            }
            else
            {
                Console.WriteLine("Transcription job failed");
                throw new Exception($"Transcription job failed for {jobName}");
            }
        }



        private async Task<HttpStatusCode> StartTranscriptionJob(string jobName, string mediaFileUri)
        {
            var response = await _amazonTranscribeService.StartTranscriptionJobAsync(
                new StartTranscriptionJobRequest()
                {
                    TranscriptionJobName = jobName,
                    Media = new Media()
                    {
                        MediaFileUri = mediaFileUri
                    },
                    LanguageCode = LanguageCode.EnUS,
                    OutputBucketName = BUCKET_NAME
                });
            return response.HttpStatusCode;
        }

        private async Task<bool> PollTranscriptionJob(string jobName)
        {
            while (true)
            {
                var getTranscriptionJobResponse = await _amazonTranscribeService.GetTranscriptionJobAsync(new GetTranscriptionJobRequest()
                {
                    TranscriptionJobName = jobName
                });

                var transcriptionJobStatus = getTranscriptionJobResponse.TranscriptionJob.TranscriptionJobStatus;

                if (transcriptionJobStatus == TranscriptionJobStatus.COMPLETED)
                {
                    Console.WriteLine("Transcription job completed");
                    Console.WriteLine($"Output is available at {getTranscriptionJobResponse.TranscriptionJob.Transcript.TranscriptFileUri}");
                    return true;
                }
                else if (transcriptionJobStatus == TranscriptionJobStatus.FAILED)
                {
                    Console.WriteLine("Transcription job failed");
                    return false;
                }
                else
                {
                    Console.WriteLine("Transcription job not completed yet");
                }
                await Task.Delay(5000);
            }
        }
    }
}
