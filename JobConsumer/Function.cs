using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using JobConsumer.Models;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace JobConsumer;

public class Function
{
  
    private readonly TranscribeWrapper _transcribeWrapper;
    private readonly DynamoDBContext _dynamoDbContext;
    private readonly SentencesValidator _sentencesValidator;

    public Function()
    {
        _transcribeWrapper = new TranscribeWrapper();
        _dynamoDbContext = new DynamoDBContext(new AmazonDynamoDBClient());
        _sentencesValidator = new SentencesValidator();
    }


   
    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        foreach(var message in evnt.Records)
        {
            await ProcessMessageAsync(message, context);
        }
    }

    private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
    {
        context.Logger.LogInformation($"Processed message {message.Body}");
        Guid.TryParse(message.Body, out var jobId);
        Job job = await _dynamoDbContext.LoadAsync<Job>(jobId);
        try
        {
            var stream = await _transcribeWrapper.GetTranscription(jobId.ToString(), job.AudioUrl);
            await _sentencesValidator.UpdateSentenceResults(stream, job.Sentences);
            job.Status = JobStatus.Complete;
            await _dynamoDbContext.SaveAsync(job);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            job.Status = JobStatus.Failed;
            await _dynamoDbContext.SaveAsync(job);
            context.Logger.LogInformation($"Processed message failed for {jobId} with error ----> {ex.Message}");
        }
    }
}