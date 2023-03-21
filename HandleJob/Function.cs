using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Core;
using Amazon.Runtime.Internal;
using Amazon.SQS;
using Amazon.TranscribeService;
using HandleJob.Models;
using HandleJob.Models.DTO;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HandleJob;

public class Function
{
    private readonly DynamoDBContext _dynamoDbContext;
    private readonly TranscribeWrapper _transcribeService;
    private readonly IAmazonSQS _amazonSQSClient;


    public Function()
    {
        _dynamoDbContext = new DynamoDBContext(new AmazonDynamoDBClient());
        _transcribeService = new TranscribeWrapper();
        _amazonSQSClient = new AmazonSQSClient();
    }

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    [LambdaFunction]
    [HttpApi(LambdaHttpMethod.Get,"/jobs/{jobId}")]
    public async Task<Job> FunctionHandler(string jobId, ILambdaContext context)
    {
        Guid.TryParse(jobId, out var id);
        var job = await _dynamoDbContext.LoadAsync<Job>(id);
        return job;
    }

    [LambdaFunction]
    [HttpApi(LambdaHttpMethod.Post, "/jobs")]
    public async Task<string> FunctionHandlerPost([FromBody] NewJobReqeust jobReqeust, ILambdaContext context) 
    {
        var newJob = new Job()
        {
            Id = Guid.NewGuid(),
            AudioUrl = jobReqeust.AudioUrl,
            Sentences = (from sentence in jobReqeust.Sentences
                         select new SentenceResult()
                         {
                             PlainText = sentence
                         }).ToList()
        };
        await _dynamoDbContext.SaveAsync(newJob);
        await _amazonSQSClient.SendMessageAsync("https://sqs.us-east-1.amazonaws.com/550130779966/jobs", newJob.Id.ToString());

        //var response = await _transcribeService.StartTranscriptionJob(newJob.Id.ToString(), newJob.AudioUrl, MediaFormat.Wav, LanguageCode.HeIL);
        return newJob.Id.ToString();
    }
}
