using Microsoft.Extensions.DependencyInjection;

namespace GenericPipeline.Tests.Behaviors.MediatorBehaviorTests;

public class DependencyContainerTests
{
    public record struct TestRequest : IRequest;
    public class TestDependency { }
    public class TestHandler : IRequestHandler<TestRequest>
    {
        public TestHandler(TestDependency testDependency)
        {
            TestDependency = testDependency;
        }

        public TestDependency TestDependency { get; }

        public void Handle(TestRequest request) => throw new NotImplementedException();
    }

    [Fact]
    public void Add_handler_from_service_provider()
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .AddSingleton<TestDependency>()
            .AddSingleton<TestHandler>()
            .BuildServiceProvider();
        var expected = serviceProvider.GetRequiredService<TestDependency>();

        // Act
        var actual = new Pipeline()
            .AppendBehavior(new MediatorBehavior().AddHandler<TestHandler>(serviceProvider))
            .GetHandler<TestHandler>()
            .TestDependency;

        // Assert
        actual.Should().Be(expected);
    }
}
