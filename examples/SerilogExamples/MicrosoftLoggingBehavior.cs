using GenericPipeline;
using Microsoft.Extensions.Logging;

namespace SerilogExamples;

public class MicrosoftLoggingBehavior : PipelineBehavior
{
    private readonly ILogger _logger;

    public MicrosoftLoggingBehavior(ILogger logger)
    {
        _logger = logger;
    }

    public override TResponse Handle<TRequest, TResponse>(TRequest request)
    {
        var response = HandleNext<TRequest, TResponse>(request);
        _logger.LogInformation(
            "Recieved {Request} and responded with {Response}\n",
            request,
            response);
        return response;
    }
}

