namespace GenericPipeline.Tests.Behaviors.MediatorBehaviorAsyncTests;

public class RemoveHandlersTests
{
    public record struct RequestA : IRequest<string>;
    public record struct RequestB : IRequest;
    public record struct RequestC : IRequest<string>;
    public record struct RequestD : IRequest<string>;

    public class RequestHandlerABC
        : IRequestHandlerAsync<RequestA, string>,
          IRequestHandlerAsync<RequestB>,
          IRequestHandlerAsync<RequestC, string>
    {
        public Task<string> Handle(RequestA request, CancellationToken cancellationToken) => Task.FromResult(nameof(RequestA));
        public Task<Unit> Handle(RequestB request, CancellationToken cancellationToken) => Unit.ValueTask;
        public Task<string> Handle(RequestC request, CancellationToken cancellationToken) => Task.FromResult(nameof(RequestC));
    }

    public class RequestHandlerD
        : IRequestHandlerAsync<RequestD, string>
    {
        public Task<string> Handle(RequestD request, CancellationToken cancellationToken) => Task.FromResult(nameof(RequestD));
    }

    [Fact]
    public async Task Removing_the_handler_by_a_request_type_can_still_be_invoked_for_other_request_types()
    {
        // Arrange
        var handlerABC = new RequestHandlerABC();
        var handlerD = new RequestHandlerD();
        var mediatorBehavior = new MediatorBehaviorAsync()
                .AddHandler(handlerABC)
                .AddHandler(handlerD);
        var pipeline = new PipelineAsync()
            .AppendBehavior(mediatorBehavior)
            .ThrowOnUnhandledRequest();

        // Act
        mediatorBehavior.RemoveHandlerByRequestType<RequestA, string>();
        mediatorBehavior.RemoveHandlerByRequestType<RequestB>();

        // Assert
        pipeline.GetHandler<RequestHandlerABC>()
            .Should().Be(handlerABC, "because it was removed for a single request only");
        await pipeline.Invoking(x => x.SendAsync<RequestA, string>(new()))
            .Should().ThrowAsync<UnhandledRequestException>("because the handler was removed for this request");
        await pipeline.Invoking(x => x.SendAsync<RequestB>(new()))
            .Should().ThrowAsync<UnhandledRequestException>("because the handler was removed for this request");
        var requestCResponse = await pipeline.SendAsync<RequestC, string>(new());
        requestCResponse.Should().Be(nameof(RequestC), "because the handler was not removed for this request");
    }

    [Fact]
    public async Task Removing_the_handler_completely_makes_it_uninvokable_for_all_requests()
    {
        // Arrange
        var handlerABC = new RequestHandlerABC();
        var handlerD = new RequestHandlerD();
        var mediatorBehavior = new MediatorBehaviorAsync()
            .AddHandler(handlerABC)
            .AddHandler(handlerD);
        var pipeline = new PipelineAsync()
            .AppendBehavior(mediatorBehavior)
            .ThrowOnUnhandledRequest();

        // Act
        mediatorBehavior.RemoveHandlerByHandlerType<RequestHandlerABC>();

        // Assert
        pipeline.Invoking(x => x.GetHandler<RequestHandlerABC>())
            .Should().Throw<HandlerNotFoundException>("because the handler was completely removed");
        await pipeline.Invoking(x => x.SendAsync<RequestA, string>(new()))
            .Should().ThrowAsync<UnhandledRequestException>("because the handler was removed");
        await pipeline.Invoking(x => x.SendAsync<RequestB>(new()))
            .Should().ThrowAsync<UnhandledRequestException>("because the handler was removed");
        await pipeline.Invoking(x => x.SendAsync<RequestC, string>(new()))
            .Should().ThrowAsync<UnhandledRequestException>("because the handler was removed");
        var requestDResponse = await pipeline.SendAsync<RequestD, string>(new());
        requestDResponse.Should().Be(nameof(RequestD), "because other handlers were not removed");
    }
}

