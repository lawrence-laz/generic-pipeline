
namespace GenericPipeline.Tests.PipelineTests;

public class SendRequestsTests
{
    public record struct RequestA() : IRequest;
    public record struct RequestB() : IRequest<int>;

    public class TestHandler :
        IRequestHandler<RequestA>,
        IRequestHandler<RequestB, int>
    {
        public Unit Handle(RequestA request) => Unit.Value;
        public int Handle(RequestB request) => 0;
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
}

