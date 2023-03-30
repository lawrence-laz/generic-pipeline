using System;
using FluentAssertions;
using GenericPipeline;
using Polly;
using Xunit;

namespace PollyExamples;

public class RetryPolicyExample
{
    [Fact]
    public void Use_Polly_policies_as_behaviors_in_pipeline()
    {
        var pipeline = new Pipeline()
            .AppendPolicy(Policy.Handle<Exception>().WaitAndRetry(retryCount: 1, _ => TimeSpan.FromSeconds(1)))
            .AppendHandler<TestHandler>();

        var response = pipeline.Send<TestRequest, string>(new());

        response.Should().Be("Hello, world!");
    }
}

public record struct TestRequest() : IRequest<string>;

public class TestHandler : IRequestHandler<TestRequest, string>
{
    private bool _shouldThrow = true;

    public string Handle(TestRequest request)
    {
        if (_shouldThrow)
        {
            _shouldThrow = false;
            throw new Exception();
        }

        return "Hello, world!";
    }
}

public class PollyBehavior : PipelineBehavior
{
    private readonly Policy _policy;

    public PollyBehavior(Policy policy)
    {
        _policy = policy;
    }

    public override TResponse Handle<TRequest, TResponse>(TRequest request)
    {
        return _policy.Execute<TResponse>(() => HandleNext<TRequest, TResponse>(request));
    }
}

public static class PollyPipelineExtensions
{
    public static Pipeline AppendPolicy(this Pipeline pipeline, Policy policy)
    {
        return pipeline.AppendBehavior(new PollyBehavior(policy));
    }
}

