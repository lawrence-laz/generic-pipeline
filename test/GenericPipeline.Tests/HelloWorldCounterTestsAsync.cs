public record struct HelloRequestAsync(string Name) : IRequest<Task<string>>;

public class HelloHandlerAsync : IRequestHandlerAsync<HelloRequestAsync, string>
{
    public async Task<string> Handle(HelloRequestAsync request)
    {
        await Task.Yield();
        return $"Hello, {request.Name}";
    }
}

public class CountingBehaviorAsync : PipelineBehaviorAsync
{
    public int Counter { get; private set; }

    public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request)
    {
        Counter++;
        return await HandleNext<TRequest, TResponse>(request);
    }
}

public class HelloWorldCounterTestsAsync
{
    [Theory, AutoData]
    public async Task Send_hello_once_should_get_counted_by_counter_behavior(
        string name)
    {
        // Arrange
        var sut = new PipelineAsync()
            .AppendBehavior<CountingBehaviorAsync>()
            .AppendHandler<HelloHandlerAsync>();

        // Act
        var actual = await sut.Send<HelloRequestAsync, string>(new(name));

        // Assert
        actual.Should().Be($"Hello, {name}");
        sut.GetBehavior<CountingBehaviorAsync>()
            .Counter.Should().Be(1, "because the pipeline handled a single request");
    }

    [Theory, AutoData]
    public async Task Send_hello_n_times_should_get_counted_by_counter_behavior(
        string name,
        int expectedCount)
    {
        // Arrange
        var sut = new PipelineAsync()
            .AppendBehavior<CountingBehaviorAsync>()
            .AppendHandler<HelloHandlerAsync>();
        var actuals = new List<string>();

        // Act
        for (var i = 0; i < expectedCount; ++i)
        {
            actuals.Add(await sut.Send<HelloRequestAsync, string>(new(name)));
        }

        // Assert
        actuals.Should().AllBe($"Hello, {name}");
        sut.GetBehavior<CountingBehaviorAsync>()
            .Counter.Should().Be(expectedCount, $"because the pipeline handled {expectedCount} requests");
    }
}

