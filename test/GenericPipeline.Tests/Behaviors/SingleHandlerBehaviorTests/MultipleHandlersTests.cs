namespace GenericPipeline.Tests.Behaviors.SingleHandlerBehaviorTests;

public class MultipleHandlersTests
{
    public record struct RequestA : IRequest;
    public record struct RequestB : IRequest;

    public class RequestAHandler : IRequestHandler<RequestA>
    {
        public int InvocationsCount { get; set; }
        public void Handle(RequestA request)
        {
            ++InvocationsCount;
        }
    }

    public class RequestBHandler : IRequestHandler<RequestB>
    {
        public int InvocationsCount { get; set; }
        public void Handle(RequestB request)
        {
            ++InvocationsCount;
        }
    }

    [Fact]
    public void Sending_first_request_it_is_handled_by_first_handler_and_second_handler_is_not_called()
    {
        // Arrange
        var pipeline = new Pipeline()
            .AppendHandler<RequestAHandler>()
            .AppendHandler<RequestBHandler>();

        // Act
        pipeline.Send<RequestA>(new());

        // Assert
        pipeline.GetHandler<RequestAHandler>().InvocationsCount.Should().Be(1);
        pipeline.GetHandler<RequestBHandler>().InvocationsCount.Should().Be(0);
    }

    [Fact]
    public void Sending_second_request_it_is_handled_by_second_handler_and_first_handler_is_not_called()
    {
        // Arrange
        var pipeline = new Pipeline()
            .AppendHandler<RequestAHandler>()
            .AppendHandler<RequestBHandler>();

        // Act
        pipeline.Send<RequestB>(new());

        // Assert
        pipeline.GetHandler<RequestAHandler>().InvocationsCount.Should().Be(0);
        pipeline.GetHandler<RequestBHandler>().InvocationsCount.Should().Be(1);
    }
}

