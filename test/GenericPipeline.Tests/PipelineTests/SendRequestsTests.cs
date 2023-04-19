
namespace GenericPipeline.Tests.PipelineTests;

public class SendRequestsTests
{
    public record struct RequestA() : IRequest;
    public record struct RequestB() : IRequest<int>;

    public class TestHandler :
        IRequestHandler<RequestA>,
        IRequestHandler<RequestB, int>
    {
        public void Handle(RequestA request) { }
        public int Handle(RequestB request) => 0;
    }

    public class TestBehavior : PipelineBehavior
    {
        public int InvocationsCount { get; set; }
        public override TResponse Handle<TRequest, TResponse>(TRequest request)
        {
            ++InvocationsCount;
            return HandleNext<TRequest, TResponse>(request);
        }
    }

    [Fact]
    public void Send_without_behaviors_throws()
    {
        // Arrange
        var pipeline = new Pipeline();

        // Act
        var actWithoutReturn = () => pipeline.Send<RequestA>(new());
        var actWithReturn = () => pipeline.Send<RequestB, int>(new());

        // Assert
        actWithoutReturn.Should().Throw<InvalidOperationException>();
        actWithReturn.Should().Throw<InvalidOperationException>();
    }


    [Fact]
    public void Send_without_handlers_returns_default()
    {
        // Arrange
        var pipeline = new Pipeline()
            .AppendBehavior<TestBehavior>();

        // Act
        var actual = pipeline.Send<RequestB, int>(new());

        // Assert
        actual.Should().Be(default);
    }
}

