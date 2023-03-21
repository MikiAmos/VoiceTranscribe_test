using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Core;
using HandleJob.Models;
using HandleJob.Models.DTO;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace HandleJob;

public class Function
{
    private readonly DynamoDBContext _dynamoDbContext;

    public Function()
    {
        _dynamoDbContext = new DynamoDBContext(new AmazonDynamoDBClient());
    }

    [LambdaFunction]
    [HttpApi(LambdaHttpMethod.Get,"/")]
    public async Task<string> IsAlive()
    {
        return "Service is up and runing!!!!";
    }

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    [LambdaFunction]
    [HttpApi(LambdaHttpMethod.Get,"/jobs/{jobId}")]
    public async Task<Job> GetJobAsync(string jobId)
    {
        Guid.TryParse(jobId, out var id);
        var job = await _dynamoDbContext.LoadAsync<Job>(id);
        return job;
    }

    [LambdaFunction]
    [HttpApi(LambdaHttpMethod.Post,"/jobs")]
    public async Task<string> StartNewJob([FromBody]NewJobReqeust jobReqeust)
    {
        var newJob = new Job(jobReqeust);
        await _dynamoDbContext.SaveAsync(newJob);
        return newJob.Id.ToString();
    }
}
