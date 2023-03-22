namespace GenericPipeline.Tests.Behaviors.SingleHandlerBehaviorTests;

public class OneHandlerTests
{
    public record struct Request : IRequest;

    public class RequestHandler : IRequestHandler<Request>
    {
        public int InvocationsCount { get; set; }
        public Unit Handle(Request request)
        {
            ++InvocationsCount;
            return Unit.Value;
        }
    }

    [Fact]
    public void Sending_a_request_it_is_handled_by_the_handler()
    {
        // Arrange
        var pipeline = new Pipeline()
            .AppendHandler<RequestHandler>();

        // Act
        pipeline.Send<Request>(new());

        // Assert

        pipeline.GetHandler<RequestHandler>().InvocationsCount.Should().Be(1);
    }
}

