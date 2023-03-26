namespace GenericPipeline.Tests.Behaviors.MediatorBehaviorTests;

public class RemoveHandlersTests
{
    public record struct RequestA : IRequest<string>;
    public record struct RequestB : IRequest;
    public record struct RequestC : IRequest<string>;
    public record struct RequestD : IRequest<string>;

    public class RequestHandlerABC
        : IRequestHandler<RequestA, string>,
          IRequestHandler<RequestB>,
          IRequestHandler<RequestC, string>
    {
        public string Handle(RequestA request) => nameof(RequestA);
        public Unit Handle(RequestB request) => Unit.Value;
        public string Handle(RequestC request) => nameof(RequestC);
    }

    public class RequestHandlerD
        : IRequestHandler<RequestD, string>
    {
        public string Handle(RequestD request) => nameof(RequestD);
    }

    [Fact]
    public void Removing_the_handler_by_a_request_type_can_still_be_invoked_with_other_request_types()
    {
        // Arrange
        var handlerABC = new RequestHandlerABC();
        var handlerD = new RequestHandlerD();
        var mediatorBehavior = new MediatorBehavior()
                .AddHandler(handlerABC)
                .AddHandler(handlerD);
        var pipeline = new Pipeline()
            .AppendBehavior(mediatorBehavior)
            .ThrowOnUnhandledRequest();

        // Act
        mediatorBehavior
            .RemoveHandlerByRequestType<RequestA, string>()
            .RemoveHandlerByRequestType<RequestB>();

        // Assert
        pipeline.GetHandler<RequestHandlerABC>()
            .Should().Be(handlerABC, "because it was removed for a single request only");
        pipeline.Invoking(x => x.Send<RequestA, string>(new()))
            .Should().Throw<UnhandledRequestException>("because the handler was removed for this request");
        pipeline.Invoking(x => x.Send<RequestB>(new()))
            .Should().Throw<UnhandledRequestException>("because the handler was removed for this request");
        var requestCResponse = pipeline.Send<RequestC, string>(new());
        requestCResponse.Should().Be(nameof(RequestC), "because the handler was not removed for this request");
    }

    [Fact]
    public void Removing_the_handler_completely_makes_it_uninvokable_for_all_requests()
    {
        // Arrange
        var handlerABC = new RequestHandlerABC();
        var handlerD = new RequestHandlerD();
        var mediatorBehavior = new MediatorBehavior()
            .AddHandler(handlerABC)
            .AddHandler(handlerD);
        var pipeline = new Pipeline()
            .AppendBehavior(mediatorBehavior)
            .ThrowOnUnhandledRequest();

        // Act
        mediatorBehavior.RemoveHandlerByHandlerType<RequestHandlerABC>();

        // Assert
        pipeline.Invoking(x => x.GetHandler<RequestHandlerABC>())
            .Should().Throw<HandlerNotFoundException>("because the handler was completely removed");
        pipeline.Invoking(x => x.Send<RequestA, string>(new()))
            .Should().Throw<UnhandledRequestException>("because the handler was removed");
        pipeline.Invoking(x => x.Send<RequestB>(new()))
            .Should().Throw<UnhandledRequestException>("because the handler was removed");
        pipeline.Invoking(x => x.Send<RequestC, string>(new()))
            .Should().Throw<UnhandledRequestException>("because the handler was removed");
        var requestDResponse = pipeline.Send<RequestD, string>(new());
        requestDResponse.Should().Be(nameof(RequestD), "because other handlers were not removed");
    }
}

