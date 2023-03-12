using GenericPipeline;
using Serilog.Core;
using Serilog.Events;

namespace SerilogExamples;

public class SerilogLoggingBehavior : PipelineBehavior
{
    private readonly Logger _logger;

    public SerilogLoggingBehavior(Serilog.Core.Logger logger)
    {
        _logger = logger;
    }

    public override TResponse Handle<TRequest, TResponse>(TRequest request)
    {
        var response = HandleNext<TRequest, TResponse>(request);
        _logger.Write(
            LogEventLevel.Information,
            "Recieved {Request} and responded with {Response}\n",
            request,
            response);
        return response;
    }
}

