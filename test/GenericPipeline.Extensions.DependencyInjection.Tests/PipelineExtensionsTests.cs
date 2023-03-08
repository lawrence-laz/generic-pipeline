namespace GenericPipeline.Extensions.DependencyInjection.Tests;

public class PipelineExtensionsTests
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .AddSingleton<InjectedDependency>()
            .AddSingleton<TestBehavior>()
            .AddSingleton<TestHandler>()
            .BuildServiceProvider();
        var expectedDependency = serviceProvider.GetRequiredService<InjectedDependency>();

        // Act
        var sut = new Pipeline()
            .AppendBehavior<TestBehavior>(serviceProvider)
            .AppendHandler<TestHandler>(serviceProvider);

        var actual = sut.Send<TestRequest, TestResponse>(new());

        // Assert
        actual.DependencyFromBehavior.Should().Be(expectedDependency);
        actual.DependencyFromHandler.Should().Be(expectedDependency);
    }
}

public class InjectedDependency
{
}

public record struct TestRequest(string Name) : IRequest<TestResponse>;
public record struct TestResponse(InjectedDependency? DependencyFromBehavior, InjectedDependency? DependencyFromHandler);

public class TestHandler : IRequestHandler<TestRequest, TestResponse>
{
    public TestHandler(InjectedDependency injectedDependency)
    {
        InjectedDependency = injectedDependency;
    }

    public InjectedDependency InjectedDependency { get; }

    public TestResponse Handle(TestRequest request) => new TestResponse(DependencyFromBehavior: null, DependencyFromHandler: InjectedDependency);
}

public class TestBehavior : PipelineBehavior
{
    public TestBehavior(InjectedDependency injectedDependency)
    {
        InjectedDependency = injectedDependency;
    }

    public InjectedDependency InjectedDependency { get; }

    public override TResponse Handle<TRequest, TResponse>(TRequest request)
    {
        var response = HandleNext<TRequest, TResponse>(request);
        return response is TestResponse testResponse
            ? (TResponse)(object)(testResponse with { DependencyFromBehavior = InjectedDependency })
            : response;
    }
}

