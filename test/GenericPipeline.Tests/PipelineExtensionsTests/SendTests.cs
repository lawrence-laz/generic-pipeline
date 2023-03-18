namespace GenericPipeline.Tests.PipelineExtensionsTests;

public class SendTests
{
    public record struct TestRequestWithoutResponse() : IRequest;
    public record struct TestRequestWithResponse() : IRequest<int>;

    public class TestRequestHandler
        : IRequestHandler<TestRequestWithoutResponse>,
        IRequestHandler<TestRequestWithResponse, int>
    {
        public int RequestWithoutResponseCount { get; set; }
        public int RequestWithResponseCount { get; set; }

        public Unit Handle(TestRequestWithoutResponse request)
        {
            RequestWithoutResponseCount++;
            return Unit.Value;
        }

        public int Handle(TestRequestWithResponse request)
        {
            RequestWithResponseCount++;
            return 123;
        }
    }

    [Fact]
    public void Send_request_by_object_reference_without_response()
    {
        // Arrange
        var handler = new TestRequestHandler();
        var pipeline = new Pipeline()
            .AppendHandler(handler);
        object request = new TestRequestWithoutResponse();

        // Act
        var actual = pipeline.Send(request);

        // Assert
        actual.Should().Be(Unit.Value);
        handler.RequestWithoutResponseCount.Should().Be(1);
        handler.RequestWithResponseCount.Should().Be(0);
    }

    [Fact]
    public void Send_request_by_object_reference_with_response()
    {
        // Arrange
        var handler = new TestRequestHandler();
        var pipeline = new Pipeline()
            .AppendHandler(handler);
        object request = new TestRequestWithResponse();

        // Act
        var actual = pipeline.Send(request);

        // Assert
        actual.Should().Be(123);
        handler.RequestWithoutResponseCount.Should().Be(0);
        handler.RequestWithResponseCount.Should().Be(1);
    }
}
