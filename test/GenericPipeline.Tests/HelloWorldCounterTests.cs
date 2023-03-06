using System.Collections.Generic;

public record struct HelloRequest(string Name) : IRequest<string>;

public class HelloHandler : IRequestHandler<HelloRequest, string>
{
    public string Handle(HelloRequest request) => $"Hello, {request.Name}";
}

public class CountingBehavior : PipelineBehavior
{
    public int Counter { get; private set; }

    public override TResponse Handle<TRequest, TResponse>(TRequest request)
    {
        Counter++;
        return HandleNext<TRequest, TResponse>(request);
    }
}

public class HelloWorldCounterTests
{
    [Theory, AutoData]
    public void Send_hello_once_should_get_counted_by_counter_behavior(
        string name)
    {
        // Arrange
        var sut = new Pipeline()
            .AppendBehavior<CountingBehavior>()
            .AppendHandler<HelloHandler>();

        // Act
        var actual = sut.Send<HelloRequest, string>(new(name));

        // Assert
        actual.Should().Be($"Hello, {name}");
        sut.GetBehavior<CountingBehavior>()
            .Counter.Should().Be(1, "because the pipeline handled a single request");
    }

    [Theory, AutoData]
    public void Send_hello_n_times_should_get_counted_by_counter_behavior(
        string name,
        int expectedCount)
    {
        // Arrange
        var sut = new Pipeline()
            .AppendBehavior<CountingBehavior>()
            .AppendHandler<HelloHandler>();
        var actuals = new List<string>();

        // Act
        for (var i = 0; i < expectedCount; ++i)
        {
            actuals.Add(sut.Send<HelloRequest, string>(new(name)));
        }

        // Assert
        actuals.Should().AllBe($"Hello, {name}");
        sut.GetBehavior<CountingBehavior>()
            .Counter.Should().Be(expectedCount, $"because the pipeline handled {expectedCount} requests");
    }
}

