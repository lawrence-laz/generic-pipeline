public record struct HelloRequest(string Name) : IRequest<string>;

public class HelloHandler : IRequestHandler<HelloRequest, string>
{
    public string Handle(HelloRequest request) => $"Hello, {request.Name}";
}

public class CountingBehavior : PipelineBehavior
{
    public int Counter { get; private set; }

    public CountingBehavior(PipelineBehavior next) : base(next) { }

    public override TResponse Handle<TRequest, TResponse>(TRequest request)
    {
        Counter++;
        return HandleNext<TRequest, TResponse>(request);
    }
}

public sealed class Pipeline
{
    private PipelineBehavior? _behavior;

    public Pipeline AddBehavior<TBehavior>(TBehavior behavior)
        where TBehavior : PipelineBehavior
    {
        _behavior ??= behavior;

        return this;
    }

    public TResponse Send<TRequest, TResponse>(TRequest request)
        where TRequest : IRequest<TResponse>
    {
        if (_behavior is null)
        {
            throw new System.Exception("TODO");
        }

        return _behavior.Handle<TRequest, TResponse>(request);
    }
}

public class SingleBehaviorTests
{
    [Theory, AutoData]
    public void Send_request_to_pipeline_with_counting_behavior_and_simple_dispatcher(
        string name)
    {
        // Arrange
        // var sut = new Pipeline()
        //     .AddBehavior()
        var sut = new CountingBehavior(new SimpleDispatcher<HelloHandler>(new()));
        var expected = $"Hello, {name}";

        // Act
        var actual = sut.Handle<HelloRequest, string>(new(name));

        // Assert
        actual.Should().Be(expected);
        sut.Counter.Should().Be(1, "because the pipeline handled a single request");
    }
}

